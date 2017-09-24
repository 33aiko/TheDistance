using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player p = collision.GetComponent<Player>();
            p.curCheckPoint = transform.position;
            print("Arrived first check point");
            gameObject.SetActive(false);
        }
    }
}
