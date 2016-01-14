using UnityEngine;
using System.Collections;

public class WeaponControllerTest : MonoBehaviour {
    //public Skill[] skills;
    Skill[] skills;

    private GameObject activeSkill;
    private PlayerScript playerInformation;
    private Skill[] skillInformation;

    private float[] currentCooldowns;
    private KeyCode[] inputCodes;

    private int currentAbility;

    Animator animator;

    void Start ()
    {
        skills = GetComponentsInChildren<Skill>();

        animator = GetComponentInParent<Animator>();
        playerInformation = GetComponentInParent<PlayerScript>();

        inputCodes = new KeyCode[4];
        inputCodes[0] = KeyCode.Q;
        inputCodes[1] = KeyCode.W;
        inputCodes[2] = KeyCode.E;
        inputCodes[3] = KeyCode.R;

        currentCooldowns = new float[skills.Length];
        skillInformation = new Skill[skills.Length];

        for (int i = 0; i < skills.Length; i++)
        {
            skillInformation[i] = skills[i].GetComponent<Skill>();
            skillInformation[i].animator = animator;
            skillInformation[i].playerInformation = playerInformation;
            currentCooldowns[i] = 0;
        }



	}
	
	void Update ()
    {
        UpdateSkillCooldowns();
        for (int i = 0; i <skills.Length; i++)
        {
            if (checkSkill(i))
            {
                if (Input.GetKey(inputCodes[i]) && playerInformation.isControllable)
                {
                    currentAbility = i;
                    skillInformation[i].Activate();
                    currentCooldowns[i] = skillInformation[i].cooldown;
                }
            }
        }
	}

    void abilityChannelEnd()
    {
        skillInformation[currentAbility].endChannel();
    }

    bool checkSkill(int i)
    {
        return (currentCooldowns[i] <= 0);
    }
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
    public void SetActiveSkill(GameObject newSkill)
    {
        activeSkill = newSkill;
        foreach (var skill in skills)
        {
            skill.gameObject.SetActive(skill == activeSkill);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        

        for (int i = 0; i < skills.Length; i++)
        {
            GUI.color = Color.yellow;
            string displayText = skills[i].name + '\n';
            if (currentCooldowns[i] == 0)
                displayText += "READY";
            else
                displayText += currentCooldowns[i];
            displayText = displayText + '\n' + ((char)inputCodes[i]).ToString().ToUpper();
            GUILayout.TextArea(displayText, GUILayout.Width(128), GUILayout.Height(64));
        }
        GUILayout.EndHorizontal();
    }
}
