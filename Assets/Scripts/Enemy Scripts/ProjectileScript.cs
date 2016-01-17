using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    /// <summary>
    /// Projectile prefab for shooting
    /// </summary>
    public Transform shotPrefab;

    //Cooldown in seconds between shots
    public float shootingRate = .25f;

    private float currentCooldown;

    // Use this for initialization
    void Start () {
        currentCooldown = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
	}
}
