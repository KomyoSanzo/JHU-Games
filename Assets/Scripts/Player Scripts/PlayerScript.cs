/** 
A script that controls the player's interaction with Main Character.
Written by Willis Wang 
*/

using UnityEngine;
using System.Collections;

//Requires Controller2D to run properly
[RequireComponent (typeof (Controller2D))]


public class PlayerScript : SimpleCharacterController
{
    //WEANING DIRECTION CHANGES 
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;


    //DASHING INFORMATION
    public float dashCooldown = 1;

    float dashCurrentCooldown = 0;
    float dashButtonCooldown = .1f;
    int dashRightButtonCount = 0;
    int dashLeftButtonCount = 0;
    bool canDash;

    float dashX = 25;
    float dashY = 0;

    //INTERNAL VARIABLE DECLARATIONS
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;

    
    //UNUSED VARIABLES (FOR LATER IMPLEMENTATION)
    [HideInInspector] public bool isAttacking;

    protected override void Start()
    {
        base.Start();
        gravity = -1 * (2 * maxJumpHeight) / Mathf.Pow(jumpTime, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * jumpTime;
        minJumpHeight = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        playerStats = GetComponent<PlayerStats>(); 
    }

    void Update()
    {
        
        if (!anim.GetBool("inUncancelableAttack"))
            CheckDash(); //Dashing is placed in Update due to more consistent input reading
        
        //Check if player is in midair or not
        if (!controller.collisions.below && velocity.y != 0)
            anim.SetBool("midAir", true);
        else
            anim.SetBool("midAir", false);
    }

    void FixedUpdate()
    {
        
        //Check for collisions       
        if (base.controller.collisions.above || base.controller.collisions.below)
            velocity.y = 0;
        movementUpdate();

        //Check player orientation
        checkFlip();
        
    }


    //==================================
    //ROUTINE FUNCTIONS
    //==================================
    
    /**
     * Check if the orientation of the player and flips the sprite renderer if necessary
     */
    void checkFlip()
    {   
        //Checks the current velocity of the player and the direction currently faced.
        if (velocity.x > 0 && !facingRight)
            Flip();
        else if (velocity.x < 0 && facingRight)
            Flip();
    }

    /**
     * Performs the physics calculations and moves the player in the input direction
     */
    void movementUpdate()
    {
        Vector2 input;

        //Check if need to maintain velocity. If not, then we can continue as normal.
        if (!maintainVelocity)
        {
            //Check if player is controllable. If so, then input is based on the Input parameter. Else, set to 0.
            if (isControllable && !anim.GetBool("inFullChannel"))
                input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            else
                input = new Vector2(0, 0);

            //Check if the player is jumping
            if (Input.GetKey(KeyCode.Space) && controller.collisions.below && !anim.GetBool("inFullChannel"))
                Jump();
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (velocity.y > minJumpVelocity)
                    velocity.y = minJumpVelocity;
            }
            
            //Check if walking by seeing if there exists a ground and a horizontal input
            if (input.x != 0 && controller.collisions.below)
            {
                //Set animator variables 
                anim.SetBool("walking", true);
                
                //Randomize a walking sound
                if (!audioPlayer.isPlaying)
                {
                    audioPlayer.clip = audioClips[Random.Range(4, 6)];
                    audioPlayer.Play();
                }
            }
            else
                //Set animator variables
                anim.SetBool("walking", false);

            //Wean towards the direction of the input. 
            float targetVelocityX = input.x * playerStats.speedCalculation(moveSpeed);
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        }
        //Apply gravity and move the player using the Controller2D's move function
        velocity.y += gravityModifier * gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /**
     * Flip the sprite renderer
     */
    void Flip()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("shootingAnimation") || anim.GetCurrentAnimatorStateInfo(0).IsName("swordStrike"))
            return;
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x = playerScale.x * -1;
        transform.localScale = playerScale;
    }
    
    /**
     * Checks if the player can dash and perform it if available. One thing to note is that the dashButtonCooldown is the time between key presses.
     */
    void CheckDash()
    {
        //Check if a left or right movement command has been entered.
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //check the direction of the key enter
            bool dashRight = Input.GetKeyDown(KeyCode.RightArrow);
        
            //Increment dash counter. Reset other direction's
            if (dashRight)
            {
                if (dashLeftButtonCount > 0)
                    dashLeftButtonCount = 0;
            }
            else
            {
                if (dashRightButtonCount > 0)
                    dashRightButtonCount = 0;
            }
            //Check if the same directional key has been pressed before and if the player can currently dash. 
            if (dashButtonCooldown > 0 && (dashRightButtonCount == 1 || dashLeftButtonCount == 1) && dashCurrentCooldown == 0)
            {
                print("dashing!");
                //Reset dash cooldown
                dashCurrentCooldown = dashCooldown;

                //Load dash sound clip and play
                audioPlayer.clip = audioClips[0];
                audioPlayer.Play();

                //Force the dash animation and start the coroutine
                anim.Play("dashAnimation");
                StartCoroutine(dashCorouting(dashRight));
            }
            else
            {
                //Otherwise, reset the dash button's cooldown and increment the count
                dashButtonCooldown = 0.4f;
                if (dashRight)
                    dashRightButtonCount += 1;
                else
                    dashLeftButtonCount += 1;
            }
        }

        //Handle cooldowns
        if (dashCurrentCooldown > 0)
            dashCurrentCooldown -= Time.deltaTime;
        else
            dashCurrentCooldown = 0;


        if (dashButtonCooldown > 0)
            dashButtonCooldown -= 1 * Time.deltaTime;
        else //If the player has not tapped a second time fast enough, reset the count
        {
            dashLeftButtonCount = 0;
            dashRightButtonCount = 0;
        }

    }




    //==================================
    //MOVEMENT HELPER FUNCTIONS
    //==================================
    
    /**
     * You get one guess as to what this does.
     */
    void Jump()
    {
        velocity.y = maxJumpVelocity;
        audioPlayer.clip = audioClips[6];
        audioPlayer.Play();
    }



    //==================================
    //COROUTINES/ASYNCHRONOUS FUNCTIONS
    //==================================

    /**
     *This function is an asynchronous function that causes the player to enter a dash animation 
     *while removing player control. 
     *@param direction - True for right and False for left. Controls the direction of the dash.
     */
    IEnumerator dashCorouting(bool direction)
    {
        //Remove control from player and remove gravity/friction effects on player
        gravityModifier = 0;
        isControllable = false;
        maintainVelocity = true;
        playerStats.setInvincible(true);

        //Force the animator to play the dashing animation
        anim.SetBool("dashing", true);

        //Change movement based on direction
        if (direction)
            velocity = new Vector2(dashX, 0);
        else
            velocity = new Vector2(-1*dashX, 0);


        //Wait the duration of the dash animation.
        //TO BE IMPLEMENTED: Make the wait time to be dependent on the GetAnimatorState clip duration. 
        yield return new WaitForSeconds(.2f);

        //Return control to player and clean up coroutine.
        isControllable = true;
        maintainVelocity = false;
        playerStats.setInvincible(false);

        gravityModifier = 1;
        anim.SetBool("dashing", false);
    }
}
