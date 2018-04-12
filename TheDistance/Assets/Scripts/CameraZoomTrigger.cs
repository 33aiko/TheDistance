using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class CameraZoomTrigger : MonoBehaviour {

    public float changeZValue;
	public float changeOffset; 

    int cnt = 0;
	float currentOffset = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "Player" && collision.gameObject.name == "Player")
        {
            cnt++;
            print("player enters zoom area!");
            if (cnt == 2)
            {
                ZoomPlayerCamera(collision);
            }
        }
    }

    void ZoomPlayerCamera(Collider2D collision)
    {
        print("Zooming the camera");
        Player p = collision.transform.gameObject.GetComponent<Player>();
        p.areaCameraZoomValue = changeZValue;
        //DOTween.To(() => p.currentCameraZoomValue, x => p.currentCameraZoomValue= x, changeZValue, 3);
        //DOTween.To(() => p.cameraZoomValue, x => p.cameraZoomValue = x, changeZValue, 3);
        p.TweenCameraZoomValue(changeZValue);
        currentOffset = p.cameraOffset;
        p.cameraOffset = changeOffset;
    }

    bool playerInside = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "Player" && collision.gameObject.name == "Player")
        {
            if (!playerInside)
            {
                playerInside = true;
                ZoomPlayerCamera(collision);
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
                p.areaCameraZoomValue = 0;
                DOTween.To(() => p.cameraZoomValue, x => p.cameraZoomValue = x, 0, 3);
                p.cameraOffset = currentOffset;
            }
        }
    }
}
