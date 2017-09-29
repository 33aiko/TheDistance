using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulTrigger : MonoBehaviour {

    private void Start()
    {
        //Debug.Log("Something here");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("somethign entered");
        if(collision.gameObject.tag == "Player")
        {
            Player p = collision.GetComponent<Player>();
            p.Die();
        }
    }

}
