using UnityEngine;
using System.Collections;

public class BlizzagaHitBoxController : Hitbox {

    BoxCollider2D box;
    public SimpleCharacterController playerInformation;
    Animator playerAnim;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        box = GetComponent<BoxCollider2D>();

        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        playerAnim = GetComponentInParent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = playerInformation.gameObject.transform.position;
        if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("blizzaga"))
        {
            Destroy(gameObject);
        }
	
	}

    void OnCollisionStay2D (Collision2D collision)
    {
        CharacterStats collisionStat = collision.gameObject.GetComponent<CharacterStats>();
        if (collisionStat != null)
        {
            collisionStat.TakeDamage(1.5f);
            collisionStat.setSlow(true);
        }
    }
}
