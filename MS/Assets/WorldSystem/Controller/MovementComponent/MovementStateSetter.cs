using FieldDefinition;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateSetter : MonoBehaviour
{
    //속도 변경시 실행 함수
    public Action<float> SpeedUpdate;

    //플레이어 이동 변수
    [SerializeField]
    private float moveSecound = 8f;
    private float moveSpeed = 0.125f;

    //이동 관련 정보 클래스
    private MovementInfo _movementInfo;

    private void Awake()
    {
        moveSpeed = 1 / moveSecound;

    }
    public void Init(MovementInfo movementInfo)
    {
        _movementInfo = movementInfo;
        _movementInfo.MoveSecound = moveSecound;
        _movementInfo.moveSpeed = moveSpeed;
        SpeedUpdate?.Invoke(moveSpeed);
    }

    void Start()
    {
        _movementInfo.MoveSecound = moveSecound;
        _movementInfo.moveSpeed = moveSpeed;
        _movementInfo.movementType = MovementType.Walk;
    }
#warning [플레이어 이동] 플레이어 이동 속도 조절 기능, 플레이어 항해 및 비행 구현 필요

}
