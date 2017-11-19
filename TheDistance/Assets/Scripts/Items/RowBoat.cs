using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBoat : MonoBehaviour {
    public float forceX = 10;
    public float forceY = 10;
    Rigidbody r;
	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (r != null)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                r.AddForce(forceX, -forceY, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                r.AddForce(forceX, forceY, 0);
            }
        }
	}
}
