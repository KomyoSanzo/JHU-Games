/**
A Controller for dealing with collision casting. Inspired by Sebastian Lague.
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

//Requires a Collider
[RequireComponent(typeof(Collider2D))]

public class RayCastController : MonoBehaviour {

    //The layer to check against
    public LayerMask collisionMask;
    public LayerMask dodgeMask;

    //Public variables
    public const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    //Hidden from inspector
    [HideInInspector] public float horizontalSpacing;
    [HideInInspector] public float verticalSpacing;

    [HideInInspector]
    public Collider2D collision;
    public RaycastOrigins raycastOrigins;



    public virtual void Awake()
    {
        collision = GetComponent<Collider2D>();
    }

    public virtual void Start()
    {
        //Calculate ray spacings for future use
        CalculateRaySpacing();
    }

    /**
     * Changes the origins of the raycasts to the new collider location
     */
    public void UpdateRaycastOrigins()
    {
        //Obtains the bounds of the collision box and expands them by the skinwidth
        Bounds bounds = collision.bounds;
        bounds.Expand(skinWidth * -2);

        //Adjust the corners based on the new x and y locations
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    /**
    * Calculates the spacing between individual ray casts
    */
    public void CalculateRaySpacing()
    {
        //Obtains the bounds of the collision box and expands them by the skin width
        Bounds bounds = collision.bounds;
        bounds.Expand(skinWidth * -2);

        //Requires at least two ray casts for both directions
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        //Divide the spacing based on the count
        horizontalSpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalSpacing = bounds.size.x / (verticalRayCount - 1);
    }

    /**
     A struct that contains the origins of the raycasts
    */
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

}
