using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeteorAttackRootMotion : StateMachineBehaviour
{
    NavMeshAgent agent;
    MeteorAttack meteorAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        meteorAttack = animator.GetComponent<MeteorAttack>();
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (meteorAttack.SkipThisFrameRootMotion)
            return;
        if (agent.isActiveAndEnabled)
            agent.Move(animator.deltaPosition);
        else
            animator.ApplyBuiltinRootMotion();
    }
}
