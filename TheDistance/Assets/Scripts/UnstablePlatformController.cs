using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnstablePlatformController : MonoBehaviour {

    public float respawnTime = 6.0f;
    public float lifetime = 3.0f;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().DOFade(0, lifetime);
            Invoke("setToTrigger", lifetime);
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
