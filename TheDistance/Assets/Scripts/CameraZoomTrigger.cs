using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class CameraZoomTrigger : MonoBehaviour {

    public float changeZValue;
	public float changeOffset; 

    int cnt = 0;
	float currentOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "Player" && collision.gameObject.name == "Player")
        {
            cnt++;
            if (cnt == 2)
            {
                print("Zooming the camera");
                Player p = collision.transform.gameObject.GetComponent<Player>();
                // p.cameraZoomValue = changeZValue;
                DOTween.To(() => p.cameraZoomValue, x => p.cameraZoomValue = x, changeZValue, 3);
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
            if (cnt != 2)
            {
                Player p = collision.transform.gameObject.GetComponent<Player>();
                //   p.cameraZoomValue = 0;
                DOTween.To(() => p.cameraZoomValue, x => p.cameraZoomValue = x, 0, 3);
                p.cameraOffset = currentOffset;
            }
        }
    }
}
