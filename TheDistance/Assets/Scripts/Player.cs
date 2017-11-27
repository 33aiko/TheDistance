﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Controller2D))]
public class Player : NetworkBehaviour
{
	//predefined names
	public string EricWorldName = "EricWorld";
	public string NatalieWorldName = "NatalieWorld";
	public string EricPosName = "EricPos";
	public string NataliePosName = "NataliePos";
	public string ShareWorldName="ShareWorld";
	public string EricAnimator = "Animations/Player_1";
	public string NatalieAnimator = "Animations/Player_2";
	public string EricSpiritAnimator = "Animations/Spirit_1";
	public string NatalieSpiritAnimator = "Animations/Spirit_2";

	public GameObject spirit;
	public Vector3 spiritTargetPos;

	public float interpolateTime = 20;


	public float jumpHeight;
	public float timeToJumpApex;
	public Vector3 curCheckPoint;
	public float moveSpeed;
	public Vector3 velocity;

    // camera parameter
	public Vector2 cameraMin, cameraMax;
	public float cameraOffset;
	float currentCameraOffset = 0; 
	private Vector3 offset;
    public float cameraZoomValue = 0;
    float currentCameraZoomValue = 0;

	public bool[] haveKey;
	public bool[] otherHaveKey;
	public bool checkBothFlag;
	public bool[] EricCheckFlag;
	public bool[] NatalieCheckFlag;
	public int keyNum;

	public NPCTrigger curNPC;
    public Text shareNotificationText = null;
    public KeyController curFragment = null;
    public float shareTextTime = 2.0f;

	public float gravity;
	float jumpVelocity;
	float velocitySmoothing;

	public bool playerJumping = false;
    public bool playerUp = false;
    float jumpTime = 0;

    bool tryShare = false;
	GameObject appearParticle; 

    private int[] finishCheck;

	[HideInInspector]
	public Controller2D controller;

    // object sharing
	[HideInInspector]
	public PlayerCircleCollider pCC;
	[HideInInspector]
    bool selectShareObject = false;

	[HideInInspector]
	GameObject root;

	[HideInInspector]
	AudioManager audioManager;
	GameObject transitionMask; 
    bool playingWalkingMusic = false;

	[HideInInspector]
	Animator animator;
    [HideInInspector]
    float nextIdleTime = 10.0f;

	[HideInInspector]
	public bool canClimbLadder = false;

	Color ericFilter, natalieFilter, currentFilter;


	void Start()
	{
        finishCheck = new int[3];
  
        // init public variables:
        root = GameObject.Find("Root");

		// get components
		audioManager = FindObjectOfType<AudioManager>();
		transitionMask = GameObject.Find ("TransitionMask");
        animator = GetComponent<Animator>();
		controller = GetComponent<Controller2D>();
		pCC = GetComponent<PlayerCircleCollider>();

		// movement offset
		gravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		//have key
		for (int i = 0; i < 3; i++) haveKey[i] = false;
		for (int i = 0; i < 3; i++) otherHaveKey[i] = false;

		// interact with objects
		controller.collisions.interact = false;

        if (!isLocalPlayer)
            return;

        Transform EricStartPoint = root.transform.Find(ShareWorldName + "/" + EricPosName);
        Transform NatalieStartPoint = root.transform.Find(ShareWorldName + "/" + NataliePosName);

		ericFilter = new Color32 (23, 76, 113, 255); 
		natalieFilter = new Color32 (50, 15, 100, 255);

        // initialize local player position and camera filter
        if (isLocalPlayer && isServer)
        {
            transform.position = EricStartPoint.position;
            curCheckPoint = EricStartPoint.position;
			Camera.main.GetComponent<VignetteModify> ().color = ericFilter;
        }
        if (isLocalPlayer && !isServer)
        {
            transform.position = NatalieStartPoint.position;
            curCheckPoint = NatalieStartPoint.position;
			Camera.main.GetComponent<VignetteModify> ().color = natalieFilter;
        }

        // initialize spirit
        spirit = root.transform.Find("Spirit").gameObject;
        //if (spirit.GetComponent<Animator>() == null)
        {
            spirit.AddComponent<Animator>();
            spirit.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(isServer ? EricSpiritAnimator : NatalieSpiritAnimator)) as RuntimeAnimatorController;
            spiritTargetPos = spirit.transform.position;
        }
        spirit.SetActive(true);


