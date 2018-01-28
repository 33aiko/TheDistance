using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StepOnTriggerController : NetworkBehaviour{

    public string mpName;
    public bool oneTimeTrigger;
    public bool moveOnOtherWorld = false;

    int cnt = 0;

    bool haveBox = false;
    bool haveUser = false;

    MovingPlatformController mPC;

	void Start () {
        mPC = GameObject.Find(mpName).GetComponent<MovingPlatformController>();
	}

    void SetCanMove (bool _canMove)
    {
        if(moveOnOtherWorld)
        {
            // the code is actually in Player.cs
            // don't know why
            // just bug fixed
            Player pP = GameObject.Find("Player").GetComponent<Player>();
            if (isServer)
            {
                pP.RpcSetPlatformMoveable(_canMove, mpName);
            }
            else
            {
                print("sending moving platform!");
                pP.CmdSetPlatformMoveable(_canMove, mpName);
            }
        }
        else
        {
            mPC.canMove = _canMove;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") cnt++;
        if ((collision.gameObject.tag == "Player" && cnt == 2))
        {
            haveUser = true;
        }
		if(collision.gameObject.tag == "Box" || collision.gameObject.tag == "BoxCannotShare")
        {
            haveBox = true;
        }
        SetCanMove(haveBox || haveUser);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") cnt--;
        if (oneTimeTrigger) return; // then it's a one time trigger, there's no stopping
        if (collision.gameObject.tag == "Player")
        {
            haveUser = false;
        }
		if(collision.gameObject.tag == "Box" || collision.gameObject.tag == "BoxCannotShare")
        {
            haveBox = false;
        }
        SetCanMove(haveBox || haveUser);
    }

}
