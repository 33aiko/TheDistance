using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBoat : MonoBehaviour {
    public float forceX = 2;
    public float forceY = 2;
    float height;
    Rigidbody2D r;
	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody2D>();
        height = GetComponent<Renderer>().bounds.size.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (r != null)
        {
            Vector3 bottom = (transform.position - transform.up * height / 2);
            Vector3 top = (transform.position + transform.up * height / 2);
            /*
            if (Input.GetKey(KeyCode.DownArrow))
            {
                r.AddForceAtPosition(-transform.up * forceY + transform.right * forceX, new Vector3(top.x, top.y, transform.position.z));
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                // r.AddForce(forceX, forceY, 0);
                r.AddForceAtPosition(transform.up * forceY + transform.right * forceX, new Vector3(bottom.x, bottom.y, transform.position.z));
            }*/

        }
        
	}

    public void move(int direction)
    {
        Debug.Log("V detected");
        Vector3 bottom = (transform.position - transform.up * height / 2);
        Vector3 top = (transform.position + transform.up * height / 2);
        if (direction==0)
        {
            r.AddForceAtPosition(-4*transform.up * forceY + 4*transform.right * forceX, new Vector3(top.x, top.y, transform.position.z));
        }
        else if (direction==1)
        {
            // r.AddForce(forceX, forceY, 0);
            r.AddForceAtPosition(4*transform.up * forceY + 4*transform.right * forceX, new Vector3(bottom.x, bottom.y, transform.position.z));
        }
    }
}
