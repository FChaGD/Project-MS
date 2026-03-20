using FieldDefinition;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelector : MonoBehaviour
{
    //타일값 받아오기
    public Func<Vector3, Vector2Int> GetTileVectorAtWorldPosition;
    //ui 매니저 요청
    public Action<Vector2Int> AddSelectedPathTile;
    public Action<Vector2Int> RemoveSelectedPathRangeTile;

    //타일 선택시 위치를 확인할 카메라
    private Camera cameraOfPlayer;

    //이동 관련 정보 클래스
    private MovementInfo _movementInfo;

    public void Init(MovementInfo movementInfo)
    {
        _movementInfo = movementInfo;
    }

    void Start()
    {
        cameraOfPlayer = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _movementInfo.movePath = new List<Vector2Int>();

    }

    //선택 타일 경로 추가 제거 함수
    public void MovePathAddReMoveFunc()
    {

        //마우스 커서 위치 받아오기
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cameraOfPlayer.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;


        //선택한 타일
        Vector2Int targetTile = GetTileVectorAtWorldPosition(worldPos);

        //플레이어가 위치한 곳의 타일
        Vector2Int presentTile = GetTileVectorAtWorldPosition(transform.position);


        if (presentTile == targetTile)
        {
            //만약 플레이어 캐릭터가 위치한 타일을 선택했다면
            return;
        }


        if (_movementInfo.movePath.Count == 0)
        {
            //리스트가 비어있을 경우
            AddPathFunc(presentTile, targetTile);
            return;
        }

        //선택한 타일이 이미 리스트에 존재하는가?
        if (_movementInfo.movePath.Contains(targetTile))
        {
            //타일 제거(중간부터)
            RemoveMiddlePathFunc(targetTile);
        }
        else
        {
#warning [플레이어 이동] A*알고리즘 구현 필요 
            AddPathFunc(_movementInfo.movePath[_movementInfo.movePath.Count - 1], targetTile);
        }

    }



    //경로 추가 함수
    private void AddPathFunc(Vector2Int presentTile, Vector2Int targetTile)
    {
        if (Mathf.Abs(presentTile.x - targetTile.x) <= 1 && Mathf.Abs(presentTile.y - targetTile.y) <= 1)
        {
            //플레이어가 마지막으로 선택한 경로의 -1 ~ 1 사이의 타일인지
            _movementInfo.movePath.Add(targetTile);
            AddSelectedPathTile(targetTile);
            Debug.Log($"현재 타일 : {_movementInfo.movePath}");
            return;
        }
        else
        {
            //리스트가 비어있고 너무 먼 타일을 선택했을 경우
            return;
        }
    }

    //중간 경로 없애기
    private void RemoveMiddlePathFunc(Vector2Int targetTile)
    {
        _movementInfo.movePath.RemoveRange(_movementInfo.movePath.IndexOf(targetTile), _movementInfo.movePath.Count - _movementInfo.movePath.IndexOf(targetTile));
        RemoveSelectedPathRangeTile(targetTile);
    }


}
