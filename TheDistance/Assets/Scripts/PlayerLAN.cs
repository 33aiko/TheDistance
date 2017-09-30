using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLAN : NetworkBehaviour
{
    public float speed = 4.0f;
    public GameObject spirit;
    public Vector3 spiritTargetPos;
    public float interpolateTime = 10;

    //predefined names
    public string EricWorldName = "EricWorld";
    public string NatalieWorldName = "NatalieWorld";
    public string EricPosName = "EricPos";
    public string NataliePosName = "NataliePos";

    void Start () {
        GameObject root = GameObject.Find("Root");
        Transform EricTransform;//EricPos
        Transform NatalieTransform;//NataliePos
        if (isServer)
        {
            EricTransform = root.transform.Find(EricWorldName+"/"+EricPosName);
            NatalieTransform = root.transform.Find(EricWorldName + "/" + NataliePosName);
        }
        else
        {
            EricTransform = root.transform.Find(NatalieWorldName + "/" + EricPosName);
            NatalieTransform = root.transform.Find(NatalieWorldName + "/" + NataliePosName);
        }

        //find root, because spirit is deactived, so we can only use transform to find it.
        spirit = root.transform.Find("Spirit").gameObject;
        spiritTargetPos = spirit.transform.position;

        //set up player game object name and remote player's name.
        if (isLocalPlayer)
        {
            gameObject.name = "Player";
        }
        else
        {
            gameObject.name = "RemotePlayer";
        }

        //when client(Natalie) is connected and created, initialize server and itself
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

        //interpolate move by frame rate, when position not equal, move
        if (!spirit.transform.position.Equals(spiritTargetPos))
        {
            spirit.transform.Translate((spiritTargetPos - spirit.transform.position) / interpolateTime);
        }
    }
    

    //sent by server, run on all clients
    [ClientRpc]
    public void RpcMove(Vector3 pos)
    {
        //print("Rpc Move");
        if (!isServer)
        {
            GameObject.Find("Player").GetComponent<PlayerLAN>().spiritTargetPos = pos;
        }
    }

    //sent by client, run on server
    [Command]
    public void CmdMove(Vector3 pos)
    {
        //print("Cmd Move");
        GameObject.Find("Player").GetComponent<PlayerLAN>().spiritTargetPos = pos;
    }
    [Command]
    public void CmdInitializeServer(Vector3 spirit_pos, Vector3 player_pos)
    {
        //print("CmdIniatiateServer");
        spirit.transform.position = spirit_pos;

        spirit.SetActive(true);
        GameObject.Find("RemotePlayer").SetActive(false);
        GameObject.Find("Player").transform.position = player_pos;
        GameObject.Find("Player").GetComponent<PlayerLAN>().spiritTargetPos = spirit_pos;
    }
    public void InitializeClient(Vector3 spirit_pos, Vector3 player_pos)
    {
        //print("IniatiateClient");
        spirit.transform.position = spirit_pos;
        spiritTargetPos = spirit_pos;
        spirit.SetActive(true);
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
    }
}
