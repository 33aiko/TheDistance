using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour {
    Camera mainCamera;
    int cnt = 0;
    int zoomFlag = 0;
    FlowerBox fb;
    Text txt;
    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        fb = GameObject.Find("FlowerBox").GetComponent<FlowerBox>();
        txt = GameObject.Find("FlowerBox/Canvas/Text").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (zoomFlag == 1)
        {
            Debug.Log("?!?0");
            mainCamera.orthographicSize -= 0.01f;
            if (txt.color.a < 255)
            {
                Color newColor = txt.color;
                newColor.a = newColor.a + 0.01f;
                txt.color = newColor;
            }
        }
        if (mainCamera.orthographicSize < 5f)
        {
            zoomFlag = 0;
            this.gameObject.SetActive(false);
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
