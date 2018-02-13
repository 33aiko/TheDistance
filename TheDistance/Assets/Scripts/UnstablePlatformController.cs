using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnstablePlatformController : MonoBehaviour {

    public float respawnTime = 6.0f;
    public float lifetime = 3.0f;
    public float fallDist = 30;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(GetComponent<SpriteRenderer>().DOFade(0, lifetime));
            seq.Insert(0, transform.DOMoveY(startPosition.y - fallDist, lifetime));
            Invoke("setToTrigger", lifetime);
            Invoke("Respawn", respawnTime);
        }
    }

    void Respawn()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(GetComponent<SpriteRenderer>().DOFade(1, lifetime / 2));
        seq.Insert(0, transform.DOMoveY(startPosition.y, lifetime / 2));

        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    void setToTrigger()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

    }
}
