using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinishController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player p = collision.GetComponent<Player>();
            bool canFinish = true;
            for(int i = 0; i < p.keyNum; i++)
            {
                canFinish &= p.haveKey[i];
                if(!p.haveKey[i]) print("Player missing key " + i);
            }
            if(canFinish)
            {
                Button bu = FindObjectOfType<Button>();
                bu.GetComponentInChildren<Text>().text = "Try again!";
                print("Finished this level!");
                // level finished
            }
            else
            {
                print("You need to collect the key first!");
                // the player have to collect the key first
            }
        }
    }
}
