using BattleDefinition;
using SystemDefinition;
using UnityEngine;

/// <summary>
/// 전투 환경 및 캐릭터를 세팅하는 하위 컴포넌트 클래스
/// </summary>
public class BattleFieldSetter : MonoBehaviour
{
    //배틀 컨트롤러 정보 컨테이너
    private BattleContainer _battleContainer;

    //테스트용 변수
    public bool isTesting;

    void Awake()
    {
        SystemEvent.Subscribe<BattleMode>(BattleSetting);
    }

    void Start()
    {
        
    }
    void OnDestroy()
    {
        SystemEvent.Unsubscribe<BattleMode>(BattleSetting);
    }

    public void Init(BattleContainer battleContainer)
    {
        _battleContainer = battleContainer;
    }



    /// <summary>
    /// 배틀 컨트롤 정보 컨테이너를 초기화하고, 관련 정보를 받아오는 함수
    /// </summary>
    public void BattleSetting()
    {

        if (!isTesting)
        {
            BattleReset();
        }

        LoadBattleInfo();
        SetCharacterPositon();
        BattleEvent<BattleStart>.Publish();


    }

    private void BattleReset()
    {
        _battleContainer.round = 0;
        _battleContainer.heroList.Clear();

    }

    private void LoadBattleInfo()
    {
#warning [배틀] 배틀 정보 불러오는 기능 추가 필요
    }

    private void SetCharacterPositon()
    {
        _battleContainer.battleFieldManager.SetBattleField(_battleContainer.heroList.ToArray(), _battleContainer.enemyList.ToArray());
    }

}
