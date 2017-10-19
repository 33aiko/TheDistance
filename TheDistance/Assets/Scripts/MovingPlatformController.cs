using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatformController : RaycastController
{

    public LayerMask passengerMask;
    public Vector3 move;
    public Vector3 curTranslate;
    public Vector3 targetTranslate;

    public bool canMove;
    public bool oneWay;
    public bool goingUp;
	public bool isMoved; 

	bool musicPlayed; 

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    public override void Start()
    {
        base.Start();
        canMove = false;
        //transform.position = defaultPosition;
        //targetPos2 = defaultPosition;
        goingUp = true;
		isMoved = false; 
		musicPlayed = false; 
    }

    void Update()
    {
        UpdateRaycastOrigins();

		if (canMove) { // if the step on trigger is triggered
//			if (!musicPlayed) {
//				GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("MovingPlatform");
//				musicPlayed = true; 
//			}
			
			Vector3 velocity;
			if (goingUp || oneWay) {
				Vector3 diff = (targetTranslate - curTranslate);
				velocity = move * Time.deltaTime;

				// in case the platform exceeds the check point in one frame
				if (diff.y < velocity.y) {
					velocity.y = diff.y;
					goingUp = false;
					gameObject.tag = "MovingPlatform";
					isMoved = true; 

				}
				if (diff.y < 0) {
					gameObject.tag = "MovingPlatform";
					goingUp = false;
					isMoved = true; 
				}
				curTranslate += velocity;
				//if (diff.x < velocity.x) velocity.x = diff.x;
			} else {
				Vector3 diff = curTranslate;
				velocity = -move * Time.deltaTime;

				// in case the platform exceeds the check point in one frame
				if (diff.y < Mathf.Abs (velocity.y)) {
					velocity.y = -diff.y;
					goingUp = true;
				}
				if (diff.y < 0)
					goingUp = true;
				curTranslate += velocity;
				//if (diff.x < velocity.x) velocity.x = diff.x;
			}

			if (velocity.y == 0) {
			}
			CalculatePassengerMovement (velocity);

			MovePassengers (true);
			transform.Translate (velocity);
			MovePassengers (false);
		} 
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
                Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.yellow);
                /*
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.yellow);
                 */

                if (hit)
                {
                    if (hit.transform.gameObject.GetComponent<Player>().playerUp)
                        continue;
                    if(rayOrigin.y > hit.transform.gameObject.GetComponent<Player>().controller.raycastOrigins.bottomLeft.y)
                        continue;
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, directionY==1));
                    }
                }
            }
        }

        // Horizontally moving platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform
        /*
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
        */
    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

}
