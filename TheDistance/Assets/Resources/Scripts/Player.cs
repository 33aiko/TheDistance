using System.Collections;
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

	public float interpolateTime = 10;


	public float jumpHeight;
	public float timeToJumpApex;
	public Vector3 curCheckPoint;
	public float moveSpeed;
	public Vector3 velocity;

    // camera parameter
	public Vector2 cameraMin, cameraMax;
	public float cameraOffset;
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

	public float gravity;
	float jumpVelocity;
	float velocitySmoothing;

	public bool playerJumping = false;
    public bool playerUp = false;


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
    bool playingWalkingMusic = false;

	[HideInInspector]
	Animator animator;

	[HideInInspector]
	public bool canClimbLadder = false;

	void Start()
	{
        //find root, because spirit is deactived, so we can only use transform to find it.
        root = GameObject.Find("Root");

        if (!isLocalPlayer)
            return;

		// get components
		audioManager = FindObjectOfType<AudioManager>();
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

        Transform EricStartPoint = root.transform.Find(ShareWorldName + "/" + EricPosName);
        Transform NatalieStartPoint = root.transform.Find(ShareWorldName + "/" + NataliePosName);

        // initialize local player position
        if (isLocalPlayer && isServer)
        {
            transform.position = EricStartPoint.position;
            curCheckPoint = EricStartPoint.position;
        }
        if (isLocalPlayer && !isServer)
        {
            transform.position = NatalieStartPoint.position;
            curCheckPoint = NatalieStartPoint.position;
        }

        // initialize spirit
        spirit = root.transform.Find("Spirit").gameObject;
        if (spirit.GetComponent<Animator>() == null)
        {
            spirit.AddComponent<Animator>();
            spirit.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(isServer ? EricSpiritAnimator : NatalieSpiritAnimator)) as RuntimeAnimatorController;
            spiritTargetPos = spirit.transform.position;
        }

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
        Vector3 ttmp = transform.position - Camera.main.transform.position;
        Vector3 moveDistance = new Vector3(ttmp.x, ttmp.y, 0);
        //Vector3 moveDistance = 
            //new Vector3(
                //Mathf.Min(ttmp.x, moveSpeed * Time.deltaTime), 
                //Mathf.Min(ttmp.y, moveSpeed*Time.deltaTime), 0);
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
		if(Input.GetKeyDown(KeyCode.E))
		{
			//print("have NPC in range? " + (curNPC != null));
			if(curNPC != null)
			{
				GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("NPCcontent");
				print("NPC says: " + curNPC.NPCtalk);
				curNPC.showTalkText();
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
			pCC.highlightNearObject();
			pCC.getDefaultShareObject();
            selectShareObject = true;
			Camera.main.GetComponent<VignetteModify> ().intensity = 0.6f;
			audioManager.Play ("StartSharing");
		}

        if(selectShareObject)
        {
			if (Input.GetKeyDown(KeyCode.RightArrow))
                pCC.getNextObject();
        }

		if(Input.GetKeyUp(KeyCode.T))
		{
			Camera.main.GetComponent<VignetteModify> ().intensity = 0;
            selectShareObject = false;
			pCC.highlightNearObject(false);
			GameObject sharedObject = pCC.shareSelectedObject();
            if(sharedObject == null)
            {
                print("We found a null here!!!!!!!");
            }
            else
            {

			if (sharedObject.tag=="FloatingPlatform")
			{

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
				audioManager.Play ("ConfirmSharing");
			}
			else if (sharedObject.tag == "MovingPlatform")
			{
				Debug.Log("mv!");
				if (isServer && isLocalPlayer)
				{
					RpcShareMv(sharedObject.name);
				}
				if (!isServer && isLocalPlayer)
				{
					CmdShareMv(sharedObject.name);
				}
				audioManager.Play ("ConfirmSharing");
			}
			else if (sharedObject.tag == "Box")
			{
				Debug.Log("box found");
                string boxname = sharedObject.name;
				if(isServer && isLocalPlayer)
				{
					RpcBox(sharedObject.name);
					root.transform.Find("EricWorld").gameObject.transform.Find("WorldA").gameObject.transform.Find(boxname).gameObject.SetActive(false);
				}
				if(!isServer && isLocalPlayer)
				{
					CmdBox(sharedObject.name);
                    root.transform.Find("NatalieWorld").gameObject.transform.Find("WorldB").gameObject.transform.Find(boxname).gameObject.SetActive(false);
                }
				audioManager.Play ("ConfirmSharing");
			}
            }
		}

		// on the ground or on the ladder
		if (controller.collisions.above || controller.collisions.below || controller.collisions.onLadder)
		{
			if(playerJumping)
			{
				audioManager.Play("PlayerLand");
			}
			playerJumping = false;
			velocity.y = 0;
		}
		else
		{
			playerJumping = true;
		}

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // move audio
        if(controller.collisions.below && input.x != 0)
        {
            if(!playingWalkingMusic)
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
		if(controller.collisions.onLadder)
		{
			print("Can move down!");
			velocity.y = input.y * moveSpeed;
		}

		if(controller.collisions.canClimbLadder && input.y < 0)
		{
			print("Player want to go down!");
			controller.collisions.playerClimbLadder = true;
		}
		else
		{
			controller.collisions.playerClimbLadder = false;
		}

		// jump
		if (Input.GetKeyDown(KeyCode.Space) && (controller.collisions.below && !controller.collisions.onLadder))
		{
			audioManager.Play("PlayerJump");
			velocity.y = jumpVelocity;
			playerJumping = true;
		}

		velocity.x = input.x * moveSpeed;
		if (velocity.x < 0)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		}
		else if(velocity.x > 0)
		{
			GetComponent<SpriteRenderer>().flipX = false;
		}
		if(!controller.collisions.onLadder) velocity.y -= gravity * Time.deltaTime;


        // set the animator statemachine
        playerUp = velocity.y > 0;
		animator.SetBool("playerJumping", playerJumping);
		animator.SetBool("playerUp", playerUp);
		animator.SetBool("playerStand", 
			(velocity.x == 0 && !playerJumping) );
		animator.SetBool("playerClimb", controller.collisions.onLadder);
		if(controller.collisions.onLadder)
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
		transform.position = curCheckPoint;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
	}

    public void checkWho(int keyIdx)
    {
        if (isServer)
        {
            int keyIdxPlus = keyIdx + 1;
            GameObject go = root.transform.Find("ShareWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;

			if (go.GetComponent<KeyController> ().both [0] != 1) {
				audioManager.Play ("FragmentOne");
				go.transform.DOScale (15, 0.4f);
				go.transform.DOScale (12, 0.4f).SetDelay (0.8f);
			}
            go.GetComponent<KeyController>().both[0] = 1;

            RpcFrag(keyIdx);


            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
				go.GetComponent<KeyController> ().ShowEricMemory ();
                //gameObject.SetActive(false);
               // Debug.Log("both key");
            }


            go.GetComponent<KeyController>().setBoth();
        }
        else
        {
            int keyIdxPlus = keyIdx + 1;
            GameObject go = root.transform.Find("ShareWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;
            
			if (go.GetComponent<KeyController> ().both [1] != 1) {
				audioManager.Play ("FragmentTwo");
				go.transform.DOScale (15, 0.8f);
				go.transform.DOScale (12, 0.8f).SetDelay (0.8f);
			}

			go.GetComponent<KeyController>().both[1] = 1;
            CmdFrag(keyIdx);

            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
				go.GetComponent<KeyController> ().ShowNatalieMemory ();
                //gameObject.SetActive(false);
                Debug.Log("both key");
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
            GameObject go = root.transform.Find("ShareWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;
            
			if (go.GetComponent<KeyController> ().both [0] != 1) {
				audioManager.Play ("FragmentOne");
				go.transform.DOScale (15, 0.8f);
				go.transform.DOScale (12, 0.8f).SetDelay (0.8f);
			}

			go.GetComponent<KeyController>().both[0] = 1;

            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
				go.GetComponent<KeyController> ().ShowNatalieMemory ();
                //gameObject.SetActive(false);
                Debug.Log("both key");
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
            GameObject go = root.transform.Find("ShareWorld").gameObject.transform.Find("Fragment" + keyIdxPlus).gameObject;

			if (go.GetComponent<KeyController> ().both [1] != 1) {
				audioManager.Play ("FragmentTwo");
				go.transform.DOScale (15, 0.8f);
				go.transform.DOScale (12, 0.8f).SetDelay (0.8f);
			}

			go.GetComponent<KeyController>().both[1] = 1;


            if (go.GetComponent<KeyController>().both[0] == 1 && go.GetComponent<KeyController>().both[1] == 1)
            {
                Image ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
                ima.enabled = true;
                ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_collected");
				go.GetComponent<KeyController> ().ShowEricMemory ();
                //gameObject.SetActive(false);
                Debug.Log("both key");
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
		if (!isServer) {
			Debug.Log("wo chi"+keyIdx);
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
			Debug.Log("wo chii"+keyIdx+name);
			Player p = GetComponent<Player>();
			p.otherHaveKey[keyIdx] = true;
		}
	}



	//sent by server, show object on clients
	[ClientRpc]
	public void RpcShare(string sharedObject)
	{
		if (isServer) { return; }
		Debug.Log(sharedObject+" read");
        GameObject sObj = root.transform.Find("EricWorld/WorldA/" + sharedObject).gameObject;
		sObj.SetActive(true);
		GameObject newObj = Instantiate(sObj);
		newObj.transform.position = sObj.transform.position;
        newObj.tag = "FloatingPlatformShared";

	}

	//sent by client, show object on server
	[Command]
	public void CmdShare(string sharedObject)
	{
		Debug.Log(sharedObject+" read");
        GameObject sObj = root.transform.Find("NatalieWorld/WorldB/" + sharedObject).gameObject;
		sObj.SetActive(true);
		GameObject newObj = Instantiate(sObj);
		newObj.transform.position = sObj.transform.position;
        newObj.tag = "FloatingPlatformShared";
		Debug.Log(newObj.name);
	}


	//sent by server, show object on clients
	[ClientRpc]
	public void RpcShareMv(string sharedObject)
	{
		if (isServer) { return; }
		Debug.Log(sharedObject + " read");
		GameObject remoteWorld = root.transform.Find("EricWorld").gameObject.transform.Find("WorldA").gameObject;
		GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
		sObj.SetActive(true);
		//sObj.transform.localPosition += sObj.GetComponent<MovingPlatformController> ().targetTranslate;
		GameObject newObj = Instantiate(sObj);
		newObj.transform.position = sObj.transform.position;
		if (!sObj.GetComponent<MovingPlatformController> ().isMoved) {
			newObj.transform.localPosition += sObj.GetComponent<MovingPlatformController> ().targetTranslate;
		}
	}

	//sent by client, show object on server
	[Command]
	public void CmdShareMv(string sharedObject)
	{
		Debug.Log(sharedObject + " read");
		GameObject remoteWorld = root.transform.Find("NatalieWorld").gameObject.transform.Find("WorldB").gameObject;
		GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
		sObj.SetActive(true);
		//sObj.transform.localPosition += sObj.GetComponent<MovingPlatformController> ().targetTranslate;
		GameObject newObj = Instantiate(sObj);
		newObj.transform.position = sObj.transform.position;
		if (!sObj.GetComponent<MovingPlatformController> ().isMoved) {
			newObj.transform.localPosition += sObj.GetComponent<MovingPlatformController> ().targetTranslate;
		}
	}


	//sent by server, show box on clients
	[ClientRpc]
	public void RpcBox(string sharedObject)
	{
		if (isServer) { return; }
		Debug.Log(sharedObject + " read");
		GameObject remoteWorld = root.transform.Find("EricWorld").gameObject.transform.Find("WorldA").gameObject;
		GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
		sObj.SetActive(true);
		GameObject newObj = Instantiate(sObj);
		newObj.transform.position = sObj.transform.position;
		Debug.Log (sObj.name);

	}

	//sent by client, show box on server
	[Command]
	public void CmdBox(string sharedObject)
	{
		Debug.Log(sharedObject + " read");
		GameObject remoteWorld = root.transform.Find("NatalieWorld").gameObject.transform.Find("WorldB").gameObject;
		GameObject sObj = remoteWorld.transform.Find(sharedObject).gameObject;
		sObj.SetActive(true);
		GameObject newObj = Instantiate(sObj);
		newObj.transform.position = sObj.transform.position;
		Debug.Log (sObj.name);

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
		Player p = GameObject.Find("Player").GetComponent<Player>();
		p.backToCheckPoint();

	}

	//sent by client, make die on server
	[Command]
	public void CmdDie()
	{
		Debug.Log("server CMDdie");
		Player p = GameObject.Find("Player").GetComponent<Player>();
		p.backToCheckPoint();
	}
}
