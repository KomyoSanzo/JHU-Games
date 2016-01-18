/**
A TO BE IMPLEMENTED controller of player health and status
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
    //PLAYER INFORMATION
    public float playerHealth = 100f;

    float angerStatus;
    float sadStatus;
    float fearStatus;
    float happinessStatus;

    bool paralyzed;
    bool slowed;
    bool confused;
    bool poisoned;
    bool burning;

    float speedModifier = 1f;
    float jumpModifier = 1f;

	void Start ()
    {
        angerStatus = sadStatus = fearStatus = happinessStatus = 0;
        paralyzed = slowed = confused = poisoned = burning = false;        
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    /**
     * Player takes a certain amount of damage. Calleable by other GameObjects.
     * @param damage - the damage to be taken
     */
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        //If player health goes to or below 0, cause player death.
        if (playerHealth <= 0)
        {
            playerDeath();
        }
    }

    //==============================================
    //HELPER FUCTIONS
    //==============================================
    void playerDeath()
    {
        Destroy(gameObject);
    }
}
