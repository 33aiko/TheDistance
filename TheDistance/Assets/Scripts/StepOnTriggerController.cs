using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

public class StepOnTriggerController : NetworkBehaviour{

    public string mpName;
    public bool oneTimeTrigger;
    bool hasMoved = false; 
    public bool moveOnOtherWorld = false;

    int cnt = 0;

    bool haveBox = false;
    bool haveUser = false;

	AudioManager audio;  

    MovingPlatformController mPC;

    //particle effect
    public bool hasParticle = false;
    [SerializeField]
    ParticleSystem PS_small;
    [SerializeField]
    ParticleSystem PS_dots;
    [SerializeField]
    ParticleSystem PS_trail;
    [SerializeField]
    GameObject pattern;
    [SerializeField]
    SpriteRenderer triggerlight;

    Tween rotatePattern;

    Transform curCollider;

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
        {
            rotatePattern = pattern.transform.DORotate(Vector3.up * 360, 5.0f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
            StopAllParticle();
        }
		audio = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
    }

    void SetCanMove (bool _canMove)
    {
        if(moveOnOtherWorld)
        {
            // the code is actually in Player.cs
            // don't know why
            // just bug fixed
            Player pP = GameObject.Find("Player").GetComponent<Player>();
            if (pP.isServer)
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
        //StopParticle(PS_pattern, true);
        StartParticle(PS_small);
        StartParticle(PS_trail);
        triggerlight.transform.DOScaleY(1, 1);

        rotatePattern.Pause();
        pattern.transform.DOMove(transform.position + Vector3.up * 100, 1.0f);
        pattern.transform.DOScale(0.5f, 1.0f);
        pattern.transform.DORotate(Vector3.zero, 1.0f);
    }

    private void StopAllParticle(bool isOnetime = false)
    {
        StopParticle(PS_dots);
        //StartParticle(PS_pattern);
        StopParticle(PS_small);
        StopParticle(PS_trail);
        triggerlight.transform.DOScaleY(0, 1);
        if(!isOnetime)
        {
            rotatePattern.Play();
            pattern.transform.DOLocalMove(Vector3.up, 1.0f);
            pattern.transform.DOScale(1.0f, 1.0f);
        }
        else
        {
            pattern.GetComponent<SpriteRenderer>().DOFade(0, 1.0f);
        }
    }


    private void StartParticle(ParticleSystem ps)
    {
        ps.Play();
    }

    private void StopParticle(ParticleSystem ps, bool clear = true)
    {
        if (clear)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        else
            ps.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasMoved) return;
        if (collision.gameObject.tag == "Player") cnt++;
        if ((collision.gameObject.tag == "Player" && cnt == 2))
        {
            haveUser = true;
            curCollider = collision.transform;
            if(oneTimeTrigger)
                hasMoved = true;
        }
        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "BoxCannotShare")
        {
            haveBox = true;
            curCollider = collision.transform;
        }
        SetCanMove(haveBox || haveUser);
		if (audio != null) {
			audio.Play ("TriggerActivate");
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") cnt--;
        //if (oneTimeTrigger) return; // then it's a one time trigger, there's no stopping
        if (collision.gameObject.tag == "Player")
        {
            haveUser = false;
            curCollider = null;
            if (hasMoved)
            {
                StopAllParticle(true);
            }
        }
		if(collision.gameObject.tag == "Box" || collision.gameObject.tag == "BoxCannotShare")
        {
            haveBox = false;
            curCollider = null;
        }
        SetCanMove(haveBox || haveUser);
		if (audio != null) {
			audio.Stop ("TriggerActivate");
		}
    }

}
