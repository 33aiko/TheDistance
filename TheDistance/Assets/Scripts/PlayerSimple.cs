using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Controller2D))]
public class PlayerSimple : MonoBehaviour{

    public float moveSpeed;
    public Vector3 velocity;
    public float jumpHeight;
    public float timeToJumpApex;
    public float gravity;
    float jumpVelocity;
    float velocitySmoothing;
    public bool playerJumping;
    bool canControlMove = true;
    bool canMove = true;
    bool keyspaceDown = false;
    public Vector2 input;

    [HideInInspector]
    public Controller2D controller;

    [HideInInspector]
    AudioManager audioManager;

    bool playingWalkingMusic = false;
    [HideInInspector]
    public Animator animator;

    float jumpTime;
    float nextIdleTime;

    public Vector3 targetPos;
    public float interpolateTime = 20;
    public bool controllerEnteredLobby = false;


    // Use this for initialization
    void Start () {
        //audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        gameObject.SetActive(true);

        gravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        targetPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //input controlling move
        if(canControlMove)
        {
            KeyControlMove();
        }
    }

    public void Change2ReadyState()
    {
        canMove = false;
    }

    public void ControlledByOthers()
    {
        canControlMove = false;
    }

    void KeyControlMove()
    {

        if (controller.collisions.above || controller.collisions.below)
        {
            playerJumping = false;
            velocity.y = 0;
        }


        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // print(input);


        if(canControlMove)
        {
            movePlayer(input);
        }
    }

    public void movePlayer(Vector2 _input, bool fromOther = false, bool jumpingInfo = false)
    {
        if (fromOther)
        {
            playerJumping = jumpingInfo;
        }

        keyspaceDown = false;
        if (canMove && 
            (
                (Input.GetButtonDown("Jump") && canControlMove) || 
                (jumpingInfo && !canControlMove)
            ) &&
            controller.collisions.below && !controller.collisions.onLadder) 
        {
            //audioManager.Play("PlayerJump");
            velocity.y = jumpVelocity;
            playerJumping = true;
            keyspaceDown = true;
        }

        // move audio
        if (controller.collisions.below && _input.x != 0)
        {
            if (!playingWalkingMusic)
            {
                //audioManager.Play("PlayerWalking");
                playingWalkingMusic = true;
            }
        }
        else
        {
            //audioManager.Stop("PlayerWalking");
            playingWalkingMusic = false;
        }

        velocity.x = _input.x * moveSpeed;

        if(fromOther)
        {
            print("moving other player at velocity:" + velocity.x);
            print("actual moveming:" + velocity.x * Time.deltaTime);
        }

        if (!canMove) velocity.x = 0;
        if (velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        velocity.y -= gravity * Time.deltaTime;

        /*
        //interpolate move by frame rate, when position not equal, move
        bool isForcedX = false;
        bool isForcedUp = (!canControlMove && controllerEnteredLobby && this.transform.position.y != targetPos.y);
        if (!canControlMove && controllerEnteredLobby && !this.transform.position.Equals(targetPos))
        {
            isForcedX = true;
            this.transform.Translate((targetPos - this.transform.position) / interpolateTime);
        }
        */


        bool playerUp = (velocity.y > 0);
        bool playerStand =
            (velocity.x == 0 && (!playerJumping && !keyspaceDown)
            && controller.collisions.below);
        animator.SetBool("playerJumping", (playerJumping) || keyspaceDown);
        animator.SetBool("playerWalking", (velocity.x != 0));
        animator.SetBool("playerUp", playerUp);
        animator.SetBool("playerStand", playerStand);
        animator.SetBool("hasInput", keyspaceDown || _input.x != 0 );

        if (playerStand)
        {
            nextIdleTime -= Time.deltaTime;
            if (nextIdleTime < 0)
            {
                animator.SetBool("playerIdle", true);
                nextIdleTime = Random.Range(8.0f, 15.0f);
            }
        }
        else
        {
            nextIdleTime = Random.Range(2.0f, 3.0f);
        }

        controller.Move(velocity * Time.deltaTime);
    }

}
