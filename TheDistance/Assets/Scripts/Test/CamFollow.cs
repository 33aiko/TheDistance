using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamFollow : MonoBehaviour {
    public GameObject target;

    public float zoomInOrthoSize = 8.0f;
    public float zoomOutOrthoSize = 11.0f;
    public float zoomTime = 3.0f;
    public float speed = 10.0f;

	void Start () {
        target = FindObjectOfType<RowBoat>().gameObject;
	}

    public void FocusObject(GameObject from)
    {
        if (target == from) return;
        target = from;
        if(from.tag == "Boat")
        {
            CameraZoom(false);
        }
        else
        {
            Camera cam = Camera.main;
            cam.transform.DOMove(target.transform.position, 1.0f);
            CameraZoom(true);
        }
    }

    private void Update()
    {
        Vector3 targetPos;
        if(target.tag == "Boat")
        {
            targetPos = target.GetComponent<RowBoat>().FollowBoxCenter();
            Camera cam = Camera.main;
            Vector3 dir = target.transform.position - cam.transform.position;
            dir.z = -5;
            dir.Normalize();
            GetComponent<CameraController>().Move(dir * speed * Time.deltaTime);
        }
    }


    public void CameraMove(GameObject from, Vector3 moveDistance)
    {
        if(from == target)
        {
            GetComponent<CameraController>().Move(moveDistance);
        }
    }

    public void CameraZoom(bool isIn = true)
    {
        print("zoom in");
        Camera cam = Camera.main;
        float vic = cam.orthographicSize;
        DOTween.To(() => vic, x => {
            vic = x;
            cam.orthographicSize = vic;
        }, isIn ? zoomInOrthoSize : zoomOutOrthoSize, zoomTime);
    }
}
