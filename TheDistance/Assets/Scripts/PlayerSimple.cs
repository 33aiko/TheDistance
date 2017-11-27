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
    bool playerJumping;

    [HideInInspector]
    public Controller2D controller;

    [HideInInspector]
    AudioManager audioManager;

    bool playingWalkingMusic = false;
    [HideInInspector]
    Animator animator;

    float jumpTime;
    float nextIdleTime;


    // Use this for initialization
    void Start () {
        //audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        gameObject.SetActive(true);

        gravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }
	
	// Update is called once per frame
	void Update () {
        //input controlling move
        KeyControlMove();
    }

    void KeyControlMove()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            playerJumping = false;
            velocity.y = 0;
        }


        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        print(input);
        // move audio
        if (controller.collisions.below && input.x != 0)
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
        bool keyspaceDown = false;
        if (Input.GetKeyDown(KeyCode.Space) && (controller.collisions.below && !controller.collisions.onLadder))
        {
            //audioManager.Play("PlayerJump");
            velocity.y = jumpVelocity;
            playerJumping = true;
            keyspaceDown = true;
        }
        velocity.x = input.x * moveSpeed;

        if (velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        velocity.y -= gravity * Time.deltaTime;


        bool playerUp = velocity.y > 0;
        bool playerStand =
            (velocity.x == 0 && (!playerJumping && !keyspaceDown)
            && controller.collisions.below);
        animator.SetBool("playerJumping", (playerJumping) || keyspaceDown);
        animator.SetBool("playerWalking", (velocity.x != 0));
        animator.SetBool("playerUp", playerUp);
        animator.SetBool("playerStand", playerStand);
        animator.SetBool("hasInput", keyspaceDown || input.x != 0 );

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
