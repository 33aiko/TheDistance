using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    public float jumpHeight;
    public float timeToJumpApex;
    public Vector3 curCheckPoint;
    public float moveSpeed;

    public bool[] haveKey;
    public int keyNum;

    public NPCTrigger curNPC;

    public float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocitySmoothing;

    private Vector3 offset;

    [HideInInspector]
    public Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //curCheckPoint = new Vector3(5457f, 685f, 0);

        //have key
        for (int i = 0; i < 3; i++) haveKey[i] = false;

        // interact with objects
        controller.collisions.interact = false;
        
        //camera
        offset = Camera.main.transform.position - transform.position;
    }

    void Update()
    {
        // press Q to interact with the object
        if(Input.GetKey(KeyCode.Q))
           controller.collisions.interact = true;
        else
           controller.collisions.interact = false;

        if(Input.GetKeyDown(KeyCode.E))
        {
            print("have NPC in range? " + (curNPC != null));
            if(curNPC != null)
            {
                print("NPC says: " + curNPC.NPCtalk);
            }
        }

        // on the ground
        if (controller.collisions.above || controller.collisions.below || controller.collisions.onLadder)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //print("On the ladder? " + controller.collisions.onLadder);

        // move in y axis if on the ladder
        if(controller.collisions.onLadder)
        {
            //print("User on the ladder!");
            //print(controller.collisions.onLadder);
            velocity.y = input.y * moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (controller.collisions.below && !controller.collisions.onLadder))
        {
            velocity.y = jumpVelocity;
        }

        velocity.x = input.x * moveSpeed;
        if(!controller.collisions.onLadder) velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //camera
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
