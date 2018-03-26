using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBoat : MonoBehaviour {
    public float forceX = 2;
    public float forceY = 2;
    public GameObject oarEric;
    public GameObject oarNatalie;
    public Quaternion originalRotationEric;
    public Quaternion newRotationEric;
    public Quaternion tempRotationEric;
    public Quaternion originalRotationNatalie;
    public Quaternion newRotationNatalie;
    public Quaternion tempRotationNatalie;
    float interpolateTime = 10;
    float height;
    Rigidbody2D r;
	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody2D>();
        height = GetComponent<Renderer>().bounds.size.y;
        oarEric = transform.Find("oar_Eric").gameObject;
        oarNatalie = transform.Find("oar_Natalie").gameObject;

        originalRotationEric = oarEric.GetComponent<Transform>().localRotation;
        Vector3 eaEric;
        eaEric=originalRotationEric.eulerAngles;
        eaEric.z += 75;
        newRotationEric = Quaternion.Euler(eaEric);
        oarEric.GetComponent<Transform>().localRotation = newRotationEric;

        originalRotationNatalie = oarNatalie.GetComponent<Transform>().localRotation;
        Vector3 eaNatalie;
        eaNatalie = originalRotationNatalie.eulerAngles;
        eaNatalie.z -= 75;
        newRotationNatalie = Quaternion.Euler(eaNatalie);
        oarNatalie.GetComponent<Transform>().localRotation = newRotationNatalie;
        //newRotationEric = originalRotationEric;
    }
	
	// Update is called once per frame
	void Update () {
        if (r != null)
        {
            tempRotationEric = oarEric.GetComponent<Transform>().localRotation;
            tempRotationNatalie = oarNatalie.GetComponent<Transform>().localRotation;

            Vector3 bottom = (transform.position - transform.up * height / 2);
            Vector3 top = (transform.position + transform.up * height / 2);

            if (oarEric.GetComponent<Transform>().localRotation != newRotationEric)
            {
                Vector3 tempeaEric = tempRotationEric.eulerAngles;
                tempeaEric.z += 2.5f;
                tempRotationEric = Quaternion.Euler(tempeaEric);
                oarEric.GetComponent<Transform>().localRotation = tempRotationEric;
            }
            if (oarNatalie.GetComponent<Transform>().localRotation != newRotationNatalie)
            {
                Vector3 tempeaNatalie = tempRotationNatalie.eulerAngles;
                tempeaNatalie.z -= 2.5f;
                tempRotationNatalie = Quaternion.Euler(tempeaNatalie);
                oarNatalie.GetComponent<Transform>().localRotation = tempRotationNatalie;
            }
            /*      if(oarEric.GetComponent<Transform>().rotation == finalRotationEric)
                  {
                      newRotationEric = originalRotationEric;
                      oarEric.GetComponent<Transform>().rotation = originalRotationEric;
                  }*/
            /*
            if (Input.GetKey(KeyCode.DownArrow))
            {
                r.AddForceAtPosition(-transform.up * forceY + transform.right * forceX, new Vector3(top.x, top.y, transform.position.z));
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                // r.AddForce(forceX, forceY, 0);
                r.AddForceAtPosition(transform.up * forceY + transform.right * forceX, new Vector3(bottom.x, bottom.y, transform.position.z));
            }*/

            //UpdateCameraPosition();

        }
        
	}


    public Vector3 FollowBoxCenter()
    {
        CameraFollowBox pCFB = GetComponent<CameraFollowBox>();
        return pCFB.focusArea.center;
    }
    void UpdateCameraPosition()
    {
        CameraFollowBox pCFB = GetComponent<CameraFollowBox>();
        Vector3 ttmp = pCFB.focusArea.center - Camera.main.transform.position;
        Vector3 moveDistance = new Vector3(ttmp.x, ttmp.y, 0);
        GameObject.Find("Main Camera").GetComponent<CameraController>().Move(moveDistance);
        Camera.main.transform.position =
            new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y
            , transform.position.z - 25.0f);
    }


    public void move(int direction)
    {
        Debug.Log("V detected");
        Vector3 bottom = (transform.position - transform.up * height / 2);
        Vector3 top = (transform.position + transform.up * height / 2);
        if (direction==1)
        {
            r.AddForceAtPosition(-4*transform.up * forceY + 4*transform.right * forceX, new Vector3(top.x, top.y, transform.position.z));
        }
        else if (direction==0)
        {
            // r.AddForce(forceX, forceY, 0);
            r.AddForceAtPosition(4*transform.up * forceY + 4*transform.right * forceX, new Vector3(bottom.x, bottom.y, transform.position.z));
        }
    }

    public void oarMove(int player)
    {
        if (player == 0)
        {
            oarEric.GetComponent<Transform>().localRotation = originalRotationEric;
            Debug.Log(originalRotationEric);
        }
        else
        {
            oarNatalie.GetComponent<Transform>().localRotation = originalRotationNatalie;
            Debug.Log(originalRotationNatalie);
        }
        //newRotationEric = finalRotationEric;
    }
}
