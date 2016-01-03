using UnityEngine;
using System.Collections;

public class WeaponsScript : MonoBehaviour {

    public float shootCooldown = .4f;
    public float projectileSpeed = 3;
    float shootingRange = 0.25f;
    float shootTimer;
    
    public Transform shotPrefab;
    PlayerScript playerInformation;
    Animator anim;
    
	void Start ()
    {
        shootTimer = 0f;
        anim = GetComponent<Animator>();
        print(shootCooldown);
        playerInformation = GetComponent<PlayerScript>();
	}
	
	void Update ()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            shootTimer = 0;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (shootTimer == 0)
            {
                shootTimer = shootCooldown;
                Attack();
            }
            else
                print(shootTimer);
        }
        
	}

    void Attack()
    {
        anim.Play("shootingAnimation");
        

    }
    void shootProject()
    {
        var shotTransform = Instantiate(shotPrefab) as Transform;
        shotTransform.position = transform.position;
        if (playerInformation.facingRight)
        {
            Vector3 playerScale = shotTransform.localScale;
            playerScale.x = playerScale.x * -1;
            shotTransform.localScale = playerScale;
        }

        projectileMovement move = shotTransform.gameObject.GetComponent<projectileMovement>();
        if (move != null)
        {
            if (playerInformation.facingRight)
                move.direction = Vector2.right * projectileSpeed;
            else
                move.direction = Vector2.right * -1 * projectileSpeed;

        }

    }
    
}
