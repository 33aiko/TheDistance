using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTrigger : MonoBehaviour {

    public string NPCtalk;
    Text t;
    Text instruct;

    int cnt = 0;

    private void Start()
    {
        instruct = GameObject.Find("NPCText").GetComponent<Text>();
        t = GetComponentInChildren<Text>();
        t.text = "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                instruct.text = "Press E to talk to the NPC";
                Player p = collision.transform.gameObject.GetComponent<Player>();
                p.curNPC = this;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Player")
        {
            cnt--;
            if(cnt == 0)
            {
                Player p = collision.transform.gameObject.GetComponent<Player>();
                p.curNPC = null;
                t.text = "";
                instruct.text = "";
            }
        }
    }

    public void showTalkText()
    {
        if(t == null)
        {
            print("nothing found");
            return;
        }
        t.text = NPCtalk;
    }
}
