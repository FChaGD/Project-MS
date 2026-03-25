using UnityEngine;

public class AnimationPramSetter : StateMachineBehaviour
{
    //애니메이터 변수 해싱
    private int CurState = Animator.StringToHash("CurState");
    private int isChanging = Animator.StringToHash("isChanging");

    private static readonly int OpenedHash = Animator.StringToHash("PathCheckOpened");
    private static readonly int OpeningHash = Animator.StringToHash("PathCheckOpening");
    private static readonly int ClosedHash = Animator.StringToHash("PathCheckClosed");
    private static readonly int ClosingHash = Animator.StringToHash("PathCheckClosing");

    //상태 진입 시 1회 호출
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // shortNameHash: "Base Layer.PathCheckOpened" 가 아닌 "PathCheckOpened" 기준
        switch (stateInfo.shortNameHash)
        {
            case var h when h == OpenedHash:
                animator.SetBool(isChanging, false);
                break;

            case var h when h == OpeningHash:
                animator.SetBool(CurState, true);
                animator.SetBool(isChanging, true);
                break;

            case var h when h == ClosedHash:
                animator.SetBool(isChanging, false);
                break;

            case var h when h == ClosingHash:
                animator.SetBool(CurState, false);
                animator.SetBool(isChanging, true);
                break;
        }
    }

}
