﻿/**
The Sword Strike ability controller
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class SwordSlashController : Skill {
    //The hitbox to be instantiated
    

	public override void Activate()
    {
        //Checks if an animator is obtained
        if (animator == null)
            animator = GetComponentInParent<Animator>();

        //Triggers the ability in the animator
        base.Activate();
        animator.SetTrigger("SwordSlash");
        
        
    }

    public override void endChannel()
    {
        base.endChannel();
        //Instantiates the hitbox and modifies it to the players location
        Transform newHitBox = Instantiate(hitbox.transform) as Transform;

        SwordHitBoxController swordHitBox = newHitBox.gameObject.GetComponent<SwordHitBoxController>();
        swordHitBox.ownerTag = this.transform.parent.gameObject.tag;
        swordHitBox.transform.position = playerInformation.transform.position;
        swordHitBox.playerInformation = playerInformation;

        //Checks the player location for directional instantiation. 
        if (playerInformation.facingRight)
        {
            Vector3 playerScale = newHitBox.transform.localScale;
            playerScale.x = playerScale.x * -1;
            newHitBox.localScale = playerScale;
        }

    }
}
