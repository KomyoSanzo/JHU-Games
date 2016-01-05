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
}
