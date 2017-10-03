using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOnTriggerController : MonoBehaviour {

    public string mpName;
    public bool oneTimeTrigger;

    int cnt = 0;

    MovingPlatformController mPC;

	void Start () {
        mPC = GameObject.Find(mpName).GetComponent<MovingPlatformController>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") cnt++;
        if((collision.gameObject.tag == "Player" && cnt==2) || collision.gameObject.tag == "Box")
        {
            mPC.canMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (oneTimeTrigger) return; // then it's a one time trigger, there's no stopping
        if (collision.gameObject.tag == "Player") cnt--;
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Box")
        {
            mPC.canMove = false;
        }
    }

}
