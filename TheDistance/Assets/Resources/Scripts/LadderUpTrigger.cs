using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderUpTrigger : MonoBehaviour {

    int cnt = 0;
    Text instruct;

    private void Start()
    {
        instruct = GameObject.Find("NPCText").GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt++;
            if (cnt < 2) return;
            Player p = collision.gameObject.GetComponent<Player>();
            if(!p.controller.collisions.onLadder)
                instruct.text = "Press down to climb the ladder";
            p.controller.collisions.canClimbLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt--;
            instruct.text = "";
            Player p = collision.gameObject.GetComponent<Player>();
            p.controller.collisions.canClimbLadder = false;
        }
    }

}
