using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNormalState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SendMessage("OnButtonNormal");
    }
}
