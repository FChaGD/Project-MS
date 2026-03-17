using FieldDefinition;
using System;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;

public class EncounterManager : IManageSetterFunc, IEncounterFunc
{
    //인카운터 정보
    [Header("인카운터 정보")]
    [SerializeField]
    private float positiveEncounterP;
    [SerializeField]
    private float negativeEncounterP;
    private List<Action> positiveEncounterList;
    private List<Action> negativeEncounterList;

    //외부 시스템 요소
    private ISystemFunc systemManager;
    private IMovementFuc movementController;


    private void Awake()
    {
        positiveEncounterList = new List<Action>();
        negativeEncounterList = new List<Action>();
    }

    void Start()
    {
        movementController.AddMovementEvent(EncounterActivation);
    }

    private void OnDestroy()
    {
        movementController.RemoveMovementEvent(EncounterActivation);
    }


    public override void SystemSetter(ISystemFunc system)
    {
        systemManager = system;
    }

    public override void MoveControlSetter(IMovementFuc movecontrol)
    {
        movementController = movecontrol;
    }


    private void EncounterActivation()
    {
        //고정 인카운터 요소 실행

#warning [미완성] 이 부분 나중에 로직 추가 필요(고정 인카운터 요소)


        //랜덤 인카운터 요소 실행
        float p = UnityEngine.Random.Range(0f, 100f);
        Debug.Log($"인카운터 확률 : {p}");
        if (p <= negativeEncounterP)
        {
            //부정적 인카운터 발생시
            movementController.StopMovement();
            Debug.Log("인카운터 테스팅 : 부정적 인카운트 발생");
            AddNegativeEncounter();

        } else if (p > negativeEncounterP && p <= negativeEncounterP + positiveEncounterP)
        {
            //긍정적 인카운터 발생시
            movementController.StopMovement();
            Debug.Log("인카운터 테스팅 : 긍정적 인카운트 발생");
            AddPositiveEncounter();
        }

    }

    private void AddPositiveEncounter()
    {
        positiveEncounterList.Clear();

#warning [미완성] 이 부분 나중에 로직 추가 필요(긍정 인카운터 추가 로직)

        //타일의 긍정적 이벤트 받아오기
        //동료의 긍정적 이벤트 받아오기
        //국가의 긍정적 이벤트 받아오기
        //월드의 긍정적 이벤트 받아오기
    }

    private void AddNegativeEncounter()
    {
        negativeEncounterList.Clear();

#warning [미완성] 이 부분 나중에 로직 추가 필요(부정 인카운터 추가 로직)

        //타일의 부정적 이벤트 받아오기
        //동료의 부정적 이벤트 받아오기
        //국가의 부정적 이벤트 받아오기
        //월드의 부정적 이벤트 받아오기



    }



}
