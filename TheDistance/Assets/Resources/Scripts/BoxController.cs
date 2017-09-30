using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class BoxController : MonoBehaviour {

    float gravity;
    Vector3 velocity;

    public Controller2D controller;

	void Start () {
        controller = GetComponent<Controller2D>();
        gravity = 300.0f / 0.3f / 0.3f;
        velocity.y = 0;
	}
	
	void Update () {
        if (controller.collisions.above || controller.collisions.below)
        {
            //print("box on the ground");
            velocity.y = 0;
        }
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
