using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorLaunchEnd : StateMachineBehaviour
{
    bool messageSent = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        messageSent = false;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!messageSent && stateInfo.normalizedTime >= 1)
        {
            animator.SendMessage("ReachSky");         
            messageSent = true;
        }
    }
}
