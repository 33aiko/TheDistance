using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

public class StepOnTriggerController : NetworkBehaviour{

    public string mpName;
    public bool oneTimeTrigger;
    public bool moveOnOtherWorld = false;

    int cnt = 0;

    bool haveBox = false;
    bool haveUser = false;

    MovingPlatformController mPC;

    //particle effect
    public bool hasParticle = false;
    [SerializeField]
    ParticleSystem PS_small;
    [SerializeField]
    ParticleSystem PS_dots;
    [SerializeField]
    ParticleSystem PS_pattern;
    [SerializeField]
    ParticleSystem PS_trail;
    [SerializeField]
    GameObject pattern;
    [SerializeField]
    SpriteRenderer triggerlight;


	void Start () {
        try
        {
            mPC = GameObject.Find(mpName).GetComponent<MovingPlatformController>();
        }
        catch(System.NullReferenceException e)
        {
            Debug.LogWarning("Cannot find moving platform");
        }
        if (hasParticle)
            StopAllParticle();
    }

    void SetCanMove (bool _canMove)
    {
        if(moveOnOtherWorld)
        {
            // the code is actually in Player.cs
            // don't know why
            // just bug fixed
            Player pP = GameObject.Find("Player").GetComponent<Player>();
            if (isServer)
            {
                pP.RpcSetPlatformMoveable(_canMove, mpName);
            }
            else
            {
                print("sending moving platform!");
                pP.CmdSetPlatformMoveable(_canMove, mpName);
            }
        }
        else
        {
            mPC.canMove = _canMove;
        }
        if(_canMove)
        {
            StartAllParticle();
        }
        else
        {
            StopAllParticle();
        }
    }

    private void StartAllParticle()
    {
        StartParticle(PS_dots);
        StartParticle(PS_pattern);
        StartParticle(PS_small);
        StartParticle(PS_trail);
        triggerlight.transform.DOScaleX(1, 1);
    }

    private void StopAllParticle()
    {
        StopParticle(PS_dots);
        StopParticle(PS_pattern);
        StopParticle(PS_small);
        StopParticle(PS_trail);
        triggerlight.transform.DOScaleX(0, 1);
    }


    private void StartParticle(ParticleSystem ps)
    {
        if (!ps.isPlaying)
            ps.Play();
    }

    private void StopParticle(ParticleSystem ps)
    {
        if (ps.isPlaying)
            ps.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") cnt++;
        if ((collision.gameObject.tag == "Player" && cnt == 2))
        {
            haveUser = true;
        }
		if(collision.gameObject.tag == "Box" || collision.gameObject.tag == "BoxCannotShare")
        {
            haveBox = true;
        }
        SetCanMove(haveBox || haveUser);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") cnt--;
        if (oneTimeTrigger) return; // then it's a one time trigger, there's no stopping
        if (collision.gameObject.tag == "Player")
        {
            haveUser = false;
        }
		if(collision.gameObject.tag == "Box" || collision.gameObject.tag == "BoxCannotShare")
        {
            haveBox = false;
        }
        SetCanMove(haveBox || haveUser);
    }

}
