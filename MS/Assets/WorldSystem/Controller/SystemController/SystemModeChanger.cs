using BattleDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;

public class SystemModeChanger : MonoBehaviour
{
    #region 변수 생성 구역

    //시스템 정보 컨테이너
    private SystemInfoContainer systemInfoContainer;

    //외부 매니저 컨트롤러 변수
    private IInputFunc inputController;
    private IUIFunc uiController;
    private ICameraFunc cameraController;
    private IBattleFunc battleController;


    #endregion

    #region 유니티 라이프사이클 관련

    void Awake()
    {
        
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => inputController != null && uiController != null && cameraController != null && battleController != null);

        //모드 맵핑 구성
        systemInfoContainer.modeFactory = new Dictionary<Type, BaseGameMode>
        {
            { typeof(WorldMode),  new WorldMode(inputController, uiController, cameraController, battleController) },
            { typeof(BattleMode), new BattleMode(inputController, uiController, cameraController, battleController) },
            { typeof(CivilMode), new CivilMode(inputController, uiController, cameraController, battleController) },
            { typeof(SystemMode), new SystemMode(inputController, uiController, cameraController, battleController) },
            { typeof(DialogMode), new DialogMode(inputController, uiController, cameraController, battleController) },
            { typeof(PauseMode),  new PauseMode(inputController, uiController, cameraController, battleController) },
        };


#warning[시스템] 저장된 게임모드 불러오기 기능 추가 필요
        systemInfoContainer.gameMode.Push(typeof(WorldMode));
        systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()].SetUp();


    }

    public void Init(IInputFunc input, IUIFunc ui, ICameraFunc camera, IBattleFunc battle, SystemInfoContainer container)
    {
        inputController = input;
        uiController = ui;
        cameraController = camera;
        systemInfoContainer = container;
        battleController = battle;
        Debug.Log("초기화 성공");
    }

    #endregion

    #region public 생성 구역

    //게임모드 외부로 변환해주기
    public BaseGameMode GetGameMode()
    {
        return systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()];
    }

    //게임모드 변경
    public void SetGameMode(Type request)
    {
        BaseGameMode requestMode = systemInfoContainer.modeFactory[request];

        if (systemInfoContainer.gameMode.Peek() == request)
        {
            //이미 최상단이 해당 모드일 경우 종료
            systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Pop()].EndSet();
            systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()].SetUp();
        }

        //변환 요청 모드가 타 모드 위에 얹기 가능한 모드일 경우
        if (requestMode is CoverableMode)
        {
            if (systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()] is EndPointMode)
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
            while (systemInfoContainer.gameMode.Count > 0 && systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()] is not FloorMode)
            {
                systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Pop()].EndSet();
            }
            if (systemInfoContainer.gameMode.Count > 0)
                systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Pop()].EndSet();

            systemInfoContainer.gameMode.Push(requestMode.GetType());
            systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()].SetUp();
        }

    }

    //현재 게임모드 종료
    public void RemoveCurGameMode()
    {
        systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Pop()].EndSet();
        systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()].SetUp();
    }


    #endregion

    #region private 생성 구역

    private void SetCoverableMode(Type request, BaseGameMode requestMode)
    {

        //얹을 수 있을 경우
        if (systemInfoContainer.gameMode.Contains(request))
        {
            //이미 스택 내에 있을때
            Stack<Type> tmpStack = new Stack<Type>();
            while (systemInfoContainer.gameMode.Peek() != request)
            {
                tmpStack.Push(systemInfoContainer.gameMode.Pop());
            }
            systemInfoContainer.gameMode.Pop();
            while (tmpStack.Count > 0)
            {
                BaseGameMode tmp = systemInfoContainer.modeFactory[tmpStack.Pop()];
                systemInfoContainer.gameMode.Push(tmp.GetType());
            }
            systemInfoContainer.gameMode.Push(request);
            requestMode.SetUp();

        }
        else
        {
            //스택 내에 없을때
            systemInfoContainer.modeFactory[systemInfoContainer.gameMode.Peek()].EndSet();
            systemInfoContainer.gameMode.Push(request);
            requestMode.SetUp();
        }
    }
    #endregion

}
