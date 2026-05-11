using BattleDefinition;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;

public class BattleController : IManageSetterFunc, IBattleFunc
{
    //배틀 컨트롤러에서 쓰이는 정보 컨테이너
    [Header("배틀 컨트롤 정보 컨테이너")]
    [SerializeField]
    private BattleContainer _battleContainer;

    //배경 및 캐릭터 제어 컴포넌트
    [Header("랜더링 제어 컴포넌트")]
    [SerializeField]
    private BattleFieldManager battleFieldManager;

    //하위 컴포넌트 변수
    private BattleTurnDeclarer battleTurnDeclarer;
    private BattleSequenceComponent battleSequenceComponent;

    void Awake()
    {
        //하위 컴포넌트 받아오기
        battleTurnDeclarer = GetComponent<BattleTurnDeclarer>();
        battleSequenceComponent = GetComponent<BattleSequenceComponent>();

        //컨테이너 등록
        battleTurnDeclarer.Init(_battleContainer);
        battleSequenceComponent.Init(_battleContainer);

    }

    void Start()
    {

    }

    /// <summary>
    /// 배틀 컨트롤 정보 컨테이너를 초기화하고, 관련 정보를 받아오는 함수
    /// </summary>
    public void BattleSetting()
    {
        //BattleReset();


    }

    private void BattleReset()
    {
        _battleContainer.round = 0;
        _battleContainer.heroList.Clear();

    }


}
