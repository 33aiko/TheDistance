using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnstablePlatformController : MonoBehaviour {

    public float respawnTime = 5.0f;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().DOFade(0, 2);
            Invoke("setToTrigger", 2.0f);
            Invoke("Respawn", respawnTime);
        }
    }

    void Respawn()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    void setToTrigger()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

    }
}
