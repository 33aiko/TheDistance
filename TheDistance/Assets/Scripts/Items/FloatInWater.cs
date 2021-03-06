﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatInWater : MonoBehaviour {
	public Vector3 move;
	public float RO = 1.0f;
	public float g = 1.5f;
	public float Volume = 0;
	public float BottomArea = 0.1f;
	public float F_float = 0;
	public float m = 0.05f;
	public float velocity = 0;
	public float a = 0;

    public bool playerOnTop = false;
    public Player p;

	public bool isInWater = false; 

	public void SetInWater(){
        velocity = 0;
        print("This is in water!");
		isInWater = true;
        GetComponent<BoxController>().haveGravity = false;
        GetComponent<Controller2D>().collisionMask ^= LayerMask.NameToLayer("Water");
	}
	
	// Update is called once per frame
	void Update () {
		if (isInWater) {
			F_float = RO * g * Volume;
			a = (m * g - F_float) / m;

			float v2 = velocity + a * Time.deltaTime;
			move.y = -(v2 + velocity) / 2;
            if (playerOnTop)
            {
                movePlayer();
                print("moving player on the platform");
                playerOnTop = false;
            }
			transform.Translate (move);

			velocity = v2;
			Volume += velocity * Time.deltaTime * BottomArea;
		}

	}

    public void movePlayer()
    {
        p.controller.Move(new Vector3(0, move.y));
        p.controller.collisions.below = true;
    }
}
