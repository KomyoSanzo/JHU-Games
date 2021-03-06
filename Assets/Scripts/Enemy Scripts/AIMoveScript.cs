﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]

public class AIMoveScript : SimpleCharacterController {
    public LayerMask collisionMask;
    
       
    Transform myTrans;
    float myWidth, myHeight;
    SpriteRenderer mySprite;

    //WEANING DIRECTION CHANGES
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    //Internal Variable Declarations
    float gravity;
    float jumpVelocity;

    BoxCollider2D bounds;

    EnemyStats stats;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        myTrans = this.transform;
        mySprite = this.GetComponent<SpriteRenderer>();
        bounds = this.GetComponent<BoxCollider2D>();
        myWidth = bounds.bounds.extents.x;
        myHeight = bounds.bounds.extents.y;
        gravity = -1 * (2 * maxJumpHeight) / Mathf.Pow(jumpTime, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpTime;
        maintainVelocity = true;
        stats = GetComponent<EnemyStats>();

	}



    void FixedUpdate()
    {
        if (base.controller.collisions.above || base.controller.collisions.below)
            velocity.y = 0;
        Vector2 lineCastPos;
        //Use this position to cast the isGrounded/isBlocked lines from
        if (facingRight)
        {
            lineCastPos = myTrans.position.toVector2()
                + myTrans.right.toVector2() * myWidth - Vector2.up * myHeight;
        } else
        {
            lineCastPos = myTrans.position.toVector2()
                - myTrans.right.toVector2() * myWidth - Vector2.up * myHeight; //Adding an offset to the height. 
        }
        //Check to see if there's ground in front of us before moving forward
        //NOTE: Unity 4.6 and below use "- Vector2.up" instead of "+ Vector2.down"

        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);

        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, collisionMask);
        //Check to see if there's a wall in front of us before moving forward
        bool blocked = false;

        if (base.controller.collisions.right || base.controller.collisions.left)
        {
            //Debug.Log(base.controller.collisions.right +" LSdkfj " + base.controller.collisions.left);
            blocked = true;
        }

        if (!isGrounded || blocked)
        {
            
            //Debug.Log("IS not grounded: " + !isGrounded + " isBlocked: " + blocked);
            Flip();
            moveSpeed *= -1;
        }

        //If character should continue
        if (maintainVelocity)
        {
            velocity.x = stats.speedCalculation(moveSpeed);

        } else
        {
            velocity.x = 0;
        }

        velocity.y += gravity * Time.deltaTime;


        controller.Move(velocity * Time.deltaTime);
    }
    void Flip()
    {
       
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x = playerScale.x * -1;
        transform.localScale = playerScale;
    }
}
