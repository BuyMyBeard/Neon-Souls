using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentRootMotion : StateMachineBehaviour
{
    NavMeshAgent agent;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.isActiveAndEnabled)
        {
            agent.Move(animator.deltaPosition);
            agent.transform.rotation *= animator.deltaRotation;
        }
        else
            animator.ApplyBuiltinRootMotion();
    }
}
