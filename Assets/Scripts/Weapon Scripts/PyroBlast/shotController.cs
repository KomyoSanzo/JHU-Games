/**
Controls player-based projectiles
Written By Willis Wang
*/

using UnityEngine;
using System.Collections;

public class shotController : Hitbox {

    //Public variables
    public float travelDistance = 30;

    //Private variables
    Vector3 startingDistance;

    public override void Start ()
    {
        base.Start();
        
        //Obtains and sets the starting location
        startingDistance = trans.position;
    }
	
	void Update ()
    {
        //If the projectil has traveled a certain distance, destroy it.
        if (Vector3.Distance(trans.position, startingDistance) >= travelDistance)
        {
            Destroy(gameObject);
        }    
	}

    /** 
     * Checks for collisions in the 2D space
     * @param collision - the object being collided against
     */
    void OnCollisionEnter2D(Collision2D collision)
    { 

        Debug.Log("my tag: " + collision.gameObject.tag + ", " + ownerTag);

        //Ignores players
        if (collision.gameObject.tag.Equals(ownerTag))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            print(LayerMask.LayerToName(this.gameObject.layer));
            print(LayerMask.LayerToName(collision.gameObject.layer));

            CharacterStats collisionStat = collision.gameObject.GetComponent<CharacterStats>();
            if (collisionStat != null)
            {
                collisionStat.TakeDamage(20);
                collisionStat.setBurning(true);
            }
            Destroy(gameObject);
        }
    }
}
