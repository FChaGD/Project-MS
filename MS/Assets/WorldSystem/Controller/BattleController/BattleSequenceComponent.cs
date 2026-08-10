using BattleDefinition;
using UnityEngine;

/// <summary>
/// 배틀 이벤트를 처리하는 하위 컴포넌트 클래스
/// </summary>
public class BattleSequenceComponent : MonoBehaviour
{

    //배틀 컨트롤러 정보 컨테이너
    private BattleContainer _battleContainer;

    void Awake()
    {
        BattleEvent<BattleStart>.SubscribeMiddle(BattleStartSequence);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(BattleContainer battleContainer)
    {
        _battleContainer = battleContainer;
    }

    private void BattleStartSequence()
    {
#warning [배틀] 배틀 시작 캐릭터 행동 및 턴구성 로직 제작 필요
#warning [배틀] 배틀 시작 UI 활성화 로직 제작 필요

    }



}
