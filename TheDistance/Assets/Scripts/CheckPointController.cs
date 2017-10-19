using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour {

    int cnt = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                Player p = collision.GetComponent<Player>();
                p.curCheckPoint = transform.position;
                print("Arrived first check point");
                gameObject.SetActive(false);
				GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("Checkpoint");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt--;
        }
    }
}
