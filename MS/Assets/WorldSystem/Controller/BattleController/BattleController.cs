using BattleDefinition;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;

public class BattleController : IManageSetterFunc, IBattleFunc
{
    //배틀 컨트롤러에서 쓰이는 정보 컨테이너
    [SerializeField]
    private BattleContainer _battleContainer;

    //하위 컴포넌트 변수
    private BattleTurnDeclarer battleTurnDeclarer;
    private BattleTriggerGenerator battleTriggerGenerator;

    void Awake()
    {
        //하위 컴포넌트 받아오기
        battleTurnDeclarer = GetComponent<BattleTurnDeclarer>();
        battleTriggerGenerator = GetComponent<BattleTriggerGenerator>();

        //컨테이너 등록
        battleTurnDeclarer.Init(_battleContainer);
        battleTriggerGenerator.Init(_battleContainer);



    }

    void Start()
    {
        
    }

}
