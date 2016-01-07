using UnityEngine;
using System.Collections;

public class projectileMovement : MonoBehaviour
{

    public Vector2 speed = new Vector2(10, 10);
    public Vector2 direction = new Vector2(-1, 0);
    private Vector2 movement;
    private Rigidbody2D myRigidbody2D;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
    }

    void FixedUpdate()
    {
        myRigidbody2D.velocity = movement;
    }
}