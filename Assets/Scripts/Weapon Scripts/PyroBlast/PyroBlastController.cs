using UnityEngine;
using System.Collections;

public class PyroBlastController : Skill {

    public Transform shotPrefab;
    public float projectileSpeed = 5f;

    
    public override void Activate()
    {
        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
            print("done");
        }
        base.Activate();
        animator.SetTrigger("PyroBlast");

    }

    public override void endChannel()
    {
        base.endChannel();
        var shotTransform = Instantiate(shotPrefab.transform) as Transform;

        shotTransform.position = playerInformation.transform.position;
        if (playerInformation.facingRight)
        {
            Vector3 playerScale = shotTransform.localScale;
            playerScale.x = playerScale.x * -1;
            shotTransform.localScale = playerScale;
        }

        projectileMovement move = shotTransform.gameObject.GetComponent<projectileMovement>();
        if (move != null)
        {
            if (playerInformation.facingRight)
                move.direction = Vector2.right * projectileSpeed;
            else
                move.direction = Vector2.right * -1 * projectileSpeed;

        }
    }
    
}
