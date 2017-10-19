using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speiteControl : MonoBehaviour
{
    public float speed = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, Input.GetAxis("Vertical") * Time.deltaTime * speed, 0f);
	}
}
