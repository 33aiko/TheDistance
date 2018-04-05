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

    public bool isShaking = false;
    //持续抖动的时长
    public float shake = 0f;

    // 抖动幅度（振幅）
    //振幅越大抖动越厉害
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    Vector3 originalPos;

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
            if (isShaking)
            {
               
                Transform camTransform = Camera.main.transform;
                if (shake > 0)
                {
                    camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                    shake -= Time.deltaTime * decreaseFactor;
                }
                else
                {
                    shake = 0f;
                    camTransform.localPosition = originalPos;
                    isShaking = false;
                }
            }
            else
            {
                targetPos = target.GetComponent<RowBoat>().FollowBoxCenter();
                Camera cam = Camera.main;
                Vector3 dir = target.transform.position - cam.transform.position;
                dir.z = -5;
                dir.Normalize();
                GetComponent<CameraController>().Move(dir * speed * Time.deltaTime);
            }
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



    public void CameraShake(float shaketime)
    {
        shake = shaketime;
        originalPos = Camera.main.transform.localPosition;
        isShaking = true;
    }
}
