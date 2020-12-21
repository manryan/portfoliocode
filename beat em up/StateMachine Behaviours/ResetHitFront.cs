﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHitFront : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("HitFront", false);
        animator.SetBool("Block", false);
        animator.SetBool("Blocked", false);
        animator.SetBool("HitFront", false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
 //   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
 //   {


  

        //  if (!animator.GetBool("HitBehind") && animator.GetInteger("hitStack") < 3)
        //  {
        //    animator.transform.root.GetComponent<Entity>().setBlock();
        //          if (animator.transform.root.GetComponent<Enemy>() != null)
        //            animator.transform.root.GetComponent<Enemy>().state = enemyState.approach;
        //   }
        //  animator.SetBool("GotHit", false);

  //  }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}