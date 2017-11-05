using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : RaycastController
{

    float maxClimbAngle = 70;
    float maxDescendAngle = 70;

    public CollisionInfo collisions;

    public override void Start()
    {
        base.Start();
    }

    public void Move(Vector3 velocity, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x != 0) // if the player is moving in x-direction, check the collision
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0) // if the player is moving in y-direction, check the collision
        {
            VerticalCollisions(ref velocity);
        }

        //move the player after checking collision
        transform.Translate(velocity);

        if(standingOnPlatform)
        {
            collisions.below = true;
        }

    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            //check hit on x-direction
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            //Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                // push the box
                bool pushBox = false;
                if((hit.transform.gameObject.tag == "Box" ||
                    hit.transform.gameObject.tag == "BoxCannotShare" )&& collisions.interact) 
                {
                    pushBox = true;
                    velocity.x /= 2;
                    print("Pushing the box by " + gameObject.tag);
                    float pushX = velocity.x;
                    //float pushX = velocity.x - (hit.distance) * directionX;
                    //float pushY = -skinWidth;
                    float pushY = 0;
                    BoxController bo = hit.transform.gameObject.GetComponent<BoxController>();
                    bo.controller.Move(new Vector3(pushX, pushY));
                    if(bo.controller.collisions.left || bo.controller.collisions.right)
                    {
                        pushBox = false;
                    }
                }


                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                //if the player can climb the slope
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                // no slope or cannot climb
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    if (hit.transform.gameObject.tag != "FloatingPlatform" &&
                        hit.transform.gameObject.tag != "Ladder" &&
                        hit.transform.gameObject.tag != "FloatingPlatformShared" &&
                        !pushBox)
                    {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                    }
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            //Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                if(directionY == 1 && 
                    (hit.transform.gameObject.tag == "FloatingPlatform" 
                    || hit.transform.gameObject.tag == "MovingPlatform" 
                    || hit.transform.gameObject.tag == "Ladder"
                    || hit.transform.gameObject.tag == "FloatingPlatformShared") )
                {
                    //hits bottom of the floating platform
                    //will not collide
                    /* 
                    if(hit.transform.gameObject.tag == "MovingPlatform")
                        print("Hit the moving platform");
                    else
                        print("Hit bottom of the floating platform");
                        */
                }
                else if(directionY == -1 && collisions.canClimbLadder && hit.transform.gameObject.tag == "Ladder"
                    && collisions.playerClimbLadder)
                {

                }
                else
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                }

                if(hit.transform.gameObject.tag == "Water" && 
                    gameObject.tag == "Box" && (GetComponent<FloatInWater>()) )
                {
                    GetComponent<FloatInWater>().SetInWater();
                }

                if(hit.transform.gameObject.tag == "Box" && 
                    hit.transform.gameObject.GetComponent<FloatInWater>().isInWater)
                {
                    // player stands on a floating box
                    print("On the floating platform");
                    FloatInWater pFW = hit.transform.gameObject.GetComponent<FloatInWater>();
                    if(pFW.p == null)
                        pFW.p = GetComponent<Player>();
                    pFW.playerOnTop = true;
                    //pFW.movePlayer();
                    //velocity.y = hit.transform.gameObject.GetComponent<FloatInWater>().move.y;

                }

                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = (directionY == 1) &&
                    ((hit.transform.gameObject.tag != "FloatingPlatform") &&
                    (hit.transform.gameObject.tag != "MovingPlatform") &&
                    (hit.transform.gameObject.tag != "FloatingPlatformShared")
                    );
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool canClimbLadder;
        public bool playerClimbLadder;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;
        public bool onLadder;

        public bool interact;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}
