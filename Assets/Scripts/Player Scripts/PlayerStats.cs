/**
A TO BE IMPLEMENTED controller of player health and status
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class PlayerStats : CharacterStats {
   
    float angerStatus;
    float sadStatus;
    float fearStatus;
    float happinessStatus;

    float speedModifier = 1f;
    float jumpModifier = 1f;


   
	public override void Start ()
    {
        base.Start();
        angerStatus = sadStatus = fearStatus = happinessStatus = 0;
        stunned = slowed = confused = Burning = false;        
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}


    /**
     * Player takes a certain amount of damage. Calleable by other GameObjects.
     * @param damage - the damage to be taken
     */
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        
        StartCoroutine(damageAnim(.2f));




        //If player health goes to or below 0, cause player death.
        if (Health<= 0)
        {
            playerDeath();
        }
    }

    IEnumerator damageAnim(float time)
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color (1, 1, 1, 1);

    }

    public override void hitStun()
    {
        base.setStun(true, .7f);
        PlayerScript playerInformation = gameObject.GetComponent<PlayerScript>();
        playerInformation.controller.collisions.below = false;
        playerInformation.velocity = new Vector2(playerInformation.facingRight ? -5 : 5, 5);

    }
    


    //==============================================
    //HELPER FUCTIONS
    //==============================================
    void playerDeath()
    {
        Destroy(gameObject);
    }
}
