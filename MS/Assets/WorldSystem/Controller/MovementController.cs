using FieldDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : IManageSetterFunc, IMovementFuc
{
    private Camera cameraOfPlayer;

    //플레이어 이동 관련
    public float MoveSecound = 8f;
    private List<Vector2Int> movePath;
    private Transform playerTransform;
    private float moveSpeed = 0.125f;
    private bool isMoving = false;

    //외부 참조 요소
    private IGridFunc gridManager;
    private IUIFunc uiManager;
    private ISystemFunc systemManager;

    //외부 클래스 이벤트 발생용
    public Action Movement = () => {};


    void Awake()
    {
        moveSpeed = 1 / MoveSecound;
        movePath = new List<Vector2Int>();
        playerTransform = gameObject.GetComponent<Transform>();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraOfPlayer = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        playerTransform.Translate(gridManager.GetCenter(new Vector2Int(0, 0)));
    }

    //그리드매니저 받아오기
    public override void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    //UI매니저 받아오기
    public override void UISetter(IUIFunc ui)
    {
        uiManager = ui;
    }

    public override void SystemSetter(ISystemFunc system)
    {
        systemManager = system;
    }

    public void AddMovementEvent(Action action)
    {
        Movement += action;
    }
    
    public void RemoveMovementEvent(Action action)
    {
        Movement -= action;
    }


    private Vector2 GetPlayerPosition()
    {
        return transform.position;
    }

    //셀 클릭했을 때, 경로 추가
    public void AddMovePath(InputAction.CallbackContext context)
    {
        //중복실행 방지
        if (!context.performed) return;

        //이동중이거나 게임모드가 월드 상태가 아니라면 작동 X
        if (isMoving || systemManager.GetGameMode() != GameModeType.World)
        {
            return;
        }

        //마우스 커서 위치 받아오기
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cameraOfPlayer.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;

        Vector2Int targetTile = gridManager.GetTileVectorAtWorldPosition(worldPos);
        
        //플레이어가 위치한 곳의 타일을 선택한 경우
        Vector3 presentPosition = GetPlayerPosition();
        Vector2Int presentTile = gridManager.GetTileVectorAtWorldPosition(presentPosition);
        if (presentTile == targetTile)
        {
            Debug.Log($"플레이어 캐릭터가 위치한 타일 선택 : {targetTile}");
            return;
        }


        if (movePath.Count == 0)
        {
            //리스트가 비어있을 경우
            AddPathFunc(presentTile, targetTile);
            return;
        }

        //선택한 타일이 이미 리스트에 존재하는가?
        if (movePath.Contains(targetTile))
        {
            //타일 제거(중간부터)
            RemoveMiddlePathFunc(targetTile);
        }
        else
        {
            AddPathFunc(movePath[movePath.Count - 1], targetTile);
        }
    }

    //움직임 시작
    public void MoveStart(InputAction.CallbackContext context)
    {
        //중복실행 방지
        if (!context.performed) return;

        //경로가 비어있다면
        if (movePath.Count == 0)
        {
            return;
        }

        if (!isMoving)
        {
            StartCoroutine(nameof(MoveAlongPath));
        }

    }

    //타일 선택했을 때 경로 추가 함수
    private void AddPathFunc(Vector2Int presentTile, Vector2Int targetTile)
    {
        if (Mathf.Abs(presentTile.x - targetTile.x) <= 1 && Mathf.Abs(presentTile.y - targetTile.y) <= 1)
        {
            //플레이어가 마지막으로 선택한 경로의 -1 ~ 1 사이의 타일인지

            Debug.Log($"타일 추가 : {presentTile}, {targetTile}");
            Debug.Log($"타일 추가 : {presentTile.x - targetTile.x}, {presentTile.y - targetTile.y}");
            movePath.Add(targetTile);
            uiManager.AddSelectedPathTile(targetTile);
            Debug.Log($"현재 타일 : {movePath}");
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
        movePath.RemoveRange(movePath.IndexOf(targetTile), movePath.Count - movePath.IndexOf(targetTile));
        uiManager.RemoveSelectedPathRangeTile(targetTile);
    }

    //경로의 한 부분만 없애기
    private void RemoveAtPathFunc(Vector2Int targetTile)
    {
        movePath.Remove(targetTile);
        uiManager.RemoveSelectedPathTile(targetTile);
    }

    //경로 이동하기
    private IEnumerator MoveAlongPath()
    {
        isMoving = true;
        int currentIndex = 0;
        int count = movePath.Count;
        while (currentIndex < count)
        {
            Debug.Log($"몇번째 이동인가? {currentIndex}");
            Vector2 target = gridManager.GetCenter(movePath[0]);
            yield return StartCoroutine(MoveTo(target));
            RemoveAtPathFunc(movePath[0]);
            currentIndex++;

        }
        isMoving = false;

    }
    
    //현재 위치에서 다음 좌표로 이동
    private IEnumerator MoveTo(Vector2 target)
    {
        Vector2 start = transform.position;

        float distance = Vector2.Distance(start, target);
        float duration = distance / moveSpeed;

        float elapsed = 0f;
        int sec = 1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector2.Lerp(start, target, t);

            //1초마다 이동 이벤트 발생
            if (elapsed >= sec)
            {
                sec++;
                Movement();
            }

            yield return null;
        }

        transform.position = target;
    }
}
