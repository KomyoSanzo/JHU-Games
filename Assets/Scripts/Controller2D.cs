/**
A 2D physics engine implementation. Inspired by Sebastian Lague.
Written by Willis Wang
*/


using UnityEngine;
using System.Collections;

public class Controller2D : RayCastController
{
    //Collision information from the RayCastController
    public CollisionInfo collisions;

    //Angles that the player can traverse
    public float maxClimbingAngle = 80;
    public float maxDescentAngle = 50;

    public override void Start()
    {
        base.Start();
    }


    /** 
     * The movement function for GameObjects to use. Collisions are checked only when the character is moving to save computation time.
     * @param velocity - the velocity for the player to move in
     * @param standingOnPlatform - a default-assigned variable for if a player is on a variable
     */
    public void Move(Vector3 velocity, bool standingOnPlatform = false)
    {
        //Reset and update necessary information
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        //Check for necessary collisions/slopes
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity); //Because you'll fall and stutter and shit
        }
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity); //Check for a wall
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity); //Check for ceilings and platforms
        }


        //Translate the GameObject in the calculated direction
        transform.Translate(velocity);
    
        //Set the below collision true if standing on moving platform (think about when you're going down on a very fast platform)
        if (standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    /**
     * Checks for horizontal collisions and ascending slopes
     * @param velocity - a reference to a variable. This variable is adjusted based on the calculations in this method
     */
    void HorizontalCollisions(ref Vector3 velocity)
    {
        //Calculate the raylength based on velocity and the direction
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        //For each raycast
        for (int i = 0; i < horizontalRayCount; i++)
        {
            //Create the RayCast
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalSpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            //Display the ray
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            //If there is a hit
            if (hit)
            {
                //checks for an edge case                
                if (hit.distance == 0)
                {
                    continue;
                }

                //Angle of the slope by calculating the angle from the normal of the hit and upwards vector
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                //Check the lowest raycast for a slope
                if (i == 0 && slopeAngle <= maxClimbingAngle)
                {
                    //Reset descending slope information
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }

                    float distanceToSlopeStart = 0;

                    //New slope angle! Adjust information to the change
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }

                    //Adjust the velocity based on the slope
                    ClimbSlope(ref velocity, slopeAngle);

                    //Move to adjust the gap to prevent stuttering
                    velocity.x += distanceToSlopeStart * directionX;
                }

                //If not climbing or if the angle is too large
                if (!collisions.climbingSlope || slopeAngle > maxClimbingAngle)
                {
                    //Close the gap and reupdate the rayLength to the new "closest point"
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    
                    //Checks for a collision while climbing slope. 
                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    //Update collisions
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    /**
     * Checks for vertical collisions
     * @param velocity - A reference to the velocity vector to be changed based on the method calculations
     */
    void VerticalCollisions(ref Vector3 velocity)
    {
        //Calculate the raylength based on the y velocity and set direction
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        //For each vertical ray
        for (int i = 0; i < verticalRayCount; i++)
        {
            //Cast the ray
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalSpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            //Draw a ray to be displayed
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            //If there is a hit
            if (hit)
            {
                //Calculate Y velocity changes and adjust the raylength to the closest collision
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                //If climbing slope, adjust y velocity
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        //If climbing slope, Do maths. 
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

    /**
     * Climbs a slope. Involves lots of math. 
     * @param velocity - a reference to the velocity to be changed
     * @param slopeAngle - the slope angle to climb
     */
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        //Maffs.
        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    /**
     * Descends a slope. Involves maths.
     * @param velocity - a reference to the velocity vector to be changed.
     */
    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            //Calculate slope angle based on the normal of the hit ray and the y axis
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescentAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    //Maffs.
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

    /**
     * A struct to contain relevant collision and climbing information
     */
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope, descendingSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;
    
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