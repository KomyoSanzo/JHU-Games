using UnityEngine;
using System.Collections;

public class EnemyStats : CharacterStats {

    /**
     * Player takes a certain amount of damage. Calleable by other GameObjects.
     * @param damage - the damage to be taken
     */
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //If player health goes to or below 0, cause player death.
        if (Health <= 0)
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
