using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[RequireComponent(typeof(Controller2D))]
public class Player : NetworkBehaviour
{
	//predefined names
	public string EricWorldName = "EricWorld";
	public string NatalieWorldName = "NatalieWorld";
	public string EricPosName = "EricPos";
	public string NataliePosName = "NataliePos";
	public string ShareWorldName="ShareWorld";

	public GameObject spirit;
	public Vector3 spiritTargetPos;

	public float interpolateTime = 10;

    public float jumpHeight;
    public float timeToJumpApex;
    public Vector3 curCheckPoint;
    public float moveSpeed;

    public bool[] haveKey;
    public int keyNum;

    public NPCTrigger curNPC;

    public float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocitySmoothing;

    private Vector3 offset;

    [HideInInspector]
    public Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //curCheckPoint = new Vector3(5457f, 685f, 0);

        //have key
        for (int i = 0; i < 3; i++) haveKey[i] = false;

        // interact with objects
        controller.collisions.interact = false;
        
        //camera
        offset = Camera.main.transform.position - transform.position;

		//find root, because spirit is deactived, so we can only use transform to find it.
		GameObject root = GameObject.Find("Root");
		spirit = root.transform.Find("Spirit").gameObject;
		spiritTargetPos = spirit.transform.position;

		Transform EricTransform;//EricPos
		Transform NatalieTransform;//NataliePos
		EricTransform = root.transform.Find(ShareWorldName + "/" + EricPosName);
		NatalieTransform = root.transform.Find(ShareWorldName + "/" + NataliePosName);

		if (isLocalPlayer && isServer) {
			transform.position = EricTransform.position;
		}
		if (isLocalPlayer && !isServer) {
			transform.position = NatalieTransform.position;
		}

		//
		root.transform.Find (EricWorldName).gameObject.SetActive (isServer);
		root.transform.Find (NatalieWorldName).gameObject.SetActive (!isServer);

		//hideRemotePlayer
		if (!isLocalPlayer) {
			gameObject.SetActive (false);
		} else {
			gameObject.name = "Player";
		}

		//when client(Natalie) is connected and created, initialize server and itself
		if (!isServer && isLocalPlayer)
		{
			CmdInitializeServer(NatalieTransform.position,EricTransform.position);
			InitializeClient(EricTransform.position,NatalieTransform.position);
		}
    }

    void Update()
    {
		//input controlling move
		KeyControlMove();

        //camera
        Vector3 tmp = new Vector3(0, 0, offset.z);

		if (isLocalPlayer) {
			Camera.main.transform.position = transform.position + tmp;
		}


		//interpolate move by frame rate, when position not equal, move
		if (!spirit.transform.position.Equals(spiritTargetPos))
		{
			spirit.transform.Translate((spiritTargetPos - spirit.transform.position) / interpolateTime);
		}
    }
	void KeyControlMove(){
		// press Q to interact with the object
		if(Input.GetKey(KeyCode.Q))
			controller.collisions.interact = true;
		else
			controller.collisions.interact = false;

		if(Input.GetKeyDown(KeyCode.E))
		{
			print("have NPC in range? " + (curNPC != null));
			if(curNPC != null)
			{
				print("NPC says: " + curNPC.NPCtalk);
			}
		}

		// on the ground
		if (controller.collisions.above || controller.collisions.below || controller.collisions.onLadder)
		{
			velocity.y = 0;
		}

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		//print("On the ladder? " + controller.collisions.onLadder);

		// move in y axis if on the ladder
		if(controller.collisions.onLadder)
		{
			//print("User on the ladder!");
			//print(controller.collisions.onLadder);
			velocity.y = input.y * moveSpeed;
		}

		if (Input.GetKeyDown(KeyCode.Space) && (controller.collisions.below && !controller.collisions.onLadder))
		{
			velocity.y = jumpVelocity;
		}

		velocity.x = input.x * moveSpeed;
		if(!controller.collisions.onLadder) velocity.y -= gravity * Time.deltaTime;
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

    void backToCheckPoint()
    {
        transform.position = curCheckPoint;
    }

    public void Die()
    {
        Debug.Log("Player died!");
        backToCheckPoint();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
	[Command]
	public void CmdInitializeServer(Vector3 spirit_pos, Vector3 player_pos)
	{
		//print("CmdIniatiateServer");
		spirit.transform.position = spirit_pos;

		spirit.SetActive(true);
		GameObject.Find("Player").transform.position = player_pos;
		GameObject.Find("Player").GetComponent<Player>().spiritTargetPos = spirit_pos;
	}
	public void InitializeClient(Vector3 spirit_pos, Vector3 player_pos)
	{
		//print("IniatiateClient");
		spirit.transform.position = spirit_pos;
		spiritTargetPos = spirit_pos;
		spirit.SetActive(true);
		GameObject.Find("Player").transform.position = player_pos;
	}
}
