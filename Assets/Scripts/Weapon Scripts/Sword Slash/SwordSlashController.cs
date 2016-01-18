/**
The Sword Strike ability controller
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class SwordSlashController : Skill {
    //The hitbox to be instantiated
    public SwordHitBoxController hitbox;


	public override void Activate()
    {
        //Checks if an animator is obtained
        if (animator == null)
            animator = GetComponentInParent<Animator>();

        //Triggers the ability in the animator
        base.Activate();
        animator.SetTrigger("SwordSlash");
        
        //Instantiates the hitbox and modifies it to the players location
        var newHitBox = Instantiate(hitbox) as SwordHitBoxController;
        newHitBox.transform.position = playerInformation.transform.position;
        newHitBox.playerInformation = playerInformation;

        //Checks the player location for directional instantiation. 
        if (playerInformation.facingRight)
        {
            Vector3 playerScale = newHitBox.transform.localScale;
            playerScale.x = playerScale.x * -1;
            newHitBox.transform.localScale = playerScale;
        }

    }
}
