using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOnTriggerController : MonoBehaviour {

    public string mpName;
    public bool oneTimeTrigger;

    int cnt = 0;

    bool haveBox = false;
    bool haveUser = false;

    MovingPlatformController mPC;

	void Start () {
        mPC = GameObject.Find(mpName).GetComponent<MovingPlatformController>();
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
        mPC.canMove = haveBox || haveUser;
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
        mPC.canMove = haveUser || haveBox;
    }

}
