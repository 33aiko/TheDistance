using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCircleCollider : MonoBehaviour {

    List<GameObject> nearObjectList = new List<GameObject>();

    bool keyDown = false;

    GameObject shareObject = null; // share this object;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "FloatingPlatform")
        {
            print(collision.gameObject.name + "enters the region");
            nearObjectList.Add(collision.gameObject);

            //show the halo when the key is down & platform inside the region
            (collision.gameObject.GetComponent("Halo") as Behaviour).enabled = keyDown;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FloatingPlatform")
        {
            //print(collision.gameObject.name + "leaves the region");
            nearObjectList.Remove(collision.gameObject);

            // remove the halo when it leaves the region
            (collision.gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }
    }

    public void highlightNearObject(bool flag = true)
    {
        keyDown = flag;
        foreach (GameObject t in nearObjectList)
        {
            (t.GetComponent("Halo") as Behaviour).enabled = flag;
        }
    }

    public void getDefaultShareObject()
    {
        // set the nearest object as default one
        if(nearObjectList.Count == 0)
        {
            //no near object here
            print("nothing can be shared");
            return;
        }
        GameObject nearestObject = null;
        float minDist = float.MaxValue;
        foreach (GameObject t in nearObjectList)
        {
            float cur = Vector3.Magnitude(t.transform.position - transform.position);
            if(cur < minDist)
            {
                minDist = cur;
                nearestObject = t;
            }
        }
        shareObject = nearestObject;
    }

    public GameObject shareSelectedObject()
    {
        if(shareObject == null)
        {
            print("nothing can be shared");
        }
        // add share code here
        return shareObject;
        // share "shareObject"
    }
}
