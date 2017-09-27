using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLAN : NetworkBehaviour
{
    public float speed = 4.0f;
    public GameObject sprite;
    public Vector3 spriteTargetPos;
    public float interpolateTime = 10;
    
	void Start () {
        GameObject root = GameObject.Find("Root");
        Transform EricTransform = root.transform.Find("EricPos");
        Transform NatalieTransform = root.transform.Find("NataliePos");

        //find root and set sprite to be visible.
        sprite = root.transform.Find("Sprite").gameObject;
        spriteTargetPos = sprite.transform.position;

        //set up player game object name and remote player's name.
        if (isLocalPlayer)
        {
            gameObject.name = "Player";
        }
        else
        {
            gameObject.name = "RemotePlayer";
        }
        //when client(Natalie) is connected and created, initial server and itself
        if (!isServer && isLocalPlayer)
        {
            CmdInitializeServer(NatalieTransform.position,EricTransform.position);
            InitializeClient(EricTransform.position,NatalieTransform.position);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //print("IsLocalPlayer:" + isLocalPlayer);

        //input controlling move
        KeyControlMove();

        //interpolate move by frame rate
        if (!sprite.transform.position.Equals(spriteTargetPos))
        {
            sprite.transform.Translate((spriteTargetPos - sprite.transform.position) / interpolateTime);
        }
    }
    

    //sent by server, run on all clients
    [ClientRpc]
    public void RpcMove(Vector3 pos)
    {
        //print("Rpc Move");
        if (!isServer)
        {
            GameObject.Find("Player").GetComponent<PlayerLAN>().spriteTargetPos = pos;
            //GameObject.Find("Sprite").transform.position = pos;
        }
    }

    //sent by client, run on server
    [Command]
    public void CmdMove(Vector3 pos)
    {
        //print("Cmd Move");
        GameObject.Find("Player").GetComponent<PlayerLAN>().spriteTargetPos = pos;
        //GameObject.Find("Sprite").transform.position = pos;
    }
    [Command]
    public void CmdInitializeServer(Vector3 sprite_pos, Vector3 player_pos)
    {
        //print("CmdIniatiateServer");
        //print("CmdIniatiateServer:sprite pos:"+sprite_pos+" player pos:"+player_pos);
        sprite.transform.position = sprite_pos;

        sprite.SetActive(true);
        GameObject.Find("RemotePlayer").SetActive(false);
        GameObject.Find("Player").transform.position = player_pos;
        GameObject.Find("Player").GetComponent<PlayerLAN>().spriteTargetPos = sprite_pos;
    }
    public void InitializeClient(Vector3 sprite_pos, Vector3 player_pos)
    {
        //print("IniatiateClient");
        sprite.transform.position = sprite_pos;
        spriteTargetPos = sprite_pos;
        sprite.SetActive(true);
        GameObject.Find("RemotePlayer").SetActive(false);
        GameObject.Find("Player").transform.position = player_pos;
    }

    void KeyControlMove()
    {
        float move = speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-move, 0, 0);
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
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(move, 0, 0);
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
        if (Input.GetKey(KeyCode.S))
        {

        }
        if (Input.GetKey(KeyCode.W))
        {

        }
    }
}
