using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Controller2D))]

public class PlayerScript : MonoBehaviour
{
    Vector3 velocity;
    float velocityXSmoothing;

    public float jumpHeight = 4;
    public float jumpTime = .4f;
    public float moveSpeed = 6;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    float gravity;
    float jumpVelocity;
    [HideInInspector] public bool facingRight;

    Controller2D controller;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        facingRight = true;
        controller = GetComponent<Controller2D>();
        gravity = -1 * (2 * jumpHeight) / Mathf.Pow(jumpTime, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpTime;
        print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    void FixedUpdate()
    {
        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;

        Vector2 input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKey(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }


        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (velocity.x > 0 && !facingRight)
            Flip();
        else if (velocity.x < 0  && facingRight)
            Flip();
        


    }

    void Flip()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("shootingAnimation"))
            return;
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x = playerScale.x * -1;
        transform.localScale = playerScale;
    }
    

}
