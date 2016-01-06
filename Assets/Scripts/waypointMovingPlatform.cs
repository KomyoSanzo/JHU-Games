using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class waypointMovingPlatform : RayCastController
{
    //Add multiple waypoints
    //Ignore direction, modify only the speed
    //Add pauses between waypoints
    //Use splines

    public Vector3 move;
    public LayerMask passengerMask;

    public float speed = 1.0F;
    public float waitAtWaypointTime = 1.0F;
    public bool loop = true;
    public GameObject[] waypoints;

    private float startTime;
    private float journeyLength;

    private Transform startMarker, endMarker;
    private bool isReversed = false;
    int currentStartPoint;
    public override void Start()
    {
        base.Start();
        currentStartPoint = 0;
        SetPoints();

    }

    void SetPoints()
    {
        if(currentStartPoint == waypoints.Length-1)
        {
            Array.Reverse(waypoints);
            currentStartPoint = 0;
            isReversed = !isReversed;
        }
        startMarker = waypoints[currentStartPoint].transform;
        endMarker = waypoints[currentStartPoint + 1].transform;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        Vector3 lerpVal = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
        Vector3 velocity = lerpVal - transform.position;
        Debug.Log("transform pos: " + transform.position + " lerpVal: " + lerpVal + " velocity " + velocity);
        Debug.Log("Comparing vectors: " + (lerpVal == velocity));
        transform.position = lerpVal;
        //Calculate the velocity to use in MovePassengers function because i don't want to modify 
        //The move passengers function

        MovePassengers(velocity);
        if (fracJourney >= 1f )
        {
            currentStartPoint++;
            SetPoints();
        } 

    }

    void MovePassengers(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

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
                        hit.transform.Translate(new Vector3(pushX, pushY));

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
                        float pushY = 0;
                        hit.transform.Translate(new Vector3(pushX, pushY));
                        Debug.Log("moving");
                    }
                }

            }
        }
        //Passenger on Top of horizontal or downward moving platform
        if (directionY == -1 || velocity.x != 0.0F)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalSpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    Debug.Log("triggered");

                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;
                        hit.transform.Translate(new Vector3(pushX, pushY));

                    }
                }
            }
        }
    }
}
