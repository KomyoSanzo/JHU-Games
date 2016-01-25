/**
Controls player-based projectiles
Written By Willis Wang
*/

using UnityEngine;
using System.Collections;

public class shotController : MonoBehaviour {

    //Public variables
    public float travelDistance = 30;
    public string ownerTag;


    //Private variables
    Transform trans;
    Vector3 startingDistance;

    void Start ()
    {
        //Obtains and sets the starting location
        trans = GetComponent<Transform>();
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
        //Ignores players
        if (collision.gameObject.tag.Equals(ownerTag))
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        else
            Destroy(gameObject);
    }
}
