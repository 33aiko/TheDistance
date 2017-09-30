using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour {

    public string NPCtalk;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.gameObject.tag == "Player")
        {
            print("Press E to talk to the NPC");
            Player p = collision.transform.gameObject.GetComponent<Player>();
            p.curNPC = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Player")
        {
            Player p = collision.transform.gameObject.GetComponent<Player>();
            p.curNPC = null;
            print(p.curNPC);
        }
    }
}
