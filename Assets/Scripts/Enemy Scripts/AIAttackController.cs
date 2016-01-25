using UnityEngine;
using System.Collections;

public class AIAttackController : MonoBehaviour {
    /// <summary>
    /// Where the enemy is
    /// </summary>
    public Transform enemyLocation;

    /// <summary>
    /// When should the enemy react?
    /// </summary>
    public float detectionDistance;

    /// <summary>
    /// Get the weaponController
    /// </summary>
    private AIWeaponController myWeaponController;
    /// <summary>
    /// The current character's location
    /// </summary>
    private Transform myCurrentLocation;

    /// <summary>
    /// Details from the player's moving script, check if facing right
    /// </summary>
    private AIMoveScript aiMoveInformation;

    // Use this for initialization
    void Start () {
        myWeaponController = GetComponent<AIWeaponController>();
        myCurrentLocation = GetComponent<Transform>();
        aiMoveInformation = GetComponent<AIMoveScript>();
	}
	
	// Update is called once per frame
	void Update () {
        //Get distance to player
        if (getDistanceToPlayer() <= detectionDistance) {
        }

        if (isEnemySeen())
        {
            //Activate the ability
            myWeaponController.activateAbility(0);
        }
	}

    /// <summary>
    /// Drawing stuff for scene editor
    /// </summary>
    void OnDrawGizmosSelected()
    {

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Vector3 vectorTo = this.transform.position;
            vectorTo.x = this.transform.position.x + detectionDistance;
        Vector3 size = new Vector3(1, 1, 1);
        Gizmos.DrawLine(this.transform.position, vectorTo);
        Debug.Log("no!");

    }

    /// <summary>
    /// Returns a vector with the distance to the player
    /// </summary>
    /// <returns></returns>
    float getDistanceToPlayer()
    {
        return Vector3.Distance(myCurrentLocation.position, enemyLocation.position);
    }

    /// <summary>
    /// Checks if the enemy is facing the player and is within a certain height difference to the player
    /// 
    /// </summary>
    /// <returns>True if seen, false o/w</returns>
    bool isEnemySeen()
    {
        Vector3 enemyPos = enemyLocation.position;
        Vector3 myPos = myCurrentLocation.position;
        float relativePosDiff = enemyPos.x - myPos.x;
        float verticalDiff = enemyPos.y - myPos.y;
        if (Mathf.Abs(relativePosDiff) < detectionDistance)
        {
            //Facing the right way?
            if (((relativePosDiff < 0 && !aiMoveInformation.facingRight) ||
                (relativePosDiff > 0 && aiMoveInformation.facingRight)) 
                && Mathf.Abs(verticalDiff) < 2f) //Within vertical view threshold
            {
                return true;

            }
        }
        return false;
    }

}
