using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    /// <summary>
    /// 1 - The speed of the ship
    /// </summary>
    public Vector2 speed = new Vector2(5, 5);

    // 2 - Store the movement
    private Vector2 movement;

    private Rigidbody2D myScriptsRigidbody2D;

    void Start()
    {
        myScriptsRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 3 - Retrieve axis information
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 4 - Movement per direction
        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);
        // ...

        // 5 - Shooting
        bool shoot = Input.GetButtonDown("Fire1");
        shoot |= Input.GetButtonDown("Fire2");
        // Careful: For Mac users, ctrl + arrow is a bad idea

        if (shoot)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                // false because the player is not an enemy
                weapon.Attack(false);
            }
        }
        if (Input.GetKeyDown("space"))
        {
            transform.Translate(Vector2.up * 200 * Time.deltaTime, Space.World);
        }

    }

    void FixedUpdate()
    {
        // 5 - Move the game object
        myScriptsRigidbody2D.velocity = movement;
    }
}
