using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BoatAtFinish : MonoBehaviour {

    int cnt = 0;

    Player p;

    [SerializeField]
    bool curPlayerOnBoat = false;
    [SerializeField]
    bool remotePlayerOnBoat = false;

    private void Start()
    {
      
    }
    public void SetRemotePlayerOnBoat(bool onBoat)
    {
        remotePlayerOnBoat = onBoat;

		TryPlayFinishAnimation ();

    }

    public float moveTime = 10.0f;

    void TryPlayFinishAnimation()
    {
        if(curPlayerOnBoat && remotePlayerOnBoat)
        {
            print("both player on boat!");
			transform.DOMoveX(transform.position.x + 350, moveTime).SetEase(Ease.Linear).SetDelay(2f);
            /*
            float vic = 0;
            float startY = transform.position.y;
            DOTween.To(() => vic, x =>
                    {
                        vic = x;
                Vector3 tmp = transform.position;
                tmp.y = startY + Mathf.Sin(vic) * 1;
                transform.position = tmp;
            }, 3.14f * 2, 1.0f).SetLoops(-1).SetEase(Ease.Linear);
             */ 
            p.canMove = false;
            p.animator.SetBool("playerWalking", false);
            p.animator.SetBool("playerStand", true);
            p.audioManager.Stop("PlayerWalking");
			p.transform.DOMoveX(p.transform.position.x + 350, moveTime).SetDelay(2f).OnComplete( 
                ()=> { p.canMove = false; 
						}).SetEase(Ease.Linear);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision is BoxCollider2D)
            {
                print("player on boat!");
                p = collision.GetComponent<Player>();
                curPlayerOnBoat = true;
                p.SetOnBoatAtFinish(true);
                Invoke("TryPlayFinishAnimation", 0.1f);
                //TryPlayFinishAnimation();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision is BoxCollider2D)
            {
                if (p == null)
                    p = collision.GetComponent<Player>();
                p.SetOnBoatAtFinish(false);
                curPlayerOnBoat = false;
            }
        }
    }

}
