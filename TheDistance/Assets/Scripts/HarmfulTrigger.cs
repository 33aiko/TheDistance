using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulTrigger : MonoBehaviour {

    int cnt = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt++;
            print(gameObject.name + " hits the player!");
            if(cnt == 2)
            {
                Player p = collision.GetComponent<Player>();
                p.Die();
                // when player falls, the player will die
                // future lethal triggers can use this function
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            cnt--;
    }

}
