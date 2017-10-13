using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour {

    public float changeZValue;
	public float changeOffset; 

    int cnt = 0;
	float currentOffset; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                Player p = collision.transform.gameObject.GetComponent<Player>();
                p.cameraZoomValue = changeZValue;
				currentOffset = p.cameraOffset; 
				p.cameraOffset = changeOffset;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt--;
            if(cnt != 2)
            {
                Player p = collision.transform.gameObject.GetComponent<Player>();
                p.cameraZoomValue = 0;
				p.cameraOffset = currentOffset;
            }
        }
    }
}