        // init camera pos
        if (isLocalPlayer)
        {
            offset.z = Camera.main.transform.position.z - transform.position.z;
            Camera.main.transform.position = transform.position + new Vector3(0, cameraOffset, offset.z);
        }

        // init world
        root.transform.Find (EricWorldName).gameObject.SetActive (isServer);
		root.transform.Find (NatalieWorldName).gameObject.SetActive (!isServer);

		//hideRemotePlayer
		if (!isLocalPlayer) {
			// gameObject.SetActive (false);
		} else {
			gameObject.name = "Player";
		}

        ////when client(Natalie) is connected and created, initialize server and itself
        //if (isServer && !isLocalPlayer)
        //{
        //	InitializeServer(NatalieTransform.position,EricTransform.position);
        //	RpcInitializeClient(EricTransform.position,NatalieTransform.position);
        //}

        // initialize player animator
        GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(isServer?EricAnimator:NatalieAnimator)) as RuntimeAnimatorController;
		if (animator == null)
		{
			print("no animation controller found!");
		}

        // get the text
        shareNotificationText = GetComponentInChildren<Text>();
        shareNotificationText.text = "";
    }

	void Update()
	{
        if (!isLocalPlayer)
            return;

		//input controlling move
		KeyControlMove();

		if (isLocalPlayer) {
            //camera
            UpdateCameraPosition();
            //Camera.main.transform.position = new Vector3 (Mathf.Clamp (Camera.main.transform.position.x, cameraMin.x, cameraMax.x), Mathf.Clamp (Camera.main.transform.position.y, cameraMin.y, cameraMax.y), Camera.main.transform.position.z);
        }


        //interpolate move by frame rate, when position not equal, move
		if (!spirit.transform.position.Equals(spiritTargetPos))
		{
            spirit.transform.Translate((spiritTargetPos - spirit.transform.position) / interpolateTime);
        }



    }

    void UpdateCameraPosition()
    {
        if (currentCameraZoomValue != cameraZoomValue)
        {
            currentCameraZoomValue += (cameraZoomValue - currentCameraZoomValue) / interpolateTime;
        }

        CameraFollowBox pCFB = GetComponent<CameraFollowBox>();
        //Vector3 targetCamereaPos = pCFB.focusArea.center - Vector3.up * pCFB.yOffset;

        Vector3 ttmp = pCFB.focusArea.center - Camera.main.transform.position;
        ttmp += new Vector3(0, pCFB.yOffset);
        //Vector3 ttmp = transform.position - Camera.main.transform.position;
        Vector3 moveDistance = new Vector3(ttmp.x, ttmp.y, 0) / interpolateTime;
        GameObject.Find("Main Camera").GetComponent<CameraController>().Move(moveDistance);
        Camera.main.transform.position = 
            new Vector3(Camera.main.transform.position.x,
				Camera.main.transform.position.y
            , transform.position.z + offset.z + currentCameraZoomValue);
    }

    void KeyControlMove(){
		// press Q to interact with the object
		if(Input.GetKey(KeyCode.Q))
			controller.collisions.interact = true;
		else
			controller.collisions.interact = false;

		// press E to view NPC contents
		if(Input.GetButtonDown("Talk"))
		{
			
			foreach (string s in Input.GetJoystickNames()) {
				Debug.Log (s);
			}

			if(curNPC != null)
			{
				GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("NPCcontent");
				print("NPC says: " + curNPC.NPCtalk);
				curNPC.showTalkText();
			}
            else if(curFragment != null)
            {
                curFragment.collectThis(this);
            }
		}

		// press R to close NPC contents 
		if (Input.GetKeyDown (KeyCode.R)) {
			if(curNPC != null)
			{
				curNPC.hideTalkText ();
			}
		}

		// object sharing
        if(Input.GetKeyDown(KeyCode.T))
        {
			audioManager.Play ("StartSharing");
            tryShare = true;

			pCC.highlightNearObject();
			pCC.getDefaultShareObject();

			GameObject shareObject = pCC.getShareObject(); 

			if (shareObject != null) {
				if (isLocalPlayer && isServer) {
					RpcWaitForShare (shareObject.name);
				}
				if (isLocalPlayer && !isServer) {
					CmdWaitForShare (shareObject.name);
				}
                if(shareObject.tag == "Box")
                    shareNotificationText.text = "A box selected, release T to share";
                else if(shareObject.tag == "FloatingPlatform" ||
                    shareObject.tag == "MovingPlatformSharable")
                    shareNotificationText.text = "A platform selected, release T to share";
            }

            selectShareObject = true;
			cameraZoomValue = 40;
			GetComponent<CameraFollowBox>().moveToCenter();

			DOTween.To (() => Camera.main.GetComponent<VignetteModify> ().intensity , (x) => Camera.main.GetComponent<VignetteModify> ().intensity  = x,0.5f,0.5f);
			currentFilter = Camera.main.GetComponent<VignetteModify> ().color;
			Camera.main.GetComponent<VignetteModify> ().color = Color.black;
		

        }

		if (tryShare && !playerJumping) {

		}
			

        if(selectShareObject)
        {
			//if (Input.GetKeyDown(KeyCode.RightArrow))
                //pCC.getNextObject();
        }

        if(Input.GetKeyUp(KeyCode.T))
        {
            tryShare = false;
			cameraZoomValue = -40;
			if (isLocalPlayer && isServer) {
				Camera.main.GetComponent<VignetteModify> ().color = ericFilter;
			}
			if (isLocalPlayer && !isServer) {
				Camera.main.GetComponent<VignetteModify> ().color = natalieFilter;
			}

        }

		if(Input.GetKeyUp(KeyCode.T) && selectShareObject)
		{
			DOTween.To (() => Camera.main.GetComponent<VignetteModify> ().intensity , (x) => Camera.main.GetComponent<VignetteModify> ().intensity  = x,0.3f,0.5f);
            selectShareObject = false;
            tryShare = false;
			pCC.highlightNearObject(false);
			GameObject sharedObject = pCC.shareSelectedObject();
            //print("got somethign with tag: " + sharedObject.tag);
            if(sharedObject == null)
            {
                print("We found a null here!!!!!!!");
            }
            else
            {
                Invoke("clearShareNotificationText", shareTextTime);
                if (sharedObject.tag == "FloatingPlatform")
                {
                    shareNotificationText.text = "A platform shared!";
                    Debug.Log(sharedObject.name);
                    if (isServer && isLocalPlayer)
                    {
                        RpcShare(sharedObject.name);
                    }
                    if (!isServer && isLocalPlayer)
                    {
                        CmdShare(sharedObject.name);
                    }
                    Debug.Log("found");
                    audioManager.Play("ConfirmSharing");
                    animator.SetBool("sendSucceed", true);
                    print("a platform here1");
                    sharedObject.tag = "FloatingPlatformShared";
                }
                else if (sharedObject.tag == "MovingPlatformSharable")
                {
                    shareNotificationText.text = "A platform shared!";
                    Debug.Log("mv!");
                    if (isServer && isLocalPlayer)
                    {
                        RpcShareMv(sharedObject.name);
                    }
                    if (!isServer && isLocalPlayer)
                    {
                        CmdShareMv(sharedObject.name);
                    }
                    audioManager.Play("ConfirmSharing");
                    animator.SetBool("sendSucceed", true);
                }
                else if (sharedObject.tag == "Box")
                {
                    shareNotificationText.text = "A box shared!";
                    Debug.Log("box found");
                    string boxname = sharedObject.name;
                    if (isServer && isLocalPlayer)
                    {
                        RpcBox(sharedObject.name);
                        root.transform.Find("EricWorld").gameObject.transform.Find("WorldA").gameObject.transform.Find(boxname).gameObject.SetActive(false);
                    }
                    if (!isServer && isLocalPlayer)
                    {
                        CmdBox(sharedObject.name);
                        root.transform.Find("NatalieWorld").gameObject.transform.Find("WorldB").gameObject.transform.Find(boxname).gameObject.SetActive(false);
                    }
					sharedObject.tag = "BoxCannotShare";
                    audioManager.Play("ConfirmSharing");
                    animator.SetBool("sendSucceed", true);
                }
            } 
        }

        // on the ground or on the ladder
        if (controller.collisions.above || controller.collisions.below || controller.collisions.onLadder)
        {
            if (playerJumping)
            {
                if (jumpTime > 0.1f)
                    audioManager.Play("PlayerLand");
            }
            playerJumping = false;
            velocity.y = 0;
            jumpTime = 0;
        }
        else
        {
            playerJumping = true;
            jumpTime += Time.deltaTime;
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // move audio
        if (controller.collisions.below && input.x != 0)
        {
            if (!playingWalkingMusic)
            {
                audioManager.Play("PlayerWalking");
                playingWalkingMusic = true;
            }
        }
        else
        {
            audioManager.Stop("PlayerWalking");
            playingWalkingMusic = false;
        }

        // move in y axis if on the ladder
        if (controller.collisions.onLadder)
        {
            print("Can move down!");
            velocity.y = input.y * moveSpeed;
        }

        if (controller.collisions.canClimbLadder && input.y < 0)
        {
            print("Player want to go down!");
            controller.collisions.playerClimbLadder = true;
        }
        else
        {
            controller.collisions.playerClimbLadder = false;
        }

        // jump
        bool keyspaceDown = false;
//        if (Input.GetKeyDown(KeyCode.Space) && (controller.collisions.below && !controller.collisions.onLadder) && (!selectShareObject))
		if (Input.GetButtonDown("Jump") && (controller.collisions.below && !controller.collisions.onLadder) && (!selectShareObject))
        {
            audioManager.Play("PlayerJump");
            velocity.y = jumpVelocity;
            playerJumping = true;
            keyspaceDown = true;
        }

        velocity.x = input.x * moveSpeed;
        if (selectShareObject) velocity.x = 0;

        if (velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (!controller.collisions.onLadder) velocity.y -= gravity * Time.deltaTime;


        // set the animator statemachine

        //print("nextIdletime:" + nextIdleTime);
        playerUp = velocity.y > 0;
        bool playerStand = 
            (velocity.x == 0 && (!playerJumping && !keyspaceDown)
            && controller.collisions.below);
        animator.SetBool("playerJumping", (playerJumping && jumpTime > 0.1f) || keyspaceDown);
        animator.SetBool("playerWalking", (velocity.x != 0));
        animator.SetBool("playerUp", playerUp);
        animator.SetBool("playerStand", playerStand);
        animator.SetBool("playerClimb", controller.collisions.onLadder);
        animator.SetBool("playerPushBox", controller.collisions.pushBox);
        animator.SetBool("hasInput", keyspaceDown || input.x != 0 || tryShare);


        if (playerStand && !tryShare)
        {
            nextIdleTime -= Time.deltaTime;
            if (nextIdleTime < 0)
            {
                animator.SetBool("playerIdle", true);
                nextIdleTime = Random.Range(8.0f, 15.0f);
            }
        }
        else
        {
            nextIdleTime = Random.Range(2.0f, 3.0f);
        }

        if (controller.collisions.onLadder)
        {
            animator.speed = (velocity.y != 0) ? 1.0f : 0;
        }
        else
        {
            animator.speed = 1.0f;
        }

        controller.Move(velocity * Time.deltaTime);


        if (isServer)
        {
            RpcMove(transform.position);
        }
        else
        {
            //print("update cmd move");
            CmdMove(transform.position);
        }
    }

    public void backToCheckPoint()
    {
		transitionMask.GetComponent<TransitionManager> ().BlackTransition ();
        transform.position = curCheckPoint;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    public void checkWho(int keyIdx)
    {
        if (isServer)
        {
            int keyIdxPlus = keyIdx + 1;
            GameObject go = root.transform.Find("EricWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;

            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                return;
            }

            if (go.GetComponent<KeyController>().both[0] != 1)
            {
                audioManager.Play("FragmentOne");
				go.GetComponent<KeyController> ().PlayEffect ();
//                go.transform.DOScale(15, 0.4f);
//                go.transform.DOScale(12, 0.4f).SetDelay(0.8f);
            }
            go.GetComponent<KeyController>().both[0] = 1;

            RpcFrag(keyIdx);


            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
                go.GetComponent<KeyController>().ShowEricMemory();
                //gameObject.SetActive(false);
                // Debug.Log("both key");
				GameObject showDiary = GameObject.Find("UI/Canvas/Diary/StoryBtnList/Scroll View/Viewport/Content/"+keyIdx.ToString());
				Debug.Log ("1show" + keyIdx);
				if (showDiary != null) {
					showDiary.SetActive (true);
				}
            }


            go.GetComponent<KeyController>().setBoth();
        }
        else
        {
            int keyIdxPlus = keyIdx + 1;
            GameObject go = root.transform.Find("NatalieWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;

            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                return;
            }

            if (go.GetComponent<KeyController>().both[1] != 1)
            {
                audioManager.Play("FragmentTwo");
				go.GetComponent<KeyController> ().PlayEffect ();
//                go.transform.DOScale(15, 0.8f);
//                go.transform.DOScale(12, 0.8f).SetDelay(0.8f);
            }

            go.GetComponent<KeyController>().both[1] = 1;
            CmdFrag(keyIdx);

            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
                go.GetComponent<KeyController>().ShowNatalieMemory();
                //gameObject.SetActive(false);
                Debug.Log("both key");
				GameObject showDiary = GameObject.Find("UI/Canvas/Diary/StoryBtnList/Scroll View/Viewport/Content/"+keyIdx.ToString());
				if (showDiary != null) {
					showDiary.SetActive (true);
				}
				Debug.Log ("2show" + keyIdx);
            }

            go.GetComponent<KeyController>().setBoth();
        }
    }

    //sent by server, check on clients
    [ClientRpc]
    public void RpcFrag(int keyIdx)
    {
        if (!isServer)
        {
            int keyIdxPlus = keyIdx + 1;
            GameObject go = root.transform.Find("NatalieWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;

            if (go.GetComponent<KeyController>().both[0] != 1)
            {
                audioManager.Play("FragmentOne");
				go.GetComponent<KeyController> ().PlayEffect ();
//                go.transform.DOScale(15, 0.8f);
//                go.transform.DOScale(12, 0.8f).SetDelay(0.8f);
            }

            go.GetComponent<KeyController>().both[0] = 1;

            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
                go.GetComponent<KeyController>().ShowNatalieMemory();
                //gameObject.SetActive(false);
                Debug.Log("both key");
				GameObject showDiary = GameObject.Find("UI/Canvas/Diary/StoryBtnList/Scroll View/Viewport/Content/"+keyIdx.ToString());
				if (showDiary != null) {
					showDiary.SetActive (true);
				}
			
            }

            go.GetComponent<KeyController>().setBoth();

        }
    }

    //sent by client, check on server
    [Command]
    public void CmdFrag(int keyIdx)
    {
        if (isServer)
        {
            int keyIdxPlus = keyIdx + 1;
            Debug.Log("Ser shoudaole");
            GameObject go = root.transform.Find("EricWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;

            if (go.GetComponent<KeyController>().both[1] != 1)
            {
                audioManager.Play("FragmentTwo");
				go.GetComponent<KeyController> ().PlayEffect ();
//                go.transform.DOScale(15, 0.8f);
//                go.transform.DOScale(12, 0.8f).SetDelay(0.8f);
            }

            go.GetComponent<KeyController>().both[1] = 1;


            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
                go.GetComponent<KeyController>().ShowEricMemory();
                //gameObject.SetActive(false);
                Debug.Log("both key");
				GameObject showDiary = GameObject.Find("UI/Canvas/Diary/StoryBtnList/Scroll View/Viewport/Content/"+keyIdx.ToString());
				if (showDiary != null) {
					showDiary.SetActive (true);
				}
				Debug.Log ("4show" + keyIdx);

            }
            go.GetComponent<KeyController>().setBoth();

        }
    }

    public void Die()
    {
        Debug.Log("Player died!");

        backToCheckPoint();


        //call another player die
        if (isServer)
        {
            Debug.Log("make nata die");
            RpcDie();
        }
        else
        {
            Debug.Log("make eric die");
            CmdDie();
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //check if both players get the fragment
    public bool checkBothKey(int keyIdx)
    {
        if (!haveKey[keyIdx])
        {
            return false;
        }
        if (isServer)
        {
            RpcCheckBoth(keyIdx);
        }
        else
        {
            CmdCheckBoth(keyIdx);
        }
        return otherHaveKey[keyIdx];
    }

    /**
     * 
     */
    //sent by server, check on clients
    [ClientRpc]
    public void RpcCheckBoth(int keyIdx)
    {
        if (!isServer)
        {
            Debug.Log("wo chi" + keyIdx);
            Player p = GetComponent<Player>();
            p.otherHaveKey[keyIdx] = true;
        }
    }

    //sent by client, check on server
    [Command]
    public void CmdCheckBoth(int keyIdx)
    {
        if (isServer)
        {
            Debug.Log("wo chii" + keyIdx + name);
            Player p = GetComponent<Player>();
            p.otherHaveKey[keyIdx] = true;
        }
    }



    //sent by server, show object on clients
    [ClientRpc]
    public void RpcShare(string sharedObject)
    {
        if (isServer) { return; }
        Debug.Log(sharedObject + " read");
		if (appearParticle != null) {
			appearParticle.GetComponent<SharingEffectsController> ().StopSelectedEffect ();
		}
        GameObject sObj = root.transform.Find("EricWorld/WorldA/" + sharedObject).gameObject;
        sObj.SetActive(true);
        GameObject newObj = Instantiate(sObj);
        newObj.transform.position = sObj.transform.position;
        newObj.tag = "FloatingPlatformShared";
		newObj.GetComponentInChildren<SharingEffectsController> ().StopAll ();
    }

    //sent by client, show object on server
    [Command]
    public void CmdShare(string sharedObject)
    {
        Debug.Log(sharedObject + " read");
		if (appearParticle != null) {
			appearParticle.GetComponent<SharingEffectsController> ().StopSelectedEffect ();
		}
        GameObject sObj = root.transform.Find("NatalieWorld/WorldB/" + sharedObject).gameObject;
        sObj.SetActive(true);
        GameObject newObj = Instantiate(sObj);
        newObj.transform.position = sObj.transform.position;
        newObj.tag = "FloatingPlatformShared";
		newObj.GetComponentInChildren<SharingEffectsController> ().StopAll ();

       // Debug.Log(newObj.name);
    }


	//sent by server, show particle effects on clients
	[ClientRpc]
	public void RpcWaitForShare(string shareObject){
		if (isServer) {
			return;
		}
		GameObject sObj = root.transform.Find ("EricWorld/WorldA/" + shareObject).gameObject;
		appearParticle = Instantiate (Resources.Load ("Prefabs/Levels/Appeareffect_Red") as GameObject);
		appearParticle.transform.position = sObj.transform.position;
		//appearParticle.transform.parent = sObj.transform;
		appearParticle.GetComponent<SharingEffectsController> ().PlaySelectedEffect ();

	}

	//sent by client, show particle effects on server
	[Command]
	public void CmdWaitForShare(string shareObject){
		GameObject sObj = root.transform.Find("NatalieWorld/WorldB/" + shareObject).gameObject;
		appearParticle = Instantiate (Resources.Load ("Prefabs/Levels/Appeareffect_Blue") as GameObject);
		appearParticle.transform.position = sObj.transform.position;
		//appearParticle.transform.parent = sObj.transform;
		appearParticle.GetComponent<SharingEffectsController> ().PlaySelectedEffect ();
	}


    //sent by server, show object on clients
    [ClientRpc]
    public void RpcShareMv(string sharedObject)
    {
        if (isServer) { return; }
        Debug.Log(sharedObject + " read");
		if (appearParticle != null) {
			appearParticle.GetComponent<SharingEffectsController> ().StopSelectedEffect ();
		}
        GameObject remoteWorld = root.transform.Find("EricWorld").gameObject.transform.Find("WorldA").gameObject;
        GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
        sObj.SetActive(true);
        //sObj.transform.localPosition += sObj.GetComponent<MovingPlatformController> ().targetTranslate;
        GameObject newObj = Instantiate(sObj);
        newObj.transform.position = sObj.transform.position;
		newObj.GetComponentInChildren<SharingEffectsController> ().StopAll ();
        if (!sObj.GetComponent<MovingPlatformController>().isMoved)
        {
            newObj.transform.localPosition += sObj.GetComponent<MovingPlatformController>().targetTranslate;
        }
    }

    //sent by client, show object on server
    [Command]
    public void CmdShareMv(string sharedObject)
    {
        Debug.Log(sharedObject + " read");
		if (appearParticle != null) {
			appearParticle.GetComponent<SharingEffectsController> ().StopSelectedEffect ();
		}
        GameObject remoteWorld = root.transform.Find("NatalieWorld").gameObject.transform.Find("WorldB").gameObject;
        GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
        sObj.SetActive(true);
        //sObj.transform.localPosition += sObj.GetComponent<MovingPlatformController> ().targetTranslate;
        GameObject newObj = Instantiate(sObj);
        newObj.transform.position = sObj.transform.position;
		newObj.GetComponentInChildren<SharingEffectsController> ().StopAll ();
        if (!sObj.GetComponent<MovingPlatformController>().isMoved)
        {
            newObj.transform.localPosition += sObj.GetComponent<MovingPlatformController>().targetTranslate;
        }
    }


    //sent by server, show box on clients
    [ClientRpc]
    public void RpcBox(string sharedObject)
    {
        if (isServer) { return; }
        Debug.Log(sharedObject + " read");
		if (appearParticle != null) {
			appearParticle.GetComponent<SharingEffectsController> ().StopSelectedEffect ();
		}
        GameObject remoteWorld = root.transform.Find("EricWorld").gameObject.transform.Find("WorldA").gameObject;
        GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
        sObj.SetActive(true);
        GameObject newObj = Instantiate(sObj);
		newObj.tag = "BoxCannotShare";
        newObj.transform.position = sObj.transform.position;
		newObj.GetComponentInChildren<SharingEffectsController> ().StopAll ();
        Debug.Log(sObj.name);

    }

    //sent by client, show box on server
    [Command]
    public void CmdBox(string sharedObject)
    {
        Debug.Log(sharedObject + " read");
		if (appearParticle != null) {
			appearParticle.GetComponent<SharingEffectsController> ().StopSelectedEffect ();
		}
        GameObject remoteWorld = root.transform.Find("NatalieWorld").gameObject.transform.Find("WorldB").gameObject;
        GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
        sObj.SetActive(true);
        GameObject newObj = Instantiate(sObj);
		newObj.tag = "BoxCannotShare";
        newObj.transform.position = sObj.transform.position;
		newObj.GetComponentInChildren<SharingEffectsController> ().StopAll ();
        Debug.Log(sObj.name);

    }


    //sent by server, run on all clients
    [ClientRpc]
    public void RpcMove(Vector3 pos)
    {
        //print("Rpc Move");
        if (!isServer)
        {
            GameObject.Find("Player").GetComponent<Player>().spiritTargetPos = pos;
        }
    }

    //sent by client, run on server
    [Command]
    public void CmdMove(Vector3 pos)
    {
        //print("Cmd Move");
        GameObject.Find("Player").GetComponent<Player>().spiritTargetPos = pos;
    }

    public void InitializeServer(Vector3 spirit_pos, Vector3 player_pos)
    {
        //print("CmdIniatiateServer");
        spirit.transform.position = spirit_pos;
        spirit.SetActive(true);
        GameObject.Find("Player").transform.position = player_pos;
        GameObject.Find("Player").GetComponent<Player>().spiritTargetPos = spirit_pos;
    }
    [ClientRpc]
    public void RpcInitializeClient(Vector3 spirit_pos, Vector3 player_pos)
    {
        //print("IniatiateClient");
        spirit.transform.position = spirit_pos;
        spirit.SetActive(true);
        GameObject.Find("Player").transform.position = player_pos;
        GameObject.Find("Player").GetComponent<Player>().spiritTargetPos = spirit_pos;
    }


    //sent by server, make die on clients
    [ClientRpc]
    public void RpcDie()
    {
        if (isServer) { return; }
        Debug.Log("client RPCdie");
		spirit.GetComponent<Animator> ().SetBool ("SpiritDie", true);
        StartCoroutine(WaitToDie());
    }

    //sent by client, make die on server
    [Command]
    public void CmdDie()
    {
        Debug.Log("server CMDdie");
		spirit.GetComponent<Animator> ().SetBool ("SpiritDie", true); 
        StartCoroutine(WaitToDie());
    }

	IEnumerator WaitToDie()
	{
        print("Waiting to die at: " + Time.time);
        GameObject.Find("Player").GetComponent<Animator>().SetTrigger("playerDie");
		yield return new WaitForSeconds (1.5f);
        print("Died at " + Time.time);
		Player p = GameObject.Find("Player").GetComponent<Player>();
		p.backToCheckPoint();
		spirit.transform.position = p.transform.position; 
	}

    public void setCheck(int level)
    {
        finishCheck[level-1] = 1;
    }
    public int getCheck(int level)
    {
        return finishCheck[level-1];
    }
    public void clearCheck(int level)
    {
        finishCheck[level - 1] = 0;
    }

    public void clearShareNotificationText()
    {
        shareNotificationText.text = "";
    }
}
