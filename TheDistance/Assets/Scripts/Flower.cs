using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    int cnt = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boat")
        {
            cnt++;
            if (cnt == 1)
            {
                RowBoat boat = collision.GetComponent<RowBoat>();
                Debug.Log("eat flower flower");
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boat")
        {
            cnt--;
            if (cnt != 1)
            {
                RowBoat boat = collision.GetComponent<RowBoat>();
                Debug.Log("out flower");
            }
        }
    }
}
