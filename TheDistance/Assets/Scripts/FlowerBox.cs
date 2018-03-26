using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlowerBox : MonoBehaviour {
    Camera mainCamera;
    int cnt = 0;
    int zoomFlag = 0;
    Text txt;
	public int Idx;
	GameObject boat ;

    bool hasTriggered = false;

    public int flowerFlag = 0;
    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		txt = GameObject.Find ("FlowerBox"+Idx.ToString()+"/Canvas/Text").GetComponent<Text> ();
		boat = GameObject.Find ("boat");
    }
	
    /*
	// Update is called once per frame
	void Update () {
        if (zoomFlag == 1)
        {
            mainCamera.orthographicSize += 0.01f;
			txt.DOFade (0, 1);

        }
        if (mainCamera.orthographicSize > 7.0f)
        {
			if (zoomFlag == 1) {
				this.gameObject.SetActive (false);
			}
        }
	
    }
     */ 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boat")
        {
			
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hasTriggered) return;
        if (collision.gameObject.tag == "Boat")
        {
            mainCamera.GetComponent<CamFollow>().FocusObject(collision.gameObject);
            txt.DOFade(0, 1).SetDelay(2.0f);
            hasTriggered = true;
        }
    }
}
