using BattleDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BattleSequenceComponent : MonoBehaviour
{

    //배틀 컨트롤러 정보 컨테이너
    private BattleContainer _battleContainer;

    //트리거 목록
    private Dictionary<Type, BattleTriggerEventBase> triggerList = new();

    void Awake()
    {
        //딕셔너리 자동화 등록
        var baseType = typeof(BattleTriggerEventBase);
        var types = Assembly.GetExecutingAssembly()
                            .GetTypes()
                            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));

        foreach (var type in types)
        {
            triggerList[type] = (BattleTriggerEventBase)Activator.CreateInstance(type);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(BattleContainer battleContainer)
    {
        _battleContainer = battleContainer;
    }

    /// <summary>
    /// 트리거 발생 함수
    /// 원하는 트리거 종류를 클래스 타입으로 호출
    /// </summary>
    /// <param name="type">BattleTriggerEventBase 계열의 Type</param>
    public void callTrigger(Type type)
    {
        triggerList[type]?.TriggerGenerate();
    }

    /// <summary>
    /// 트리거 추가 함수
    /// 트리거의 종류를 클래스 타입과 추가하려는 Action으로 추가
    /// </summary>
    /// <param name="type">>BattleTriggerEventBase 계열의 Type</param>
    /// <param name="addtrigger">추가하려는 Action 함수 Delegate</param>
    public void AddTrigger(Type type, Action addtrigger)
    {
        triggerList[type]?.AddTrigger(addtrigger);
    }

    /// <summary>
    /// 트리거 제거 함수
    /// 트리거의 종류를 클래스 타입과 제거하려는 Action으로 추가
    /// </summary>
    /// <param name="type">BattleTriggerEventBase 계열의 Type</param>
    /// <param name="removetrigger">제거하려는 Action 함수 Delegate</param>

    public void RemoveTrigger(Type type, Action removetrigger)
    {
        triggerList[type]?.RemoveTrigger(removetrigger);
    }

}
