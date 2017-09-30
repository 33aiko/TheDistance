using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("User enters the ladder");
            Player p = collision.gameObject.GetComponent<Player>();
            p.controller.collisions.onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("User leaves the ladder");
            Player p = collision.gameObject.GetComponent<Player>();
            p.controller.collisions.onLadder = false;
        }
    }
}
