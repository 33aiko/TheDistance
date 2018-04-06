using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoatAtFinish : MonoBehaviour {

    int cnt = 0;

    Player p;

    [SerializeField]
    bool curPlayerOnBoat = false;
    [SerializeField]
    bool remotePlayerOnBoat = false;

    public void SetRemotePlayerOnBoat(bool onBoat)
    {
        remotePlayerOnBoat = onBoat;
        TryPlayFinishAnimation();
    }

    void TryPlayFinishAnimation()
    {
        if(curPlayerOnBoat && remotePlayerOnBoat)
        {
            print("both player on boat!");
            transform.DOMoveX(transform.position.x + 500, 5.0f);
            p.canMove = false;
            p.transform.DOMoveX(p.transform.position.x + 500, 5.0f).OnComplete( 
                ()=> { p.canMove = true; });
            Camera.main.transform.DOMoveX(Camera.main.transform.position.x + 300, 5.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                print("player on boat!");
                p = collision.GetComponent<Player>();
                curPlayerOnBoat = true;
                p.SetOnBoatAtFinish(true);
                TryPlayFinishAnimation();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            cnt--;
            if(p == null)
                p = collision.GetComponent<Player>();
            p.SetOnBoatAtFinish(false);
            curPlayerOnBoat = false;
        }
    }

}
