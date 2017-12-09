using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening; 

public class NPCTrigger : MonoBehaviour {

	public Image blackmask; 
	public GameObject NPCcontent; 
    public string NPCtalk;

	Image inputUI;
    Text t;
    Text instruct;

    string UIPath = "Sprites/UI/controls/input hint UI";
    string ps4UIName = "inputUI_tri";
    string keyboardUIName = "inputUI_keyE";

    int cnt = 0;

    bool currentIsKeyboard = true;

    private void Start()
    {
		inputUI = GetComponentInChildren<Image> ();
        instruct = GameObject.Find("Instruction").GetComponent<Text>();
        loadSprite(UIPath, keyboardUIName);
		inputUI.gameObject.SetActive (false);
        t = GetComponentInChildren<Text>();
        t.text = "";
    }

    private void loadSprite(string path, string UIname)
    {
        Sprite[] sprites;
        sprites = Resources.LoadAll<Sprite>(path);
        inputUI.sprite = sprites.Where(tmp => tmp.name == UIname).First();
    }

    public void setImage(bool isKeyboard)
    {
        if (isKeyboard == currentIsKeyboard) return;
        if (isKeyboard)
        {
            loadSprite(UIPath, keyboardUIName);
        }
        else
        {
            loadSprite(UIPath, ps4UIName);
        }

        currentIsKeyboard = isKeyboard;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                instruct.text = "";
				//t.text = "Press E to view" ;
				inputUI.gameObject.SetActive(true);
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
            if(cnt != 2)
            {
                Player p = collision.transform.gameObject.GetComponent<Player>();
                p.curNPC = null;
               // t.text = "";
				inputUI.gameObject.SetActive(false);
                instruct.text = "";
                print("Player leaving NPC");
                blackmask.DOFade(0, 0);
                NPCcontent.SetActive(false);
            }
        }
    }


    public void showTalkText()
    {
		blackmask.DOFade (0.8f, 0);
		NPCcontent.SetActive (true);
//        if(t == null)
//        {
//            print("nothing found");
//            return;
//        }
       // t.text = "";
		inputUI.gameObject.SetActive(false);
    }

	public void hideTalkText()
	{
		//t.text = "press E to view";
		inputUI.gameObject.SetActive(true);
		blackmask.DOFade (0, 0);
		NPCcontent.SetActive (false);
	}
}
