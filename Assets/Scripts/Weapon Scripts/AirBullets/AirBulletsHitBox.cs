using UnityEngine;
using System.Collections;

public class AirBulletsHitBox : Hitbox {

    public float travelDistance = 15;
    Vector3 startingDistance;


	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        startingDistance = trans.position;

	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Vector3.Distance(trans.position, startingDistance) >= travelDistance)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D (Collision2D collision)
    {
        Debug.Log("my tag: " + collision.gameObject.tag + ", " + ownerTag);
        if (collision.gameObject.tag.Equals(ownerTag))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            CharacterStats collisionStat = collision.gameObject.GetComponent<CharacterStats>();
            if (collisionStat != null)
            {
                collisionStat.TakeDamage(5);
            }
            Destroy(gameObject);
        }

    }


}
