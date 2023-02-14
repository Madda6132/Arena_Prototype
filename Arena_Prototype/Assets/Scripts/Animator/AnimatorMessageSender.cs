using UnityEngine;
using RPG.Creatures;

public class AnimatorMessageSender : StateMachineBehaviour
{

    [SerializeField] AnimatorMessageInfo[] messageInfo;
    private enum StateType {
        ENTER,
        EXIT
    }


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (messageInfo.Length == 0) return;

        if(animator.TryGetComponent(out Creature creature)) {

            foreach (var info in messageInfo) {

                if(info.StateType == StateType.ENTER)
                    creature.AnimationMessageReciver(layerIndex, info.Message.ToString());
            }
            
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (messageInfo.Length == 0) return;

        if (animator.TryGetComponent(out Creature creature)) {

            foreach (var info in messageInfo) {

                if (info.StateType == StateType.EXIT)
                    creature.AnimationMessageReciver(layerIndex, info.Message.ToString());
            }
        }
    }

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


    [System.Serializable]
    private struct AnimatorMessageInfo {

        [SerializeField] StateType _StateType;
        [SerializeField] UtilityAnimations.AnimatorMessageType _Message;

        public AnimatorMessageInfo(StateType stateType, UtilityAnimations.AnimatorMessageType message) {

            this._StateType = stateType;
            this._Message = message;
        }

        public StateType StateType => _StateType;
        public UtilityAnimations.AnimatorMessageType Message => _Message;
    }
}
