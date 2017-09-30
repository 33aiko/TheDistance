using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {

    public int keyIdx;

    int cnt = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                Player p = collision.GetComponent<Player>();
                p.haveKey[keyIdx] = true;
                print("The player got the key!");
                gameObject.SetActive(false);
            }
        }
    }
}
