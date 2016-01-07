using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class movingPlatformController : RayCastController {

    public Vector3 move;
    public LayerMask passengerMask;
    List<PassengerMovement> pMovements;
    Dictionary<Transform, Controller2D> pDict = new Dictionary<Transform, Controller2D>();


	public override void Start () {
        base.Start();


	}
	
	void Update () {

        UpdateRaycastOrigins();

        Vector3 velocity = move * Time.deltaTime;
        CalculateMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }
    
    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in pMovements)
        {
            if (!pDict.ContainsKey(passenger.transform))
            {
                pDict.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }
            if (passenger.moveBefore == beforeMovePlatform)
            {
                pDict[passenger.transform].Move(passenger.velocity, passenger.standing);
            }
        }
    }
    void CalculateMovement (Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        pMovements = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Vertical moving platform - Passenger
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalSpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        pMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                } 
            }
        }

        //Horizontal
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalSpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);
                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        pMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }

            }
        }

        //Passenger on Top of horizontal or downward moving platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalSpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        pMovements.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standing;
        public bool moveBefore;

        public PassengerMovement (Transform trans, Vector3 vel, bool stand, bool move)
        {
            transform = trans;
            velocity = vel;
            standing = stand;
            moveBefore = move;
        }
    }
}
