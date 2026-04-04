using FieldDefinition;
using ItemDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : IManageSetterFunc, IUIFunc
{
    //내부 UI 컴포넌트 변수
    private Dictionary<Type, BaseUIControl> uiControlList;
    private WorldUISetter worldUISetter;
    private CivilUISetter civilUISetter;
    private BattleUISetter battleUISetter;
    private DialogUISetter dialogUISetter;
    private SystemUISetter systemUISetter;
    private PauseUISetter pauseUISetter;




    //외부 컨트롤러 매니저 변수
    private IGridFunc gridManager;
    private IPartyFunc partyManager;
    private ISystemFunc systemManager;

    private IMovementFuc movementController;

    void Awake()
    {

        worldUISetter = GetComponent<WorldUISetter>();
        civilUISetter = GetComponent<CivilUISetter>();
        battleUISetter = GetComponent<BattleUISetter>();
        dialogUISetter = GetComponent<DialogUISetter>();
        systemUISetter = GetComponent<SystemUISetter>();
        pauseUISetter = GetComponent<PauseUISetter>();

        uiControlList = new Dictionary<Type, BaseUIControl>()
        {
            {typeof(WorldMode),  worldUISetter},
            {typeof(CivilMode),  civilUISetter},
            {typeof(BattleMode),  battleUISetter},
            {typeof(DialogMode),  dialogUISetter},
            {typeof(SystemMode),  systemUISetter},
            {typeof(PauseMode),  pauseUISetter}
        };
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitUntil(() => gridManager != null && movementController != null && partyManager != null);

        //WorldUI HUD 업데이트 함수 등록
        partyManager.UpdateSet(worldUISetter.UpdateFood, worldUISetter.UpdateGold);
        movementController.UpdateSet(worldUISetter.UpdateSpeed);

    }

    void OnDestroy()
    {
        //WorldUI HUD 업데이트 함수 해제
        partyManager.UnBindUpdateSet(worldUISetter.UpdateFood, worldUISetter.UpdateGold);
        movementController.UnBindUpdateSet(worldUISetter.UpdateSpeed);
    }

    #region 외부 매니저 컨트롤러
    //외부 매니저 컨트롤러 받아오기
    public override void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    public override void MoveControlSetter(IMovementFuc movecontrol)
    {
        movementController = movecontrol;
    }

    public override void PartySetter(IPartyFunc party)
    {
        partyManager = party;
    }

    public override void SystemSetter(ISystemFunc system)
    {
        systemManager = system;
    }
    #endregion

    //길 추가
    public void AddSelectedPathTile(Vector2Int xy)
    {
        worldUISetter.AddSelectedPathTile(xy);

    }

    //중간 제거
    public void RemoveSelectedPathRangeTile(Vector2Int xy)
    {
        worldUISetter.RemoveSelectedPathRangeTile(xy);
    }
    
    //특정 위치 제거
    public void RemoveSelectedPathTile(Vector2Int xy)
    {
        worldUISetter.RemoveSelectedPathTile(xy);
    }

    public void UIModeChange(bool OnOff, Type gamemode)
    {

    }

    //경로 분석 UI toggle 입력
    public void PathCheckUIToggle(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        worldUISetter.PathCheckUIToggle();


    }

    //소지품 SystemUI 전환 입력
    public void InventoryInput(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        SetTabInput<InventoryTabType>();


    }

    //파티 SystemUI 전환 입력
    public void PartyInput(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        SetTabInput<PartyTabType>();

    }

    //영웅 SystemUI 전환 입력
    public void HeroInput(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        SetTabInput<HeroTabType>();
    }

    private void SetTabInput<T>()
    {

        //만약 현재 게임 모드가 시스템 게임모드라면
        if (systemManager.GetGameMode() is SystemMode)
        {

            Debug.Log($"인벤토리 입력 시 탭 타입 : {systemManager.GetGameMode()}");
            Debug.Log($"인벤토리 입력 시 탭 타입 : {systemUISetter.GetCurTabType()}, {typeof(T).Name}, {systemUISetter.GetCurTabType() == typeof(T)}");
            //만약 현재 탭 포커스가 전환 대상 탭모드라면
            if (systemUISetter.GetCurTabType() == typeof(T))
            {
                systemUISetter.UIOff();
                systemManager.RemoveCurGameMode();
            }
            else
            {
                //만약 현재 탭 포커스가 다른 탭모드라면
                systemUISetter.TabFocus(typeof(T));
            }
        }
        else
        {
            //현재 게임 모드가 시스템 모드가 아니라면
            systemUISetter.TabFocus(typeof(T));
            systemUISetter.UIOn();
            systemManager.SetGameMode(typeof(SystemMode));
        }
    }



}
