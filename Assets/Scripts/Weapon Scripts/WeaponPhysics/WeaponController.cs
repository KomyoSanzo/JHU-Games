/**
A script that controls the active abilities of the main character. Implementation v2.
Written by Willis Wang
*/
using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
    //CHILDREN INFORMATION
    private float[] currentCooldowns;
    private Skill[] skills;
    
    //CONTROLLER INFORMATION
    private KeyCode[] inputCodes;
    private int currentAbility;
    private GameObject activeSkill;

    //PLAYER COMPONENTS
    Animator animator;
    PlayerScript playerInformation;

    void Start ()
    {
        //Obtain skills from children
        skills = GetComponentsInChildren<Skill>();

        //Obtain components from parent
        animator = GetComponentInParent<Animator>();
        playerInformation = GetComponentInParent<PlayerScript>();


        //Set controls. 
        //TO BE IMPLEMENTED: Inherit this from an InputController
        inputCodes = new KeyCode[4];
        inputCodes[0] = KeyCode.Q;
        inputCodes[1] = KeyCode.W;
        inputCodes[2] = KeyCode.E;
        inputCodes[3] = KeyCode.R;


        //Set cooldowns
        currentCooldowns = new float[skills.Length];
        for (int i = 0; i < skills.Length; i++)
        {
            currentCooldowns[i] = 0;
        }



	}
	
	void Update ()
    {
        //Routines
        UpdateSkillCooldowns();

        //Cycle through all abilities
        for (int i = 0; i <skills.Length; i++)
        {
            //Check if ability is off cooldown
            if (checkSkill(i))
            {
                //Check if the player is capable of executing an ability. 
                if (Input.GetKeyDown(inputCodes[i])
                    && playerInformation.isControllable 
                    && !animator.GetCurrentAnimatorStateInfo(0).IsTag("ability"))
                {
                    //activate, reset cooldowns, and set active ability
                    currentAbility = i;
                    skills[i].Activate();
                    currentCooldowns[i] = skills[i].cooldown;
                }
            }
        }
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

    /**
     * Draws cooldowns every frame
     */
    void OnGUI()
    {
        //Begin a horizontal layout at the top left side of the screen
        GUILayout.BeginHorizontal();
        
        //Draw each skill
        for (int i = 0; i < skills.Length; i++)
        {
            //Display name of ability
            string displayText = skills[i].skillName + '\n';

            //Check current cooldown
            if (currentCooldowns[i] == 0)
                displayText += "READY";
            else
                displayText += currentCooldowns[i];
            displayText = displayText + '\n' + ((char)inputCodes[i]).ToString().ToUpper();

            //Draw the icons and text
            GUI.DrawTexture(new Rect(i*84, 0, 80, 80), skills[i].icon);           
            GUILayout.TextArea(displayText, GUILayout.Width(80), GUILayout.Height(80));
        }


        GUILayout.EndHorizontal();
    }




    


    //=====================================
    //DEPRECATED METHODS
    //=====================================
    public void SetActiveSkill(GameObject newSkill)
    {
        activeSkill = newSkill;
        foreach (var skill in skills)
        {
            skill.gameObject.SetActive(skill == activeSkill);
        }
    }

    void setActive()
    {
        playerInformation.isAttacking = true;
    }

    void setInactive()
    {
        playerInformation.isAttacking = false;
    }
}
