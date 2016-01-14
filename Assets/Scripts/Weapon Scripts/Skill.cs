using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

    public string skillName;
    public float cooldown;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerScript playerInformation;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Activate()
    {
        print("activating: " + skillName);
    }

    public virtual void endChannel()
    {
        print("ability fully channeled");
    }
}
