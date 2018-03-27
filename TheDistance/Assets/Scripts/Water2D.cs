using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water2D : MonoBehaviour {

    Material waterM;

    void Start()
    {
        waterM = GetComponent<SpriteRenderer>().material;
        waterM.SetFloat("_Height", getWaterSurface());
    }

    private void Update()
    {
        //if (transform.hasChanged)
        {
            waterM.SetFloat("_Height", getWaterSurface());
            transform.hasChanged = false;
        }
    }

    // return the y of water surface in ndc space
    float getWaterSurface()
    {
        Vector3 upper = transform.position + Vector3.Scale(GetComponent<SpriteRenderer>().sprite.bounds.size, transform.localScale) / 2.0f;
        Debug.DrawRay(upper, Vector3.left * 100, Color.red);
        return Camera.main.WorldToViewportPoint(upper).y;
    }
}
