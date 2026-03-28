using System.Text;
using System;
using UnityEngine;

public class WorldUISetter : MonoBehaviour
{
    //UI 객체들
    [SerializeField]
    private GameObject worldUIPanel;
    [SerializeField]
    private GameObject partyInfoPanel;
    [SerializeField]
    private GameObject pathCheckUI;

    //파티 정보 제어 Setter들
    private FoodSetter foodSetter;
    private SpeedSetter speedSetter;
    private GoldSetter goldSetter;


    //경로 검사 관련 UI 변수들
    private Animator pathCheckUIAnmation;
    private int pathCheckUICurState = Animator.StringToHash("CurState");
    private int pathCheckUIIsChanging = Animator.StringToHash("isChanging");
    private int openTrigger = Animator.StringToHash("OpenTrigger");
    private int closeTrigger = Animator.StringToHash("CloseTrigger");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathCheckUIAnmation = pathCheckUI.GetComponent<Animator>();

        foodSetter = partyInfoPanel.GetComponentInChildren<FoodSetter>();
        speedSetter = partyInfoPanel.GetComponentInChildren<SpeedSetter>();
        goldSetter = partyInfoPanel.GetComponentInChildren<GoldSetter>();


    }


    //경로 검사 토글
    public void PathCheckUIToggle()
    {

        if (pathCheckUIAnmation.GetBool(pathCheckUICurState))
        {
            //만약 애니메이션이 진행중이라면 종료
            if (pathCheckUIAnmation.GetBool(pathCheckUIIsChanging))
            {
                return;
            }

            //open 상태라면 닫는 트리거 발동
            pathCheckUIAnmation.SetTrigger(closeTrigger);
        }
        else
        {
            //만약 애니메이션이 진행중이라면 종료
            if (pathCheckUIAnmation.GetBool(pathCheckUIIsChanging))
            {
                return;
            }

            //close 상태라면 여는 트리거 발동
            pathCheckUIAnmation.SetTrigger(openTrigger);
        }
    }
    
    public void UpdateFood(int food, int foodmax)
    {
        StringBuilder updateText = new StringBuilder(food.ToString());
        updateText.Append(" / ");
        updateText.Append(foodmax.ToString());
        foodSetter.TextSet(updateText.ToString());
    }

    public void UpdateSpeed(float speed)
    {

        string updateText = Math.Round(speed, 2).ToString();
        speedSetter.TextSet(updateText);
    }

    public void UpdateGold(int gold)
    {
        string updateText = gold.ToString();
        goldSetter.TextSet(updateText);
    }

}
