using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinishController : MonoBehaviour {

	Text instruct;
    int cnt = 0;

	public void Start(){
		instruct = GameObject.Find("Instruction").GetComponent<Text>();
        int i = 0;
        foreach (KeyController p in pKC)
        {
            print(p.name);
            i++;
        }
        print("Number of PKC is " + i);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt++;
            if (cnt != 2) return;
            Player p = collision.GetComponent<Player>();
            bool canFinish = true;
            KeyController[] pKC = FindObjectsOfType<KeyController>();
            foreach(KeyController k in pKC)
            {
                if(k.both[0] > 0 && k.both[1] > 0)
                {
                    print("player have got key " + k.name);
                }
                else
                {
                    canFinish = false;
                }
                /*
                canFinish &= p.haveKey[i];
                if(!p.haveKey[i]) print("Player missing key " + i);
                 */
            }
            if(canFinish)
            {
//                Button bu = FindObjectOfType<Button>();
//                bu.GetComponentInChildren<Text>().text = "Try again!";
				instruct.text = "Level completed!" ;
                print("Finished this level!");
                // level finished
            }
            else
            {
				instruct.text = "You need to collect all memory fragments.";
                print("You need to collect the key first!");
                // the player have to collect the key first
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt--;
        }
    }
}
