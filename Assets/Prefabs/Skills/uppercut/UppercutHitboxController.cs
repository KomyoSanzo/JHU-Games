using UnityEngine;
using System.Collections;

public class UppercutHitboxController : Hitbox
{

    BoxCollider2D box;
    public CharacterController playerInformation;
    public Animator playerAnim;

	public override void Start ()
    {
        base.Start();
        box = GetComponent<BoxCollider2D>();
        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        playerAnim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        //Constantly stay by the player
        transform.position = playerInformation.gameObject.transform.position;
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (!hitObjects.Contains(collision.gameObject.transform))
        {
            hitObjects.Add(collision.gameObject.transform);
            CharacterStats collisionStat = collision.gameObject.GetComponent<CharacterStats>();
            if (collisionStat != null)
            {
                collisionStat.TakeDamage(20);
                collision.gameObject.GetComponent<CharacterStats>().setStun(true, 2f);
                collision.gameObject.GetComponent<CharacterController>().velocity.y += 20f;
                collision.gameObject.GetComponent<CharacterController>().velocity.x = 0;
                collision.gameObject.transform.Translate(Vector3.up * .5f);
                collision.gameObject.GetComponent<CharacterController>().controller.collisions.below = false;


            }
        }
    }
}
