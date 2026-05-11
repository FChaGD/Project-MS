using BattleDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SystemController : IManageSetterFunc, ISystemFunc
{
    //시스템 정보 컨테이너
    [SerializeField]
    private SystemInfoContainer systemInfoContainer;

    //외부 매니저 컨트롤러 변수
    private IInputFunc inputController;
    private IUIFunc uiController;
    private ICameraFunc cameraController;
    private IBattleFunc battleController;

    // 하위 컴포넌트
    private SystemModeChanger modeChanger;


    void Awake()
    {
        systemInfoContainer.gameMode = new Stack<Type>();
        systemInfoContainer.modeFactory = new Dictionary<Type, BaseGameMode>();

        StartCoroutine(ManageWaiter());

    }

    //매니지 매니저가 의존성 할당해줄 때까지 기다리는 코루틴
    private IEnumerator ManageWaiter()
    {
        yield return new WaitUntil(() => inputController != null && uiController != null && cameraController != null && battleController != null);

        //의존성 할당 이후 하위 컴포넌트 초기화
        modeChanger = GetComponent<SystemModeChanger>();
        if (modeChanger != null)
        {

            Debug.Log($"Awake inputController: {inputController != null}, uiController: {uiController != null}, cameraController: {cameraController != null}, battleController: {battleController != null}");
            modeChanger.Init(inputController, uiController, cameraController, battleController, systemInfoContainer);
        }
    }


    #region 외부 매니저 컨트롤러 받아오기

    public override void InputSetter(IInputFunc input)
    {
        Debug.Log($"InputSetter called with input: {input}");
        inputController = input;
    }

    public override void UISetter(IUIFunc ui)
    {
            Debug.Log($"UISetter called with ui: {ui}");
        uiController = ui;
    }

    public override void CameraControlSetter(ICameraFunc cameracontrol)
    {
            Debug.Log($"CameraControlSetter called with cameracontrol: {cameracontrol}");
        cameraController = cameracontrol;
    }

    public override void BattleControlSetter(IBattleFunc battlecontrol)
    {
        battleController = battlecontrol;
    }
    #endregion

    //게임모드 외부로 변환해주기
    public BaseGameMode GetGameMode()
    {
        return modeChanger.GetGameMode();
    }

    //게임모드 변경
    public void SetGameMode(Type request)
    {
        modeChanger.SetGameMode(request);
    }

    //현재 게임모드 종료
    public void RemoveCurGameMode()
    {
        modeChanger.RemoveCurGameMode();
    }


    //소지품 SystemUI 전환 입력
    public void InventoryInput(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        uiController.TabModeChange(typeof(InventoryTabType));
        // uiController의 SetTabInput에 시스템 모드 변경 내용 포함됨


    }

    //파티 SystemUI 전환 입력
    public void PartyInput(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        uiController.TabModeChange(typeof(PartyTabType));
        // uiController의 SetTabInput에 시스템 모드 변경 내용 포함됨

    }

    //영웅 SystemUI 전환 입력
    public void HeroInput(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        uiController.TabModeChange(typeof(HeroTabType));
        // uiController의 SetTabInput에 시스템 모드 변경 내용 포함됨
    }

}
