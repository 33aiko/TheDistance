using System.Collections;
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

	bool isInWater = false; 

	public void SetInWater(){
        print("This is in water!");
		isInWater = true; 
	}
	
	// Update is called once per frame
	void Update () {
		if (isInWater) {
			F_float = RO * g * Volume;
			a = (m * g - F_float) / m;

			float v2 = velocity + a * Time.deltaTime;
			move.y = -(v2 + velocity) / 2;
			transform.Translate (move);

			velocity = v2;
			Volume += velocity * Time.deltaTime * BottomArea;
		}

	}
}
