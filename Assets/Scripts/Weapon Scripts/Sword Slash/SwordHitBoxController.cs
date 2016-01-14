using UnityEngine;
using System.Collections;

public class SwordHitBoxController : MonoBehaviour {

    BoxCollider2D box;

    float endTime;

    public PlayerScript playerInformation;

    public float timeAlive = 2f;
	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider2D>();

        endTime = Time.time + timeAlive;
        
        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        print(transform.parent.transform.position);
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = playerInformation.gameObject.transform.position;
        if (endTime < Time.time)
            Destroy(gameObject);
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(box.offset + (Vector2)transform.position, new Vector2(2, 2));
    }
}
