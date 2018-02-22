using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingRock : MonoBehaviour {

    Vector3 startPosition;

    bool canFall = true;

    void Start()
    {
        startPosition = transform.position;
        InvokeRepeating("RockFall", 6.0f, 6.0f);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //print("Hit!");
        if(coll.gameObject.tag == "Player")
        {
            print("hits player");
            coll.gameObject.GetComponent<Player>().Die();
        }
        else
        {
            //print("hit ground!");
            RespawnRock();
        }
    }

    void RespawnRock()
    {
        transform.position = startPosition;
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0.0f;
        GetComponent<SpriteRenderer>().color = tmp;
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.GetComponent<SpriteRenderer>().DOFade(1, 3.0f));
        seq.Append(DOTween.To(() => transform.position, vv => transform.position += Vector3.right * Mathf.Sin(vv.y) * 0.5f, Vector3.up * Mathf.PI * 4, 0.5f));
        seq.PrependInterval(1.0f);
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void RockFall()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

}
