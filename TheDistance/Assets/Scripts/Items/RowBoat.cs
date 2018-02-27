using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBoat : MonoBehaviour {
    public float forceX = 2;
    public float forceY = 2;
    public GameObject oarEric;
    public GameObject oarNatalie;
    public Quaternion originalRotationEric;
    public Quaternion newRotationEric;
    public Quaternion finalRotationEric;
    public Quaternion tempRotationEric;
    float height;
    Rigidbody2D r;
	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody2D>();
        height = GetComponent<Renderer>().bounds.size.y;
        oarEric = transform.Find("oar_Eric").gameObject;
        oarNatalie = transform.Find("oar_Natalie").gameObject;
        originalRotationEric = oarEric.GetComponent<Transform>().localRotation;
        newRotationEric = new Quaternion(0.0f,0.0f,0.9f,0.4f);
        //newRotationEric.z = 0.4f;
        //newRotationEric.w = originalRotationEric.w;
        finalRotationEric = newRotationEric;
        oarEric.GetComponent<Transform>().localRotation = finalRotationEric;
        Debug.Log(originalRotationEric);
        Debug.Log(newRotationEric.w);
        Debug.Log(oarEric.GetComponent<Transform>().localRotation);
        //newRotationEric = originalRotationEric;
    }
	
	// Update is called once per frame
	void Update () {
        if (r != null)
        {
            tempRotationEric = oarEric.GetComponent<Transform>().localRotation;
            Vector3 bottom = (transform.position - transform.up * height / 2);
            Vector3 top = (transform.position + transform.up * height / 2);
            if (oarEric.GetComponent<Transform>().localRotation != finalRotationEric)
            {
                tempRotationEric.z += 0.05f;
                tempRotationEric.w -= (0.0001f);
                oarEric.GetComponent<Transform>().localRotation = tempRotationEric;
            }
      /*      if(oarEric.GetComponent<Transform>().rotation == finalRotationEric)
            {
                newRotationEric = originalRotationEric;
                oarEric.GetComponent<Transform>().rotation = originalRotationEric;
            }*/
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
        if (direction==1)
        {
            r.AddForceAtPosition(-4*transform.up * forceY + 4*transform.right * forceX, new Vector3(top.x, top.y, transform.position.z));
        }
        else if (direction==0)
        {
            // r.AddForce(forceX, forceY, 0);
            r.AddForceAtPosition(4*transform.up * forceY + 4*transform.right * forceX, new Vector3(bottom.x, bottom.y, transform.position.z));
        }
    }

    public void oarMove(int player)
    {
        oarEric.GetComponent<Transform>().localRotation = originalRotationEric;
        //newRotationEric = finalRotationEric;
    }
}
