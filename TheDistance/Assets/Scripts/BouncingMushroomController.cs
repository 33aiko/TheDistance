using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncingMushroomController : MonoBehaviour {

    public float jumpFactor = 1.2f;
    public float scaleFactor = 0.4f;

    BoxCollider2D bc;

	void Start () {
        bc = GetComponent<BoxCollider2D>();
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            print("Player enters!");
            float bound_this_y = (bc.bounds.size.y / 2 + transform.position.y);
            float bound_coll_y = (-coll.collider.bounds.size.y / 2 + coll.transform.position.y);
            print("this: " + bound_this_y + " collision: " + bound_coll_y);
            if(Mathf.Abs(bound_coll_y - bound_this_y) < 1.0f)
            {
                // can jump
                Player p = coll.gameObject.GetComponent<Player>();

                p.PlayerJump(true, jumpFactor);

                // bouncing effect
                StartBouncingEffect();
            }
        }
    }

    void StartBouncingEffect()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(scaleFactor * transform.localScale.y, 0.2f).SetLoops(2, LoopType.Yoyo));
        seq.Insert(0, transform.DOMoveY(transform.position.y - transform.localScale.y * (1.3f- scaleFactor) / 2, 0.2f).SetLoops(2, LoopType.Yoyo));
    }

}
