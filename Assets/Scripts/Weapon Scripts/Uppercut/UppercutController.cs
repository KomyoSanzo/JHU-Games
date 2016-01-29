/**
Tatsumaki ability controller 
Written By Willis Wang
*/

using UnityEngine;
using System.Collections;

public class UppercutController : Skill{
    public Transform hitboxPrefab;
    AudioSource audioPlayer;

    void Start()
    {
        //Obtain information from player
        animator = GetComponentInParent<Animator>();
        playerInformation = GetComponentInParent<PlayerScript>(); 
        audioPlayer = GetComponent<AudioSource>();
    }


    /** 
     * Follows the basic workflow of setting the attack trigger
     */
	public override void Activate()
    {
        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }
        base.Activate();
        animator.SetTrigger("uppercut");    
    }

    /**
     * If the animation plays, activate the ability and create a hitbox
     */
    public override void endChannel()
    {
        base.endChannel();


        if (playerInformation == null)
            playerInformation = GetComponentInParent<PlayerScript>();

        playerInformation.controller.collisions.below = false;
        StartCoroutine(moveUp());
        audioPlayer.Play();
    }


    //=======================================================
    //COROUTINES
    //=======================================================

    /**
     * Moves the player upwards in the direction the main character is facing
     */
    IEnumerator moveUp()
    {
        //Set player as uncontrollable and remove gravity effects
        playerInformation.isControllable = false;
        playerInformation.gravityModifier = 0;


        //Creates the hitbox
        Transform uppercutHitBox = Instantiate(hitboxPrefab.transform) as Transform;
        UppercutHitboxController newHitbox = uppercutHitBox.gameObject.GetComponent<UppercutHitboxController>();

        newHitbox.ownerTag = this.transform.parent.gameObject.tag;
        newHitbox.transform.position = playerInformation.transform.position;
        newHitbox.playerInformation = playerInformation;

        //Checks the player location for directional instantiation. 
        if (playerInformation.facingRight)
        {
            Vector3 playerScale = uppercutHitBox.transform.localScale;
            playerScale.x = playerScale.x * -1;
            uppercutHitBox.localScale = playerScale;
        }



        //Move player slightly up to avoid moving platform collisions
        playerInformation.gameObject.transform.Translate(Vector3.up*.5f);
        
        //Check the direction of the player and set the upward velocity
        if (playerInformation.facingRight)
            playerInformation.velocity = new Vector2(1f, 10f);
        else
            playerInformation.velocity = new Vector2(-1f, 10f);

        //Wait for the animation to complete, then return control
        //TO BE IMPLEMENTED: Make the duration dependent on the GetCurrentAnimatorClipInfo length
        yield return new WaitForSeconds(.4f);
        playerInformation.isControllable = true;
        playerInformation.gravityModifier = 1;
        Destroy(uppercutHitBox.gameObject);
    }
}
