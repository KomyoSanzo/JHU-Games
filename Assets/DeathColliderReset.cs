using UnityEngine;
using System.Collections;

public class DeathColliderReset : MonoBehaviour {
    public GameObject spawnPoint; 
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").gameObject;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        other.gameObject.transform.position = spawnPoint.transform.position;
    }
}
