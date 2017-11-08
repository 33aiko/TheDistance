using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBox : MonoBehaviour {

    [HideInInspector]
    public FocusArea focusArea;
    public Vector2 focusAreaSize;
    public float yOffset;

    void Start()
    {
        focusArea = new FocusArea(GetComponent<BoxCollider2D>().bounds, focusAreaSize);
    }

    void Update()
    {
        focusArea.Update(GetComponent<BoxCollider2D>().bounds);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    public struct FocusArea
    {
        public Vector3 center;
        float left, right;
        float top, bottom;


        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            center = new Vector3((left + right) / 2, (top + bottom) / 2);
        }
    }
}
