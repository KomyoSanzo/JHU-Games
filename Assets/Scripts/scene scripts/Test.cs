using UnityEngine;
using System.Collections;

/**
public class Test : MonoBehaviour {

    public ActionBarRow WeaponsBar;
    ActionBarDescriptor[] spellDescriptor = new ActionBarDescriptor[0];
	
    
    void Start () {
        spellDescriptor = new ActionBarDescriptor[3];
        for (int i = 0; i < spellDescriptor.Length; i++)
        {
            float cooldown = WeaponController.Instance.skills[i].cooldown;
            spellDescriptor[i] = new ActionBarDescriptor
            {
                Atlas = 2,
                Icon = i,
                Callback = (d) =>
                {
                    d.Cooldown = cooldown;
                },
            };
        }

        WeaponsBar.AddInitCallback((row) =>
        {
            row.SetButton(0, spellDescriptor[0]);
            row.SetButton(1, spellDescriptor[1]);
            row.SetButton(2, spellDescriptor[2]);

        });
	}
	
	
}
    */