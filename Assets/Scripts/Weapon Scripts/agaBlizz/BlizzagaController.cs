/**
Blizzaga Controller 
Written By Willis Wang
*/

using UnityEngine;
using System.Collections;

public class BlizzagaController : Skill {
    

	// Use this for initialization
	public override void Start () {
        base.Start();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();

        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }
        animator.SetTrigger("blizzaga");

        GetComponent<AudioSource>().Play();
    }

    public override void endChannel()
    {
        base.endChannel();

        Transform newHitBox = Instantiate(hitbox.transform) as Transform;

        BlizzagaHitBoxController blizzagaHitbox = newHitBox.gameObject.GetComponent<BlizzagaHitBoxController>();
        blizzagaHitbox.ownerTag = this.transform.parent.gameObject.tag;
        blizzagaHitbox.transform.position = playerInformation.transform.position;
        blizzagaHitbox.playerInformation = playerInformation;

        if (playerInformation.facingRight)
        {
            Vector3 playerScale = newHitBox.transform.localScale;
            playerScale.x = playerScale.x * -1;
            newHitBox.localScale = playerScale;
        }

        
    }
}
