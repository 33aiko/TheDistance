using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelFinishController : MonoBehaviour {

	Text instruct;
    int cnt = 0;
    GameObject root;

    int finishCount;

    public void Start(){
        finishCount = 0;
        root = GameObject.Find("Root");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt++;
            if (cnt != 2) return;
            Player p = collision.GetComponent<Player>();
            if (p.getCheck(1) == 1) { return; }
            p.setCheck(1);//1 i level
            finishCount++;
            if (finishCount < 2) { return; }
			if (collision.gameObject.GetComponent<Player> ().isServer) {
				for (int i = 1; i <= 3; i++) {
					GameObject go = root.transform.Find ("EricWorld").gameObject.transform.Find ("Fragment" + i).gameObject;
					if (go.GetComponent<KeyController> ().both [0] + go.GetComponent<KeyController> ().both [1] != 2) {
						return;
					}
				}
			} else {
				for (int i = 1; i <= 3; i++) {
					GameObject go = root.transform.Find ("NatalieWorld").gameObject.transform.Find ("Fragment" + i).gameObject;
					if (go.GetComponent<KeyController> ().both [0] + go.GetComponent<KeyController> ().both [1] != 2) {
						return;
					}
				}
			}
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
			//	instruct.text = "Level completed!" ;
                print("Finished this level!");
                SceneManager.LoadScene("Loading");
                // level finished
            }
            else
            {
			//	instruct.text = "You need to collect all memory fragments.";
                print("You need to collect the key first!");
                // the player have to collect the key first
            }
        }

		if (collision.gameObject.tag == "Boat" && SceneManager.GetActiveScene().name=="Boat") {
			SceneManager.LoadScene("Loading");
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt--;
            Player p = collision.GetComponent<Player>();
            p.clearCheck(1);
        }
    }
}
