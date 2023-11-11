using UnityEngine;

public class BoxBehaviour : StateMachineBehaviour{
    private int _randomNumber;

    [SerializeField] int numberOfAnimations;
    private static readonly int BoxingAnimation = Animator.StringToHash("BoxingAnimation");

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        if (stateInfo.normalizedTime % 1 > 0.98f){
            _randomNumber = Random.Range(0, numberOfAnimations);
        }

        animator.SetFloat(BoxingAnimation, _randomNumber, 0.2f, Time.deltaTime);
    }
}