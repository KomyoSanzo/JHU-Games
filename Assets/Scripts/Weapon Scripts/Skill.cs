using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

    public string skillName;
    public float cooldown;
    

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Activate()
    {
        print("activating: " + skillName);
    }
}
