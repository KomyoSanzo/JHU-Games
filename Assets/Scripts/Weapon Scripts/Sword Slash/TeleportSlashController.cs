/**
The Sword Strike ability controller
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class TeleportSlashController : Skill
{
    //The hitbox to be instantiated
    public Transform target = null;
    

    public override void Activate()
    {
        //Checks if an animator is obtained
        if (animator == null)
            animator = GetComponentInParent<Animator>();

        //Triggers the ability in the animator
        base.Activate();
        animator.SetTrigger("SwordSlash");


    }

    public override void endChannel()
    {
        base.endChannel();


        if (checkTarget())
        {
            teleportToPos(target);
        }
        else
        {
            playerInformation.gameObject.GetComponent<PlayerScript>().controller.Move(Vector3.right * ((playerInformation.facingRight) ? 5 : -5));
        }



        //Instantiates the hitbox and modifies it to the players location
        Transform newHitBox = Instantiate(hitbox.transform) as Transform;

        SwordHitBoxController swordHitBox = newHitBox.gameObject.GetComponent<SwordHitBoxController>();
        swordHitBox.ownerTag = this.transform.parent.gameObject.tag;
        swordHitBox.transform.position = playerInformation.transform.position;
        swordHitBox.playerInformation = playerInformation;

        //Checks the player location for directional instantiation. 
        if (playerInformation.facingRight)
        {
            Vector3 playerScale = newHitBox.transform.localScale;
            playerScale.x = playerScale.x * -1;
            newHitBox.localScale = playerScale;
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            if (target == null || Vector2.Distance(other.gameObject.transform.position, this.gameObject.GetComponentInParent<Transform>().position) < 
                Vector2.Distance(target.position, this.gameObject.GetComponentInParent<Transform>().position))
            {
                target = other.gameObject.transform;
                print(target.gameObject.tag);
            }
        }
    }

    bool checkTarget()
    {
        if (target == null)
            return false;
        else
        {
            float difference;
            if (playerInformation.facingRight)
                difference = target.position.x - transform.parent.transform.position.x;
            else
                difference = transform.parent.transform.position.x - target.position.x;
                    
            print(target.position);
            print(transform.parent.transform.position);
            print(difference);
            print(Mathf.Abs(target.position.y - transform.parent.transform.position.y));

            if (difference < 10 && difference >= 0)
            {
                float difference2 = target.position.y - transform.parent.transform.position.y;
                return (difference2 < .75*difference && difference2 >= 0);
            }
            else
                return false;



            if (Mathf.Abs(target.position.y - this.gameObject.GetComponentInParent<Transform>().position.y) < 5)
            {
                return (Mathf.Abs(target.position.x - this.gameObject.GetComponentInParent<Transform>().position.x) < 10);
            }
            else
                return false;
        }
    }

    private bool teleportToPos(Transform pos)
    {
        if (target == null)
            return false;
        else
        {
            if (playerInformation.facingRight)
            {
                playerInformation.gameObject.transform.position = new Vector3(target.position.x - 2, target.position.y);

            }
            else
            {
                playerInformation.gameObject.transform.position = new Vector3(target.position.x + 2, target.position.y);

            }
        }
        return true;
    }





}
