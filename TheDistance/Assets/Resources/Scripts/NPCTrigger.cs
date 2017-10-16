using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class NPCTrigger : MonoBehaviour {

	public Image blackmask; 
	public GameObject NPCcontent; 
    public string NPCtalk;
    Text t;
    Text instruct;

    int cnt = 0;

    private void Start()
    {
        instruct = GameObject.Find("Instruction").GetComponent<Text>();
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
               // instruct.text = "Press E to talk to the NPC";
				t.text = "Press E to view" ;
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
		blackmask.DOFade (0.8f, 0);
		NPCcontent.SetActive (true);
        if(t == null)
        {
            print("nothing found");
            return;
        }
        t.text = "";
    }

	public void hideTalkText()
	{
		t.text = "press E to view";
		blackmask.DOFade (0, 0);
		NPCcontent.SetActive (false);
	}
}
