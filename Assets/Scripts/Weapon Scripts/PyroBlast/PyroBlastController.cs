/**
The PyroBlast ability controller.
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class PyroBlastController : Skill {
    
    //Set the ability's basic information and hitbox generator
    public Transform shotPrefab;
    public float projectileSpeed = 5f;

    
    /**
     * An overriden method that activates the ability from the weapon controller.
     */
    public override void Activate()
    {
        //Checks if the animator has been retrieved
        if (animator == null)
            animator = GetComponentInParent<Animator>();

        base.Activate();

        //Sets the animation trigger for the ability
        animator.SetTrigger("PyroBlast");

    }

    /**
     * At the end of the channel time, the player emits an attack prefab.
     */
    public override void endChannel()
    {
        base.endChannel();
        //Instantiate the fireball prefab
        var shotTransform = Instantiate(shotPrefab.transform) as Transform;

        //Adjust the positioning and the direction of the prefab
        shotTransform.position = playerInformation.transform.position;
        if (playerInformation.facingRight)
        {
            Vector3 playerScale = shotTransform.localScale;
            playerScale.x = playerScale.x * -1;
            shotTransform.localScale = playerScale;
        }

        //Modifies the projectileMovement controller that is attached to the projectile. 
        projectileMovement move = shotTransform.gameObject.GetComponent<projectileMovement>();
        if (move != null)
        {
            //moves the fireball in the direction of the Main Character
            if (playerInformation.facingRight)
                move.direction = Vector2.right * projectileSpeed;
            else
                move.direction = Vector2.right * -1 * projectileSpeed;

        }
    }
    
}
