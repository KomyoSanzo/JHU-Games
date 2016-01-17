using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
    public bool isEnemy;
    public int hp = 1;


    /// <summary>
    /// Inflicts damage and check if the object should be destroyed
    /// </summary>
    /// <param name="damageCount"></param>
    public void Damage(int damageCount)
    {
        hp -= damageCount;

        if (hp <= 0)
        {
            // Dead!
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a skill from the player? 
        //^^ This can be changed to whatever class we count as having damage
        Skill skill = otherCollider.gameObject.GetComponent<Skill>();

        /*if (skill != null)
        {
            // Avoid friendly fire
            if (skill.isEnemyShot != isEnemy)
            {
                Damage(skill.damage);

                // Destroy the shot
                Destroy(skill.gameObject); // Remember to always target the game object, otherwise you will just remove the script
            }
        }*/
    }
}
