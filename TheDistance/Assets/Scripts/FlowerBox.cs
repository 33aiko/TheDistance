using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBox : MonoBehaviour {
    Camera mainCamera;
    int cnt = 0;
    int zoomFlag = 0;

    public int flowerFlag = 0;
    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        if (zoomFlag == 1)
        {
            mainCamera.orthographicSize += 0.01f;
        }
        if (mainCamera.orthographicSize > 7.0f)
        {
            zoomFlag = 0;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boat")
        {
            /*cnt++;
            if (cnt == 1)
            {
                RowBoat boat = collision.GetComponent<RowBoat>();
                this.gameObject.SetActive(false);
                //mainCamera.orthographicSize = 4.5f;
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boat")
        {
            if (flowerFlag == 1)
            {
                zoomFlag = 1;
            }
        }
    }
}
