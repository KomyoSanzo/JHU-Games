using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {
    //CHARACTER INFORMATION
    public float Health = 100f;


    protected bool paralyzed;
    protected bool slowed;
    protected bool confused;
    protected bool poisoned;
    protected bool burning;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
    }

}
