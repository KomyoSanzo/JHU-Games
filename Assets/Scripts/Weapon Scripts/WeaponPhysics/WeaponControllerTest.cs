using UnityEngine;
using System.Collections;

public class WeaponControllerTest : MonoBehaviour {
    public GameObject[] skills;

    private GameObject activeSkill;
    private PlayerScript playerInformation;
    private Skill[] skillInformation;

    private float[] currentCooldowns;
    private KeyCode[] inputCodes;

    void Start ()
    {
        inputCodes[1] = KeyCode.Q;
        inputCodes[2] = KeyCode.W;
        inputCodes[3] = KeyCode.E;
        inputCodes[4] = KeyCode.R;


        for (int i = 0; i < skills.Length; i++)
        {
            skillInformation[i] = skills[i].GetComponent<Skill>();
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
                if (Input.GetKey(inputCodes[i]))
                {
                    skillInformation[i].Activate();
                }
            }
        }
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
        foreach (var skill in skills)
        {
            GUI.color = (skill == activeSkill) ? Color.yellow : Color.gray;
            if (GUILayout.Button(skill.name, GUILayout.Width(64), GUILayout.Height(64)))
            {
                SetActiveSkill(skill);
            }
        }
        GUILayout.EndHorizontal();
    }
}
