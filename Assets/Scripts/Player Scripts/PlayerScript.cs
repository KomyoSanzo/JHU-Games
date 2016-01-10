using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Controller2D))]

public class PlayerScript : MonoBehaviour
{
    Vector3 velocity;
    float velocityXSmoothing;
    float test;
    //PHYSICS AND GRAVITY INFORMATION
    public float jumpHeight = 4;
    public float jumpTime = .4f;
    public float moveSpeed = 6;

    //WEANING DIRECTION CHANGES
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;


    //DASHING INFORMATION
    public float dashCooldown = 1;
    float dashCurrentCooldown = 0;

    float dashButtonCooldown = 0.4f;
    int dashRightButtonCount = 0;
    int dashLeftButtonCount = 0;
    bool canDash;

    //INTERNAL VARIABLE DECLRATAIONS
    float gravity;
    float jumpVelocity;

    [HideInInspector] public bool isControllable = true;

    public AudioClip[] audioClips;

    [HideInInspector] public bool facingRight;




    Controller2D controller;
    Animator anim;
    AudioSource audioPlayer;
    
    void Start()
    {
        isControllable = true;
        audioPlayer = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        facingRight = true;
        controller = GetComponent<Controller2D>();
        gravity = -1 * (2 * jumpHeight) / Mathf.Pow(jumpTime, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpTime;
        print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        if (!controller.collisions.below && velocity.y != 0)
            anim.SetBool("midAir", true);
        else
            anim.SetBool("midAir", false);


    }

    void FixedUpdate()
    {
        
        CheckDash();
        

        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;

        Vector2 input;
        if (isControllable)
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else
            input = new Vector2(0, 0);
            
        if (Input.GetKey(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
            audioPlayer.clip = audioClips[6];
            audioPlayer.Play();
        }

        if (input.x != 0 && controller.collisions.below)
        {
            anim.SetBool("walking", true);
            if (!audioPlayer.isPlaying)
            {
                audioPlayer.clip = audioClips[Random.Range(4, 6)];
                audioPlayer.Play();
            }
        }
        else
            anim.SetBool("walking", false);

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
    
    void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            bool dashRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
            if (dashRight)
            {
                if (dashLeftButtonCount > 0)
                {
                    dashLeftButtonCount = 0;
                }
            }
            else
            {
                if (dashRightButtonCount > 0)
                {
                    dashRightButtonCount = 0;
                }
            }
            if (dashButtonCooldown > 0 && (dashRightButtonCount == 1 || dashLeftButtonCount == 1) && dashCurrentCooldown == 0)
            {
                if (dashRight)
                {
                    velocity.x = 30;
                    velocity.y = 10;
                }
                else
                {
                    velocity.x = -30;
                    velocity.y = 10;
                }
                dashCurrentCooldown = dashCooldown;
                audioPlayer.clip = audioClips[0];
                audioPlayer.Play();
                print("double tapped!");
                anim.Play("dashAnimation");
                StartCoroutine(dashCorouting());
            }
            else
            {
                dashButtonCooldown = 0.4f;
                if (dashRight)
                    dashRightButtonCount += 1;
                else
                    dashLeftButtonCount += 1;
            }
        }
        if (dashCurrentCooldown > 0)
        {
            dashCurrentCooldown -= Time.deltaTime;
        }
        else
        {
            dashCurrentCooldown = 0;
        }

        if (dashButtonCooldown > 0)
        {
            dashButtonCooldown -= 1 * Time.deltaTime;
        }
        else
        {
            dashLeftButtonCount = 0;
            dashRightButtonCount = 0;
        }

    }

   
    IEnumerator dashCorouting()
    {
        isControllable = false;
        yield return new WaitForSeconds(.2f);
        print("hello world!");
        isControllable = true;
    }
}
