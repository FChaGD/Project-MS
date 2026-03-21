using FieldDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(TileSelector))]
public class MovementController : IManageSetterFunc, IMovementFuc
{


    //이동 관련 부가 컴포넌트 변수
    private MovementComponent movementComponent;
    private TileSelector tileSelector;
    private MovementStateSetter movementStateSetter;

    //외부 참조 요소
    private IGridFunc gridManager;
    private IUIFunc uiManager;
    private ISystemFunc systemManager;
    private ICameraFunc cameraController;

    //이동 관련 정보 클래스
    [SerializeField]
    private MovementInfo movementInfo;


    void Awake()
    {
        movementComponent = GetComponent<MovementComponent>();
        tileSelector = GetComponent<TileSelector>();
        movementStateSetter = GetComponent<MovementStateSetter>();

        movementInfo = new MovementInfo();

        movementComponent.Init(movementInfo);
        tileSelector.Init(movementInfo);
        movementStateSetter.Init(movementInfo);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {

        yield return new WaitForSeconds(0.2f);

        movementComponent.SetPlayertLocation(gridManager.GetCenter(new Vector2Int(0, 0)));


        tileSelector.GetTileVectorAtWorldPosition += GetTileVectorAtWorld;
        tileSelector.AddSelectedPathTile += AddSelectedPathTile;
        tileSelector.RemoveSelectedPathRangeTile += RemoveSelectedPathRangeTile;
        tileSelector.SetMinMax(gridManager.GetMaxMinXY());

        movementComponent.CameraUpdate += CameraUpdate;
        movementComponent.RemoveAtPathFunc += RemoveAtPathFunc;
        movementComponent.GetCenter += GetCenter;

    }

    //그리드매니저 받아오기
    public override void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    //매니저, 컨트롤러 받아오기
    public override void UISetter(IUIFunc ui)
    {
        uiManager = ui;
    }

    public override void SystemSetter(ISystemFunc system)
    {
        systemManager = system;
    }

    public override void CameraControlSetter(ICameraFunc cameracontrol)
    {
        cameraController = cameracontrol;
    }

    //Movement 이벤트 등록 및 제거
    public void AddMovementEvent(Action action)
    {
        movementComponent.Movement += action;
    }

    public void RemoveMovementEvent(Action action)
    {
        movementComponent.Movement -= action;
    }

    public void StopMovement()
    {
        movementComponent.StopMovement();
    }

    //셀 클릭 입력 = 경로 추가 및 제거
    public void AddMovePath(InputAction.CallbackContext context)
    {
        //중복실행 방지
        if (!context.performed) return;

        //이동중이거나 게임모드가 월드 상태가 아니라면 작동 X
        if (movementComponent.isMoving || systemManager.GetGameMode() != GameModeType.World)
        {
            return;
        }

        


        tileSelector.MovePathAddReMoveFunc();

    }

    //움직임 시작 입력
    public void MoveStart(InputAction.CallbackContext context)
    {
        //중복실행 방지
        if (!context.performed) return;

        movementComponent.MoveStart();

    }

    //하위 컴포넌트 이벤트 함수
    private Vector2Int GetTileVectorAtWorld(Vector3 worldPos)
    {
        return gridManager.GetTileVectorAtWorldPosition(worldPos);
    }

    private void AddSelectedPathTile(Vector2Int xy)
    {
        uiManager.AddSelectedPathTile(xy);
    }

    private void RemoveSelectedPathRangeTile(Vector2Int xy)
    {
        uiManager.RemoveSelectedPathRangeTile(xy);
    }

    private void CameraUpdate()
    {
        cameraController.CameraUpdate(); 
    }

    private void RemoveAtPathFunc(Vector2Int xy)
    {
        uiManager.RemoveSelectedPathTile(xy);

    }

    private Vector2 GetCenter(Vector2Int cellPosition)
    {
        return gridManager.GetCenter(cellPosition);
    }

}

    

   
   
