﻿using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {
    //CHARACTER INFORMATION
    public float Health = 100f;
    public float burnPercentage = .05f;

    protected float burnTickTime = .5f;
    protected float burnTime = 3f;
    protected float currentBurnTime;
    protected float currentTickTime;

    private string gameLayer;
    private LayerMask gameMask;
    private LayerMask dodgeMask;

    protected bool stunned;
    protected bool slowed;
    public bool confused;

    private bool burning;
    protected bool Burning
    {
        get
        {
            return burning;
        }
        set
        {
            burning = value;
            if (value == true)
            {
                setBurn();
            }
        }
    }

    private bool targetable = true;
    public bool Targetable
    {
        get
        {
            return targetable;
        }
        set
        {
            targetable = value;
            if (!targetable)
            {
                gameObject.layer = LayerMask.NameToLayer(gameLayer + " Dodge");
                gameObject.GetComponent<Controller2D>().collisionMask = dodgeMask;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer(gameLayer);         
                gameObject.GetComponent<Controller2D>().collisionMask = gameMask;
            }                
        }
    }
    

    private float test = 0;

    void setBurn()
    {
        GetComponent<ParticleSystem>().enableEmission = true;
        currentBurnTime = burnTime;
        currentTickTime = burnTickTime;
        test = 5;
    }

    public virtual void Start()
    {
        stunned = slowed = burning = confused = false;
        targetable = true;
        gameLayer = LayerMask.LayerToName(gameObject.layer);
        gameMask = gameObject.GetComponent<Controller2D>().collisionMask;
        dodgeMask = gameObject.GetComponent<Controller2D>().dodgeMask;
    }
    public virtual void Update ()
    {
        if (currentBurnTime > 0)
        {
            currentBurnTime -= Time.deltaTime;
            if (currentBurnTime <= 0)
            {
                currentBurnTime = 0;
                GetComponent<ParticleSystem>().enableEmission = false;
                GetComponent<ParticleSystem>().Clear();
                Burning = false;

            }
            if (currentTickTime >= 0)
            {
                currentTickTime -= Time.deltaTime;
            }
            else
            {
                currentTickTime = burnTickTime;
                TakeDamage(Health * burnPercentage);
            }
        }


	    
	}

    public virtual void TakeDamage(float damage)
    {
        if (targetable)
            Health -= damage;
    }

    public virtual void hitStun()
    {

    }
    public float speedCalculation(float speed)
    {

        float speedModifier = 1f;
        if (slowed)
        {
            speedModifier = .5f;
        }
        if (stunned)
        {
            speedModifier = 0;
        }

        if (confused)
        {
            speedModifier *= -1;
        }

        return speed * speedModifier;
    }
    


    

    public bool canMove()
    {
        return !stunned;
    }



    public void setStun(bool status, float time = 0)
    {
        stunned = status;
        if (stunned)
            StartCoroutine(stunCoroutine(time));
    }

    IEnumerator stunCoroutine(float time)
    {
        GetComponent<SimpleCharacterController>().isControllable = false;
        yield return new WaitForSeconds(time);
        stunned = false;
        GetComponent<SimpleCharacterController>().isControllable = true;
    }

    public void setSlow(bool status)
    {
        slowed = status;
    }

    public void setConfused (bool status)
    {
        confused = status;
    }
    

    public void setBurning (bool status)
    {
        Burning = status;
    }

    public void setInvincible (bool status)
    {
        Targetable = !status;
    }
}
