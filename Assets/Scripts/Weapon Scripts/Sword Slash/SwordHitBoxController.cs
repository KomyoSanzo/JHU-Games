/**
Controls the sword slash's hitbox
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class SwordHitBoxController : Hitbox {
    //Basic hitbox information
    public float timeAlive = 2f;

    //Internal Variables
    float endTime;
    
    //Player-retrived information
    BoxCollider2D box;
    public CharacterController playerInformation;
    Animator playerAnim;

    public override void Start () {
        base.Start();
        box = GetComponent<BoxCollider2D>();

        //Set hitbox death time
        endTime = Time.time + timeAlive;
        
        //Set player as the parent of the object
        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        playerAnim = GetComponentInParent<Animator>();       
	}
	
	void Update () {
        //Constantly stay by the player
        transform.position = playerInformation.gameObject.transform.position;

        if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("swordStrike"))
        {
            Destroy(gameObject);
        }
       
        //Check for hitbox duration
        //if (endTime < Time.time)
        //    Destroy(gameObject);
	}

    //========================================
    //ROUTINES
    //========================================
    /*void OnDrawGizmos()
    {
        //Draw a red box at the hitbox location
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube((Vector2)box.bounds.center, new Vector2(2, 2));
    }*/

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (!hitObjects.Contains(collision.gameObject.transform))
        {
            hitObjects.Add(collision.gameObject.transform);
            CharacterStats collisionStat = collision.gameObject.GetComponent<CharacterStats>();
            if (collisionStat != null)
            {
                collisionStat.TakeDamage(20);
            }
        }
    }
}
