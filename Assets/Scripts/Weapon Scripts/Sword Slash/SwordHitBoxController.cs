/**
Controls the sword slash's hitbox
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class SwordHitBoxController : MonoBehaviour {
    //Basic hitbox information
    public float timeAlive = 2f;

    //Internal Variables
    float endTime;

    //Player-retrived information
    BoxCollider2D box;
    public CharacterController playerInformation;

    void Start () {
        box = GetComponent<BoxCollider2D>();

        //Set hitbox death time
        endTime = Time.time + timeAlive;
        
        //Set player as the parent of the object
        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;        
	}
	
	void Update () {
        //Constantly stay by the player
        transform.position = playerInformation.gameObject.transform.position;

        //Check for hitbox duration
        if (endTime < Time.time)
            Destroy(gameObject);
	}

    //========================================
    //ROUTINES
    //========================================
    void OnDrawGizmos()
    {
        //Draw a red box at the hitbox location
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(box.offset + (Vector2)transform.position, new Vector2(2, 2));
    }
}
