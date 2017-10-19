using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour {

    int cnt = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt++;
            if (cnt < 2) return;
            print("User enters the ladder");
            Player p = collision.gameObject.GetComponent<Player>();
            p.controller.collisions.onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt--;
            print("User leaves the ladder");
            Player p = collision.gameObject.GetComponent<Player>();
            p.controller.collisions.onLadder = false;
        }
    }
}
