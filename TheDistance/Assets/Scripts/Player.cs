﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

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
	public Image shareBarBg, shareBar; 
    public KeyController curFragment = null;
    public float shareTextTime = 1.5f;

	public float gravity;
	float jumpVelocity;
	float velocitySmoothing;

	public bool playerJumping = false;
    public bool playerUp = false;
    float jumpTime = 0;

    int sceneState;

    bool tryShare = false;
    float tPressedTime = 0.0f;
    bool tCanShare = false;
    public float tNeededTime = 3.0f;
	GameObject appearParticle; 

	bool showNPCcontent = false; 

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

    bool keyspaceDown;
    Vector2 input;

    RowBoat boat;

    public InputDeviceType currentInputDevice = InputDeviceType.KEYBOARD;

	void Start()
	{
        //sceneState = 1;!!!!
		// boat = GameObject.Find("boat").GetComponent<RowBoat>();

        //Text load
        GameObject UIobject = GameObject.Find("UI");
        TextSystem textSystem = UIobject.GetComponent<TextSystem>();
        if (isServer && isLocalPlayer)
        {
            textSystem.HandAwake(1);
            textSystem.already = 1;
            textSystem.HandStart();
        }
        else if(!isServer && isLocalPlayer)
        {
            textSystem.HandAwake(2);
            textSystem.already = 1;
            textSystem.HandStart();
        }
        

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

        // get the text &BG
        shareNotificationText = GetComponentInChildren<Text>();
        shareNotificationText.text = "";
		if (shareBar != null) {
			shareBar.DOFade (0, 0);
		}
		if (shareBarBg != null) {
			shareBarBg.DOFade (0, 0);
		}

    }

	void Update()
	{
        if (!isLocalPlayer)
            return;
		if (isServer)
		{

		//	RpcBoat();
		//	RpcBoatMove(boat.GetComponent<Transform>().position,boat.GetComponent<Transform>().rotation);
		}
		else
		{

		//	CmdBoat();
		//	CmdBoatMove(boat.GetComponent<Transform>().position, boat.GetComponent<Transform>().rotation);
		}
		//input controlling move
		KeyControlMove();

		if (isLocalPlayer) {
            //camera
            UpdateCameraPosition();
            //Camera.main.transform.position = new Vector3 (Mathf.Clamp (Camera.main.transform.position.x, cameraMin.x, cameraMax.x), Mathf.Clamp (Camera.main.transform.position.y, cameraMin.y, cameraMax.y), Camera.main.transform.position.z);
        }


        //interpolate move by frame rate, when position not equal, move
		if (!spirit.transform.position.Equals (spiritTargetPos)) {
			spirit.transform.Translate ((spiritTargetPos - spirit.transform.position) / interpolateTime);
//			audioManager.Play ("SpiritMove");
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

        handleInput();

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

		//push box audio
		if (controller.collisions.pushBox) {

			audioManager.Stop ("PlayerWalking");
			if (!audioManager.GetSound ("WalkWithBox").source.isPlaying) {
				audioManager.Play ("WalkWithBox");
			}
		} else {
			audioManager.Stop("WalkWithBox");
		}

        // move in y axis if on the ladder
		if (controller.collisions.onLadder) {
			print ("Can move down!");
			velocity.y = input.y * moveSpeed;

			if (input.y != 0) {
				audioManager.Play ("ClimbLadder");
			} else {
				audioManager.Stop ("ClimbLadder");
			}
		} else {
			audioManager.Stop ("ClimbLadder");
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
        keyspaceDown = false;
//        if (Input.GetKeyDown(KeyCode.Space) && (controller.collisions.below && !controller.collisions.onLadder) && (!selectShareObject))
		if (Input.GetButtonDown("Jump") && (controller.collisions.below && !controller.collisions.onLadder) && (!selectShareObject))
        {
            audioManager.Play("PlayerJump");
            velocity.y = jumpVelocity;
            playerJumping = true;
            keyspaceDown = true;
        }

        velocity.x = input.x * moveSpeed;


        updatePlayerAnimator();

        if (!controller.collisions.onLadder) velocity.y -= gravity * Time.deltaTime;
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

    private void handleInput()
    {
        //if boat found
        //press v to exciting
        if (Input.GetKeyDown("v"))
        {
            Debug.Log("V pressed");
            boatControl(boat);
        }
        //else:

		// press Q to interact with the object
		if (Input.GetButton("Push")) {
			controller.collisions.interact = true;
		} else {
			controller.collisions.interact = false;
		}

		// press E to view NPC contents
		if(Input.GetButtonDown("Talk"))
		{

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
		if (Input.GetButtonDown("Submit")) {
			if(curNPC != null)
			{
				curNPC.hideTalkText ();
			}
		}

        tCanShare = false;
		// object sharing
		if(Input.GetButtonDown("Share"))
        {
			audioManager.Play ("StartSharing");
			audioManager.Play ("SharingHold");
            tryShare = true;

			pCC.highlightNearObject();
			pCC.getDefaultShareObject();

			GameObject shareObject = pCC.getShareObject(); 
            print("got " + shareObject.name + " here");

			if (shareObject != null) {
				if (isLocalPlayer && isServer) {
					RpcWaitForShare (shareObject.name);
				}
				if (isLocalPlayer && !isServer) {
					CmdWaitForShare (shareObject.name);
				}
				shareBarBg.DOFade (1, 0);
				shareBar.DOFade (1, 0);
				//start progress bar 
                if(shareObject.tag == "Box")
                    shareNotificationText.text = "Sharing the box";
                else if(shareObject.tag == "FloatingPlatform" ||
                    shareObject.tag == "MovingPlatformSharable")
                    shareNotificationText.text = "Sharing the platform";
                else if(shareObject.tag == "Rock")
                    shareNotificationText.text = "Sharing the rock";
            }

            selectShareObject = true;
			cameraZoomValue = 40;
			GetComponent<CameraFollowBox>().moveToCenter();

			DOTween.To (() => Camera.main.GetComponent<VignetteModify> ().intensity , (x) => Camera.main.GetComponent<VignetteModify> ().intensity  = x,0.5f,0.5f);
			currentFilter = Camera.main.GetComponent<VignetteModify> ().color;
			Camera.main.GetComponent<VignetteModify> ().color = Color.black;
        }

        if(tryShare)
        {
            tPressedTime += Time.deltaTime;
            print("t pressed time is : " + tPressedTime);
        }

        if (Input.GetButtonUp("Share"))
        {
            if(tPressedTime >= tNeededTime)
            {
                tCanShare = true;
                print("t pressed time: " + tPressedTime);
            }
            else
            {
                tCanShare = false;
            }
            if (!tCanShare)
            {
                print("trying to stop sharing");
                pCC.StopSharingEffect();
                if (isLocalPlayer && isServer)
                {
                    print("server trying to stop!");
                    RpcStopShare();
                }
                if (isLocalPlayer && !isServer)
                {
                    CmdStopShare();
                }
            }
            tPressedTime = 0;
            tryShare = false;
			audioManager.Stop ("SharingHold");
			cameraZoomValue = -40;
			if (isLocalPlayer && isServer) {
				Camera.main.GetComponent<VignetteModify> ().color = ericFilter;
			}
			if (isLocalPlayer && !isServer) {
				Camera.main.GetComponent<VignetteModify> ().color = natalieFilter;
			}

        }

		if(Input.GetButtonUp("Share") && selectShareObject)
		{
            selectShareObject = false;
            tryShare = false;
			DOTween.To (() => Camera.main.GetComponent<VignetteModify> ().intensity , (x) => Camera.main.GetComponent<VignetteModify> ().intensity  = x,0.3f,0.5f);
			pCC.highlightNearObject(false);
            Invoke("clearShareNotificationText", shareTextTime);

            if (tCanShare)
            {
                GameObject sharedObject = pCC.shareSelectedObject();
                print(sharedObject.name + " is going to be shared");

                print("got somethign with tag: " + sharedObject.tag);
                if (sharedObject == null)
                {
                    print("We found a null here!!!!!!!");
                }
                else
                {
                    if (sharedObject.tag == "FloatingPlatform")
                    {
                        shareNotificationText.text = "A platform is shared!";
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
                        shareNotificationText.text = "A platform is shared!";
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
                        shareNotificationText.text = "An object is shared!";
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
                    else if (sharedObject.tag == "Rock")
                    {
                        shareNotificationText.text = "A rock is shared";
                        string rockname = sharedObject.name;
                        if (isServer && isLocalPlayer)
                        {
                            RpcRock(sharedObject.name);
                            root.transform.Find("EricWorld").gameObject.transform.Find("WorldA").gameObject.transform.Find(rockname).gameObject.SetActive(false);
                        }
                        if (!isServer && isLocalPlayer)
                        {
                            CmdRock(sharedObject.name);
                            root.transform.Find("NatalieWorld").gameObject.transform.Find("WorldB").gameObject.transform.Find(rockname).gameObject.SetActive(false);
                        }
                        sharedObject.tag = "CannotShare";
                        audioManager.Play("ConfirmSharing");
                        animator.SetBool("sendSucceed", true);
                    }
                }
            }
        }

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.anyKey)
        {
            currentInputDevice = getInputDevice();
            print("Input device is: " + currentInputDevice.ToString());
        }
        else
        {
            if (input.x != 0) // no key input && input != 0 => joystick handle moved
            {
                currentInputDevice = InputDeviceType.PS4;
            }
        }

        if(curNPC != null)
        {
            curNPC.setImage(currentInputDevice == InputDeviceType.KEYBOARD);
        }

        if(curFragment != null)
        {
            curFragment.setImage(currentInputDevice == InputDeviceType.KEYBOARD);
        }
    }

    public enum InputDeviceType
    {
        XBOX,
        KEYBOARD,
        PS4,
        OTHER,
    }

    private InputDeviceType getInputDevice() 
    {
        string k = "";
        if (Input.anyKey)
        {
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKey(kc))
                {
                    if (kc.ToString().Length > 0)
                    {
                        k = kc.ToString();
                        //print(k + " pressed");
                        break;
                    }
                }
            }
        }
        if(k.Contains("Joystick"))
        {
            for(int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                if(Input.GetJoystickNames()[i].Length > 0)
                {
                    return getJoystickName(Input.GetJoystickNames()[i]);
                }
            }
        }
        return InputDeviceType.KEYBOARD;
        
        // TODO: distinguish the controller when both XBOX and PS4 controller is inserted
        // CURRENT: use joystick[0] 
        /*
        if(k.Contains("Joystick1"))
        {
            if(Input.GetJoystickNames().Length > 1)
            {
                string js1Name = Input.GetJoystickNames()[1];
                return getJoystickName(js1Name);
            }
        }
        if(k.Contains("Joystick"))
        {
            if(Input.GetJoystickNames().Length > 0)
            {
                string js0Name = Input.GetJoystickNames()[0];
                return getJoystickName(js0Name);
            }
        }
         */ 
    }

    private InputDeviceType getJoystickName(string str)
    {
        print("Inputstr is :" + str);
        if(str.Contains("Windows"))
        {
            return InputDeviceType.XBOX;
        }
        if(str.Contains("Wireless"))
        {
            return InputDeviceType.PS4;
        }
        return InputDeviceType.OTHER;
    }

    private void updatePlayerAnimator()
    {
        if (selectShareObject) velocity.x = 0;

        if (velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        // set the animator statemachine
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
    }

    public void backToCheckPoint()
    {
        transform.position = curCheckPoint;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
		transitionMask.GetComponent<TransitionManager> ().BlackTransition ();
		transitionMask.GetComponent<TransitionManager> ().transitionTime = 2; 
    }

    /***********************************************************************
     * Fragment status
     ***********************************************************************/
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


    /***********************************************************************
     * 2-player fragment status
     ***********************************************************************/
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


    /***********************************************************************
     * Platform share
     ***********************************************************************/
    //sent by server, show object on clients
    [ClientRpc]
    public void RpcShare(string sharedObject)
    {
        if (isServer) { return; }
        Debug.Log(sharedObject + " read");
		audioManager.Stop ("SharingHold");
		audioManager.Play ("ConfirmSharing");
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
		audioManager.Stop ("SharingHold");
		audioManager.Play ("ConfirmSharing");
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

		audioManager.Play ("SharingHold");
		GameObject sObj = root.transform.Find ("EricWorld/WorldA/" + shareObject).gameObject;
		appearParticle = Instantiate (Resources.Load ("Prefabs/Levels/Appeareffect_Red") as GameObject);

		appearParticle.transform.position = sObj.transform.position;
		if (sObj.GetComponent<MovingPlatformController>()!=null) {
			if (!sObj.GetComponent<MovingPlatformController>().isMoved)
			{
				appearParticle.transform.localPosition += sObj.GetComponent<MovingPlatformController>().targetTranslate;
			}
		}
		appearParticle.GetComponent<SharingEffectsController> ().PlaySelectedEffect ();

	}

	//sent by client, show particle effects on server
	[Command]
	public void CmdWaitForShare(string shareObject){
		audioManager.Play ("SharingHold");
		GameObject sObj = root.transform.Find("NatalieWorld/WorldB/" + shareObject).gameObject;
		appearParticle = Instantiate (Resources.Load ("Prefabs/Levels/Appeareffect_Blue") as GameObject);
		appearParticle.transform.position = sObj.transform.position;
		if (sObj.GetComponent<MovingPlatformController> () != null) {
			if (!sObj.GetComponent<MovingPlatformController> ().isMoved) {
				appearParticle.transform.localPosition += sObj.GetComponent<MovingPlatformController> ().targetTranslate;
			}
		}
		appearParticle.GetComponent<SharingEffectsController> ().PlaySelectedEffect ();
	}

    //sent by server, show particle effects on clients
    [ClientRpc]
    public void RpcStopShare()
    {
        if (isServer)
            return;
        StopShare();
    }

    //sent by client, show particle effects on server
    [Command]
    public void CmdStopShare()
    {
        StopShare();
    }

    void StopShare()
    {
        audioManager.Stop("SharingHold");
        print("stopping share!");
        if (appearParticle != null)
        {
            appearParticle.GetComponent<SharingEffectsController>().StopSelectedEffect();
        }
    }


    /***********************************************************************
     * Moving platform share
     ***********************************************************************/
    //sent by server, show object on clients
    [ClientRpc]
    public void RpcShareMv(string sharedObject)
    {
        if (isServer) { return; }
		audioManager.Stop ("SharingHold");
		audioManager.Play ("ConfirmSharing");
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
		audioManager.Stop ("SharingHold");
		audioManager.Play ("ConfirmSharing");
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

    /***********************************************************************
     * Box share
     ***********************************************************************/
    //sent by server, show box on clients
    [ClientRpc]
    public void RpcBox(string sharedObject)
    {
        if (isServer) { return; }
        Debug.Log(sharedObject + " read");
		audioManager.Stop ("SharingHold");
		audioManager.Play ("ConfirmSharing");
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
		audioManager.Stop ("SharingHold");
		audioManager.Play ("ConfirmSharing");
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

    // sent by server, show rock on clients
    [ClientRpc]
    public void RpcRock(string sharedObject)
    {
        if (isServer) return;
        ShareRock(sharedObject, true);
    }

    // sent by client, show box on server
    [Command]
    public void CmdRock(string sharedObject)
    {
        ShareRock(sharedObject, false);
    }

    void ShareRock(string sharedObject, bool isEricWorld)
    {
        StopShare();
		audioManager.Stop ("SharingHold");
		audioManager.Play ("ConfirmSharing");
        GameObject remoteWorld = root.transform.Find(isEricWorld?"EricWorld":"NatalieWorld").gameObject.transform.Find(isEricWorld?"WorldA":"WorldB").gameObject;
        GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
        sObj.SetActive(true);
        GameObject newObj = Instantiate(sObj);
		newObj.tag = "CannotShare";
        newObj.transform.position = sObj.transform.position;
    }

    /***********************************************************************
     * 2-player position sync
     ***********************************************************************/
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

    /***********************************************************************
     * initialization
     ***********************************************************************/
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

    /***********************************************************************
     * Die sync
     ***********************************************************************/
    public void Die()
    {
        Debug.Log("Player died!");

        audioManager.Play("Death");

        if(transitionMask != null && transitionMask.GetComponent<TransitionManager>() != null)
            transitionMask.GetComponent<TransitionManager>().transitionTime = 7;
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
		yield return new WaitForSeconds (2f);
        print("Died at " + Time.time);
		audioManager.Play ("Death");
		Player p = GameObject.Find("Player").GetComponent<Player>();
		p.backToCheckPoint();
		spirit.transform.position = p.transform.position; 
	}


    /***********************************************************************
     * Step on trigger control
     ***********************************************************************/
    // i don't know why i should put it here
    // but after putting the rpc and cmd command in player   Answer:Because only the player class is NetworkBehavior while others are MonoBehavior!
    // bug fixed!
    // sent by server, run on all clients
    [ClientRpc]
    public void RpcSetPlatformMoveable(bool _canMove, string mpName)
    {
        if (!isServer)
            SetPlatformMovealbe(_canMove, mpName);
    }

    // sent by client, run on server
    [Command]
    public void CmdSetPlatformMoveable(bool _canMove, string mpName)
    {
        print("Got input from client!");
        SetPlatformMovealbe(_canMove, mpName);
    }

    void SetPlatformMovealbe(bool _canMove, string mpName)
    {
        MovingPlatformController pMPC = GameObject.Find(mpName).GetComponent<MovingPlatformController>();
        pMPC.canMove = _canMove;
    }




    /***********************************************************************
     * Boat control
     ***********************************************************************/
    public void boatControl(RowBoat boat)
    {
        if (isServer)
        {
            boat.move(1);
            RpcBoat();
            RpcBoatMove(boat.GetComponent<Transform>().position,boat.GetComponent<Transform>().rotation);
        }
        else
        {
            boat.move(0);
            CmdBoat();
            CmdBoatMove(boat.GetComponent<Transform>().position, boat.GetComponent<Transform>().rotation);
        }
    }
    [ClientRpc]
    public void RpcBoat()
    {
        if (!isServer)
        {
            //
        }
    }

    //sent by client, run on server
    [Command]
    public void CmdBoat()
    {
        //
    }

    //sent by server, run on all clients
    [ClientRpc]
    public void RpcBoatMove(Vector3 pos, Quaternion rot)
    {
        //print("Rpc Move");
        if (!isServer)
        {
            GameObject.Find("boat").GetComponent<RowBoat>().GetComponent<Transform>().position = pos;
            GameObject.Find("boat").GetComponent<RowBoat>().GetComponent<Transform>().rotation = rot;
        }
    }

    //sent by client, run on server
    [Command]
    public void CmdBoatMove(Vector3 pos, Quaternion rot)
    {
        //print("Cmd Move");
        GameObject.Find("boat").GetComponent<RowBoat>().GetComponent<Transform>().position = pos;
        GameObject.Find("boat").GetComponent<RowBoat>().GetComponent<Transform>().rotation = rot;
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
		shareBarBg.DOFade (0, 0);
		shareBar.DOFade (0, 0);
    }
}
