using UnityEngine;
using System.Collections;

public class UppercutController : Skill{

    PlayerScript playerInformation2;
    AudioSource audioPlayer;

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

	public override void Activate()
    {
        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }
        base.Activate();
        animator.SetTrigger("uppercut");

        if (playerInformation2 == null)
            playerInformation2 = GetComponentInParent<PlayerScript>();

        playerInformation2.controller.collisions.below = false;
        StartCoroutine(moveUp());
        audioPlayer.Play();
    }

    IEnumerator moveUp()
    {
        playerInformation2.isControllable = false;
        if (playerInformation.facingRight)
            playerInformation2.velocity = new Vector2(1f, 10f);
        else
            playerInformation2.velocity = new Vector2(-1f, 10f);
        yield return new WaitForSeconds(.4f);
        playerInformation2.isControllable = true;
    }
}
