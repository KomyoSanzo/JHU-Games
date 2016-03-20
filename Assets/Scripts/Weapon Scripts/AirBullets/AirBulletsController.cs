using UnityEngine;
using System.Collections;

public class AirBulletsController : Skill {

    public float projectileSpeed = 5f;
    public float MaxAngle = 7.5f;
    public float bulletCount = 5;


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
        animator.SetTrigger("airbullets");
        GetComponent<AudioSource>().Play();
    }

    public override void endChannel()
    {
        base.endChannel();


        for (int i = 0; i < bulletCount; i++)
        {
            Transform shotTransform = Instantiate(hitbox.transform) as Transform;

            shotTransform.position = playerInformation.transform.position;

            if (playerInformation.facingRight)
            {
                Vector3 playerScale = shotTransform.localScale;
                playerScale.x = playerScale.x * -1;
                shotTransform.localScale = playerScale;
            }

            projectileMovement move = shotTransform.gameObject.GetComponent<projectileMovement>();
            AirBulletsHitBox shotController = shotTransform.gameObject.GetComponent<AirBulletsHitBox>();
            shotController.ownerTag = this.transform.parent.gameObject.tag;

            if (move != null)
            {
                float angle = Random.value * MaxAngle;
                if (Random.value > .5)
                    angle = angle * -1;


                if (playerInformation.facingRight)
                {
                    move.direction = new Vector2(projectileSpeed, projectileSpeed * Mathf.Tan(Mathf.Deg2Rad * angle));

                }
                else
                {
                    move.direction = new Vector2(-1f * projectileSpeed, projectileSpeed * Mathf.Tan(Mathf.Deg2Rad * angle));
                }
            }
        }

        
    }
}
