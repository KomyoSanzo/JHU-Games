/**
An Animator controller that deals with attacks that are unable to be canceled by specific commands, such as dash.
Written by Willis Wang
*/

using UnityEngine;
using System.Collections;

public class fullChannelController : StateMachineBehaviour
{

    /**
     * Occurs when a state is being entered. using this to set our attack variables.
     * @param animator - the Animator being controlling the animation clips
     * @param stateInfo - information about the current animation state
     * @param layerIndex - the current layer of the animator
     */
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("inFullChannel", true);
    }


    /**
     * Occurs when a state is being exited. Using this to set our attack variables.
     * @param animator - the Animator being controlling the animation clips
     * @param stateInfo - information about the current animation state
     * @param layerIndex - the current layer of the animator
     */
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("inFullChannel", false);
    }


}
