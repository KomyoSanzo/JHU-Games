﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class waypointMovingPlatform : RayCastController
{
    //Add multiple waypoints
    //Ignore direction, modify only the speed
    //Add pauses between waypoints
    //Use splines

    public LayerMask passengerMask;
    List<PassengerMovement> pMovements;
    Dictionary<Transform, Controller2D> pDict = new Dictionary<Transform, Controller2D>();

    public float speed = 1.0F;
    public float easingValue = 2.0F;

    //Waiting info
    public float waitAtWaypointTime = 1.0F;
    float waitCurrentCooldown = 0;

    public bool loop = true;
    public GameObject[] waypoints;

    private float startTime;
    private float journeyLength;

    private Transform startMarker, endMarker;

    private bool isReversed = false;

    //Check when to stop moving the platform
    private bool isStopped = false;

    //Which index was the last waypoint?
    int currentStartPoint;

    public override void Start()
    {
        base.Start();
        currentStartPoint = 0;
        SetPoints();

    }

    void SetPoints()
    {
        //If you reached the end of your travels loop the other way if 
        if(currentStartPoint == waypoints.Length-1)
        {
            if (loop)
            {
                Array.Reverse(waypoints);
                currentStartPoint = 0;
                isReversed = !isReversed;
            } else
            {
                isStopped = true;
            }
            
        } 
        if(isStopped == false)
        {
            startMarker = waypoints[currentStartPoint].transform;
            endMarker = waypoints[currentStartPoint + 1].transform;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        }
        

  



    }

    void Update()
    {
        if (isStopped == false)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            float easedFracJourney;
            if (fracJourney == 0)
            {
                easedFracJourney = 0;
            }else
            {
                //y = x ^ a / (x ^ a + (1 - x) ^ a)
                if(fracJourney > 1) //Sometimes distcovered is greater than journey length may be a worse bug
                {
                    fracJourney = 1;
                }
                easedFracJourney = Mathf.Pow(fracJourney, easingValue)
               / (Mathf.Pow(fracJourney, easingValue) + Mathf.Pow((1 - fracJourney), easingValue));
                //Debug.Log(fracJourney + "  " + Mathf.Pow(fracJourney, easingValue) + " " + easedFracJourney);
            }
           
            
            //Get the next position the platform should be in between two points
            Vector3 lerpVal = Vector3.Lerp(startMarker.position, endMarker.position, easedFracJourney);
            //Calculate velocity (after pos - before pos)
            Vector3 velocity = lerpVal - transform.position;


            //Debug.Log("transform pos: " + transform.position + " lerpVal: " + lerpVal + " velocity " + velocity);
            //Debug.Log("Comparing vectors: " + (lerpVal == velocity));

            //Willis's Code calls
            UpdateRaycastOrigins();
            CalculateMovement(velocity);
            MovePassengers(true);
            transform.Translate(velocity);
            MovePassengers(false);

            //Continue the journey to the next waypoint
            if (fracJourney >= 1f)
            {
                currentStartPoint++;
                SetPoints();
            }
        }
        

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
    void CalculateMovement(Vector3 velocity)
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
        if (directionY == -1 || velocity.x != 0)
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

        public PassengerMovement(Transform trans, Vector3 vel, bool stand, bool move)
        {
            transform = trans;
            velocity = vel;
            standing = stand;
            moveBefore = move;
        }
    }
}
