using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public int rayCount;
    float xRaySpacing;
    float yRaySpacing;

    Vector2 curSize;

    Vector3 bottomLeft;
    Vector3 bottomRight;
    Vector3 topLeft;
    Vector3 topRight;

    Camera cam;

    public LayerMask collisionMask;

	void Start () {
        cam = GetComponent<Camera>();
	}
	
	void UpdateCollisionBox() {
        curSize = new Vector2(200, 200);
        //Vector2 curSize = new Vector2(cam.pixelWidth, cam.pixelHeight);
        xRaySpacing = curSize.x / (rayCount - 1);
        yRaySpacing = curSize.y / (rayCount - 1);
        bottomLeft = 
            cam.transform.position + new Vector3(-curSize.x, -curSize.y);
        bottomRight = 
            cam.transform.position + new Vector3(curSize.x, -curSize.y);
        topLeft = 
            cam.transform.position + new Vector3(-curSize.x, curSize.y);
        topRight = 
            cam.transform.position + new Vector3(curSize.x, curSize.y);
	}

    public void Move(Vector3 velocity)
    {
        UpdateCollisionBox();
        if (velocity.y != 0)
        {
            VerticalCheck(ref velocity);
        }
        if (velocity.x != 0)
        {
            HorizontalCheck(ref velocity);
        }
        transform.Translate(velocity);
    }

    void HorizontalCheck(ref Vector3 velocity)
    {
        Vector3 rayOrigin = transform.position;
        float rayLength = curSize.x + Mathf.Abs(velocity.x);
        float directionX = Mathf.Sign(velocity.x);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.right * directionX, rayLength, collisionMask);
        //Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
        if (hit)
        {
            velocity.x = 0;
        }
    }

    void VerticalCheck(ref Vector3 velocity)
    {
        Vector3 rayOrigin = transform.position;
        float rayLength = curSize.y + Mathf.Abs(velocity.y);
        float directionY = Mathf.Sign(velocity.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.up* directionY, rayLength, collisionMask);
        //Debug.DrawRay(rayOrigin, Vector2.up* directionY * rayLength, Color.red);
        if (hit)
        {
            velocity.y = 0;
        }
    }
}
