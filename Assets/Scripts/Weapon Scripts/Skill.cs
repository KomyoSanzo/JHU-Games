﻿/**
Base script for all skills
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
    //Skill defining features
    public Texture2D icon;
    public string skillName;
    public float cooldown;

    public Transform hitbox;

    //Required Components from Players
    [HideInInspector] public Animator animator;
    [HideInInspector] public SimpleCharacterController playerInformation;

    //Obtain components from player
    public virtual void Start ()
    {
        animator = GetComponentInParent<Animator>();
        playerInformation = GetComponentInParent<SimpleCharacterController>();
	}
	
	
    //VIRTUAL FUNCTIONS SO THAT THE WEAPONS CONTROLLER CAN CALL THEM
    public virtual void Activate()
    {
        print("activating: " + skillName);
    }

    public virtual void endChannel()
    {
        print("ability fully channeled");
    }
}
