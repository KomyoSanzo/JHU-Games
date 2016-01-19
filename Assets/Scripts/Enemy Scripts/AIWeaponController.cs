using UnityEngine;
using System.Collections;

/**
* Manages the skills and cooldowns of an AI. 
* Use activateAbility(int index) to start an attack
*/
public class AIWeaponController : MonoBehaviour {

    //Children Info
    private float[] currentCooldowns;
    private Skill[] skills;

    //Controller Info
    private int currentAbility;
    private GameObject activeSkill;

    //Player Components
    Animator animator;
    AIMoveScript AIInformation; //Mainly consists of 

	// Use this for initialization
	void Start () {
        //Obtain skills from children
        skills = GetComponentsInChildren<Skill>();

        //Obtain components from parent
        animator = GetComponentInParent<Animator>();
        AIInformation = GetComponentInParent<AIMoveScript>();

        //Set cooldowns
        currentCooldowns = new float[skills.Length];
        for(int i=0; i<skills.Length; i++)
        {
            currentCooldowns[i] = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        //Routines
        UpdateSkillCooldowns();
    }

    public bool activateAbility(int index)
    {
        if (checkSkill(index))
        {
            if(AIInformation.isControllable && !animator.GetCurrentAnimatorStateInfo(0).IsTag("ability"))
            {
                currentAbility = index;
                skills[index].Activate();
                currentCooldowns[index] = skills[index].cooldown;
                return true;
            }
        }
        return false;
    }

    //==========================================
    //ANIMATOR HELPER FUNCTIONS
    //==========================================
    /**
     * This method is for the animator to be able to access whenever a ability is ready to deploy a hitbox.
     */
    void abilityChannelEnd()
    {
        skills[currentAbility].endChannel();
    }

    //===========================================
    //HELPER FUNCTIONS
    //===========================================
    /**
     * Checks if an ability is off cooldown
     * @param i - the current ability index
     * @return - a true if it is on cooldown
     */
    bool checkSkill(int i)
    {
        return (currentCooldowns[i] <= 0);
    }


    //=======================================
    //ROUTINES
    //=======================================

    /**
     * Cycles through the abilities and lowers their cooldowns every time step
     */
    void UpdateSkillCooldowns()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (currentCooldowns[i] > 0)
                currentCooldowns[i] -= Time.deltaTime;
            else
                currentCooldowns[i] = 0;
        }
    }


}
