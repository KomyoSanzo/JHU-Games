using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]

public class AIMoveScript : MonoBehaviour {
    public LayerMask enemyMask;
    public Transform enemyLocation;
    public int detectionDistance;
    public int health;
    public float speed = 1.0f;
    
        
    Transform myTrans;
    float myWidth, myHeight;
    SpriteRenderer mySprite;


    [HideInInspector]
    public Vector3 velocity;
    float velocityXSmoothing;

    //PHSYICS AND GRAVITY INFORMATION

    public float jumpHeight = 4;
    public float jumpTime = .4f;
    public float moveSpeed = 6;

    //WEANING DIRECTION CHANGES
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    //Internal Variable Declarations
    float gravity;
    float jumpVelocity;

    [HideInInspector]
    public bool facingRight;

    [HideInInspector]
    public Controller2D controller;

    Animator anim;
    AudioSource audioPlayer;


	// Use this for initialization
	void Start () {
        myTrans = this.transform;
        mySprite = this.GetComponent<SpriteRenderer>();
        myWidth = mySprite.bounds.extents.x;
        myHeight = mySprite.bounds.extents.y;

        facingRight = true;


        controller = GetComponent<Controller2D>();
        gravity = -1 * (2 * jumpHeight) / Mathf.Pow(jumpTime, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpTime;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}


    void FixedUpdate()
    {
        if (controller.collisions.above || controller.collisions.below)
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
                - myTrans.right.toVector2() * myWidth - Vector2.up * myHeight;
        }
        //Check to see if there's ground in front of us before moving forward
        //NOTE: Unity 4.6 and below use "- Vector2.up" instead of "+ Vector2.down"

        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);

        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        //Check to see if there's a wall in front of us before moving forward
        Debug.DrawLine(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f, Color.blue);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f, enemyMask);
        //If theres no ground, turn around. Or if I hit a wall, turn around

        if (!isGrounded || isBlocked)
        {
            Flip();
            speed *= -1;
        }



        velocity.x = speed;
        

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
