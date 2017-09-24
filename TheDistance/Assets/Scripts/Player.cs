using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[RequireComponent (typeof (Controller2D) )]
public class Player : NetworkBehaviour {

    public float jumpHeight;
    public float timeToJumpApex;
    public bool haveKey1 = false;
    public Vector3 curCheckPoint;
    public float moveSpeed;
    public Vector3 offset;
    public Sprite clientSprite;

    float gravity;
    float jumpVelocity;
    float accelOnAir = .1f;
    float accelOnGround= .2f;
    Vector3 velocity;
    float velocitySmoothing;

    Controller2D controller;

	void Start () {
        controller = GetComponent<Controller2D>();
        gravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        curCheckPoint = new Vector3(12.82f, 58f, 0);
        transform.position = curCheckPoint;

        //camera
        offset = Camera.main.transform.position - transform.position;

        //SpriteImage
        if ((!isServer&&isLocalPlayer) || (isServer&&!isLocalPlayer)) { GetComponent<SpriteRenderer>().sprite = clientSprite; }
	}

	void Update ()
    {
        //LocalPlayerDetection
        if (!isLocalPlayer) { return; }

        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        velocity.x = input.x * moveSpeed;
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Camera
        Vector3 tmp = new Vector3(0, 0, offset.z);
        Camera.main.transform.position = transform.position + tmp;
    }

    void backToCheckPoint()
    {
        transform.position = curCheckPoint;
    }

    public void Die()
    {
        Debug.Log("Player died!");
        backToCheckPoint();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
