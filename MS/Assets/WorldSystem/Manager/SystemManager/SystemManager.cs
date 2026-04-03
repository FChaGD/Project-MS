using System;
using System.Collections;
using System.Collections.Generic;
using SystemDefinition;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;

public class SystemManager : IManageSetterFunc, ISystemFunc
{
    //게임 모드 리스트
    [SerializeField]
    private Stack<Type> gameMode;

    //외부 매니저 컨트롤러 변수
    private IInputFunc inputController;
    private IUIFunc uiController;
    private ICameraFunc cameraController;

    //모드 전환 딕셔너리
    private Dictionary<Type, BaseGameMode> modeFactory;


    void Awake()
    {
        gameMode = new Stack<Type>();

        

    }

    IEnumerator Start()
    {
        yield return new WaitUntil(()=> inputController != null && uiController != null && cameraController != null);

        //모드 맵핑 구성
        modeFactory = new Dictionary<Type, BaseGameMode>
        {
            { typeof(WorldMode),  new WorldMode(inputController, uiController, cameraController) },
            { typeof(BattleMode), new BattleMode(inputController, uiController, cameraController) },
            { typeof(CivilMode), new CivilMode(inputController, uiController, cameraController) },
            { typeof(SystemMode), new SystemMode(inputController, uiController, cameraController) },
            { typeof(DialogMode), new DialogMode(inputController, uiController, cameraController) },
            { typeof(PauseMode),  new PauseMode(inputController, uiController, cameraController) },
        };


#warning[시스템] 저장된 게임모드 불러오기 기능 추가 필요
        gameMode.Push(typeof(WorldMode));
        modeFactory[gameMode.Peek()].SetUp();

    }

    //외부 매니저 컨트롤러 받아오기

    public override void InputSetter(IInputFunc input)
    {
        inputController = input;
    }

    public override void UISetter(IUIFunc ui)
    {
        uiController = ui;
    }

    public override void CameraControlSetter(ICameraFunc cameracontrol)
    {
        cameraController = cameracontrol;
    }

    //게임모드 외부로 변환해주기
    public BaseGameMode GetGameMode()
    {
        return modeFactory[gameMode.Peek()];
    }

    //게임모드 변경
    public void SetGameMode(Type request)
    {
        BaseGameMode requestMode = modeFactory[request];

        if(gameMode.Peek() == request)
        {
            //이미 최상단이 해당 모드일 경우 종료
            modeFactory[gameMode.Pop()].EndSet();
            modeFactory[gameMode.Peek()].SetUp();
        }

        //변환 요청 모드가 타 모드 위에 얹기 가능한 모드일 경우
        if (requestMode is CoverableMode)
        {
            if (modeFactory[gameMode.Peek()] is EndPointMode)
            {
                //더이상 얹을 수 없을 경우
                return;
            }
            else
            {
                SetCoverableMode(request, requestMode);
            }
        }
        else
        {
            //요청한 모드가 기반 모드일 경우
            while (gameMode.Count > 0 && modeFactory[gameMode.Peek()] is not FloorMode)
            {
                modeFactory[gameMode.Pop()].EndSet();
            }
            if (gameMode.Count > 0)
                modeFactory[gameMode.Pop()].EndSet();

            gameMode.Push(requestMode.GetType());
            modeFactory[gameMode.Peek()].SetUp();
        }

    }

    //현재 게임모드 종료
    public void RemoveCurGameMode()
    {
        modeFactory[gameMode.Pop()].EndSet();
        modeFactory[gameMode.Peek()].SetUp();
    }


    private void SetCoverableMode(Type request, BaseGameMode requestMode)
    {

        //얹을 수 있을 경우
        if (gameMode.Contains(request))
        {
            //이미 스택 내에 있을때
            Stack<Type> tmpStack = new Stack<Type>();
            while (gameMode.Peek() != request)
            {
                tmpStack.Push(gameMode.Pop());
            }
            gameMode.Pop();
            while (tmpStack.Count > 0)
            {
                BaseGameMode tmp = modeFactory[tmpStack.Pop()];
                gameMode.Push(tmp.GetType());
            }
            gameMode.Push(request);
            requestMode.SetUp();

        }
        else
        {
            //스택 내에 없을때
            modeFactory[gameMode.Peek()].EndSet();
            gameMode.Push(request);
            requestMode.SetUp();
        }
    }
}
