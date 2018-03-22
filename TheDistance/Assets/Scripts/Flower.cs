using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Flower : MonoBehaviour {
    Camera mainCamera;
    int cnt = 0;
    int zoomFlag = 0;
    public FlowerBox fb;
    Text txt;

    public int Idx;

    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		txt = GameObject.Find("FlowerBox"+Idx.ToString()+"/Canvas/Text").GetComponent<Text>();
		txt.DOFade (0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (zoomFlag == 1)
        {
            //Debug.Log("?!?0");
            mainCamera.orthographicSize -= 0.01f;
			txt.DOFade (1, 1);
        }
        if (mainCamera.orthographicSize < 5f)
        {
            //zoomFlag = 0;
            //this.gameObject.SetActive(false);
			Debug.Log(Idx);
			if (zoomFlag == 1) {
				GameObject.Find ("Flower" + Idx.ToString ()).SetActive (false);
			}
		}
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
                //this.gameObject.SetActive(false);
                zoomFlag = 1;
                fb.flowerFlag = 1;
            }

			mainCamera.GetComponent<CamFollow> ().target = this.gameObject;

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
