using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderUpTrigger : MonoBehaviour {

    int cnt = 0;


    private void Start()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt++;
            if (cnt < 2) return;
            Player p = collision.gameObject.GetComponent<Player>();
            if(!p.controller.collisions.onLadder)
            p.controller.collisions.canClimbLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt--;
            Player p = collision.gameObject.GetComponent<Player>();
            p.controller.collisions.canClimbLadder = false;
        }
    }

}
