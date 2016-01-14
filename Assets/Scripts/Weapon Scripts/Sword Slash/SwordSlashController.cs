using UnityEngine;
using System.Collections;

public class SwordSlashController : Skill {
    public SwordHitBoxController hitbox;


	public override void Activate()
    {
        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }
        base.Activate();
        animator.SetTrigger("SwordSlash");
        

        var newHitBox = Instantiate(hitbox) as SwordHitBoxController;
        newHitBox.transform.position = playerInformation.transform.position;
        newHitBox.playerInformation = playerInformation;

        if (playerInformation.facingRight)
        {
            Vector3 playerScale = newHitBox.transform.localScale;
            playerScale.x = playerScale.x * -1;
            newHitBox.transform.localScale = playerScale;
        }

    }
}
