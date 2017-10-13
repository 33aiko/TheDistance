using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour {

    public float changeZValue;

    int cnt = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                Player p = collision.transform.gameObject.GetComponent<Player>();
                p.cameraZoomValue = changeZValue;
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
            }
        }
    }
}
