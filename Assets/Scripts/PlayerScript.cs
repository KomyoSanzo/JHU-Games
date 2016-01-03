using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    public bool isJumping = false;
    [HideInInspector]
    public bool isGrounded = false;

    public float jumpForce = 650.0f;
    public float maxSpeed = 7.0f;

    public Transform groundCheck;
    public LayerMask groundLayers;

    private float groundCheckRadius = 0.2f;

    private Rigidbody2D myRigidBody;


    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded == true)
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
                myRigidBody.AddForce(new Vector2(0, jumpForce));
            }
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle
            (groundCheck.position, groundCheckRadius, groundLayers);

        float move = Input.GetAxis("Horizontal");
        myRigidBody.velocity = new Vector2(move * maxSpeed, myRigidBody.velocity.y);

        if((move > 0.0f && isFacingRight == false) || (move <0.0f && isFacingRight == true))
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x = playerScale.x * -1;
        transform.localScale = playerScale;
    }
}
