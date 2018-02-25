using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntranceController : MonoBehaviour {

    public bool isRight = true;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            print("player enters!");
            Player p = coll.gameObject.GetComponent<Player>();
            //p.EnterCave();
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Player p = coll.gameObject.GetComponent<Player>();
            print(p.transform.position.x);
            print(transform.position.x);
            if(isRight)
            {
                if (p.transform.position.x < transform.position.x) ;
                    //p.LeaveCave();
            }
            else
            {
                if (p.transform.position.x > transform.position.x);
                    //p.LeaveCave();
            }
        }
    }
}
