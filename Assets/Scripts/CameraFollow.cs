/**
Controls the battle camera for the player. Inspired by most 2D game camera implementations.
Written by Willis Wang
*/
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Controller2D target;
    public Vector2 areaSize;
    public float lookAheadXDistance;
    public float lookSmoothXTime;
    public float lookSmoothYTime;


    public float vertOffset;
    
    FocusArea focusArea;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirectionX;
    float smoothLookVelocityX; 
    float smoothVelocityY;
 

    void Start () {
        focusArea = new FocusArea(target.collision.bounds, areaSize);
	}
	
	void LateUpdate () {
        focusArea.Update(target.collision.bounds);
        Vector2 focusPos = focusArea.center + Vector2.up * vertOffset;

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);

        }

        targetLookAheadX = lookAheadDirectionX * lookAheadXDistance;
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothXTime);

        focusPos += Vector2.right * currentLookAheadX;

        transform.position = (Vector3)focusPos + Vector3.forward * -10;
        
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.center, areaSize);
    }

    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;
        float left, right;
        float top, bottom;


        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;

            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        
        public void Update(Bounds player)
        {
            float shiftX = 0;
            if (player.min.x < left)
            {
                shiftX = player.min.x - left;
            }
            else if (player.max.x > right)
            {
                shiftX = player.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (player.min.y < bottom)
            {
                shiftY = player.min.y - bottom;
            }
            else if (player.max.y > top)
            {
                shiftY = player.max.y - top;

            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);

        }
    }
}

