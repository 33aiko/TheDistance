using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCircleCollider : MonoBehaviour {

    public List<GameObject> nearObjectList = new List<GameObject>();

    bool keyDown = false;

    GameObject shareObject = null; // share this object;
    GameObject arrow;
    int shareIdx;

    public List<string> sharableObjectTag = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //    if (collision.gameObject.tag == "FloatingPlatform" ||
        //collision.gameObject.tag == "Box" ||
        //(collision.gameObject.tag == "MovingPlatform" &&
        //collision.gameObject.GetComponent<MovingPlatformController>().oneWay)
        //)
        if (sharableObjectTag.Contains(collision.gameObject.tag))
        {
            //print(collision.gameObject.name + "enters the region");
            nearObjectList.Add(collision.gameObject);

            //show the halo when the key is down & platform inside the region
            //(collision.gameObject.GetComponent("Halo") as Behaviour).enabled = keyDown;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "FloatingPlatform" ||
        //    collision.gameObject.tag == "Box" ||
        //    (collision.gameObject.tag == "MovingPlatform" && 
        //    collision.gameObject.GetComponent<MovingPlatformController>().oneWay)
        //    )
        if (sharableObjectTag.Contains(collision.gameObject.tag))
        {
            //print(collision.gameObject.name + "leaves the region");
            nearObjectList.Remove(collision.gameObject);
            if(shareObject == collision.gameObject)
            {
                //getNextObject();
            }

            // remove the halo when it leaves the region
            //(collision.gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }
    }

    public void highlightNearObject(bool flag = true)
    {
        keyDown = flag;
//        foreach (GameObject t in nearObjectList)
//        {
//            Behaviour b = (t.GetComponent("Halo") as Behaviour);
//            b.enabled = flag;
//        }
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
        List<GameObject> removeList = new List<GameObject>();
        print("object in the list!");
        foreach (GameObject t in nearObjectList)
        {
            print(t.name);
            if(!sharableObjectTag.Contains(t.tag))
            {
                // this gameObject is no longer sharable
                print(t.name + " is not sharable anymore, removing it from the nearList");
                removeList.Add(t);
                continue;
            }
            float sizeY = GetComponent<BoxCollider2D>().size.y / 2;
            Vector3 basePosition = transform.position - new Vector3(0, sizeY);
            var rend = t.GetComponent<SpriteRenderer>();
            float platformY;
            if (rend == null)
                platformY = t.GetComponent<MeshRenderer>().bounds.size.y;
            else
                platformY = t.GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 platformPos = t.transform.position + new Vector3(0, platformY);
            float cur = 
                Vector3.Magnitude(platformPos- basePosition);
            if(cur < minDist)
            {
                minDist = cur;
                nearestObject = t;
            }
        }

        shareIdx = nearObjectList.IndexOf(nearestObject);
        shareObject = nearestObject;

        foreach(GameObject t in removeList)
        {
            nearObjectList.Remove(t);
        }

        if (shareObject == null)
            return;
		
		if(shareObject.GetComponentInChildren<SharingEffectsController> () != null)
            shareObject.GetComponentInChildren<SharingEffectsController>().PlaySelectedEffect();


        createArrow();
    }

    public void getNextObject()
    {
        shareIdx += 1;
        if(nearObjectList.Count == 0)
        {
            shareObject = null;
        }
        else
        {
            shareIdx = shareIdx % nearObjectList.Count;
            shareObject = nearObjectList[shareIdx];
            deletePrevArrow();
            createArrow();
        }
    }

	public GameObject getShareObject()
	{
		if (shareObject == null) {
			return null; 
		} else
			return shareObject;
	}

    public GameObject shareSelectedObject()
    {
        if(shareObject == null)
        {
            print("nothing can be shared");
            return null;
        }

        deletePrevArrow();
		if(shareObject.GetComponentInChildren<SharingEffectsController> () != null)
            shareObject.GetComponentInChildren<SharingEffectsController>().StopSelectedEffect();

        if (shareObject.tag == "Box")
        {
            print("Should be deleteing box!");
            nearObjectList.Remove(shareObject);
            GameObject res = shareObject;
            getNextObject();
            deletePrevArrow();
            return res;
        }

        return shareObject;
    }

    public void StopSharingEffect()
    {
		if(shareObject != null && shareObject.GetComponentInChildren<SharingEffectsController> () != null)
			shareObject.GetComponentInChildren<SharingEffectsController>().FadeOutEffect();
    }

    void createArrow()
    {
        // create arrow on the share object
        SpriteRenderer sp = shareObject.gameObject.GetComponent<SpriteRenderer>();
        if(sp != null)
        {
            Vector2 v = shareObject.gameObject.GetComponent<SpriteRenderer>().bounds.size;
            if (arrow == null)
            {
                arrow = Instantiate(Resources.Load("Prefabs/Items/ShareArrow")) as GameObject;
            }
            Vector3 tmp = new Vector3(0, v.y/2);
            arrow.transform.position = shareObject.transform.position + tmp;
        }
    }

    public void deletePrevArrow()
    {
        if(arrow != null)
            Destroy(arrow);
        arrow = null;
    }
}
