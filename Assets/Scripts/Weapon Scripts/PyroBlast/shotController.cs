using UnityEngine;
using System.Collections;

public class shotController : MonoBehaviour {

    Transform transform;
    

    float travelDistance = 30;
    Vector3 startingDistance;

    void Start () {
        transform = GetComponent<Transform>();
        startingDistance = transform.position;
    }
	
	void Update () {
        if (Vector3.Distance(transform.position, startingDistance) >= travelDistance)
        {
            Destroy(gameObject);
        }    
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        else
            Destroy(gameObject);
    }
}
