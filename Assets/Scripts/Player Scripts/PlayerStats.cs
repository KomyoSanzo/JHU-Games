using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

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

    void TakeDamage(float damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            playerDeath();
        }
    }

    void playerDeath() { }
}
