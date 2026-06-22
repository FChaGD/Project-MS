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

    //게임 모드 별 액션 맵 활성화/비활성화 맵핑
    private Dictionary<Type, Action> onActionUI = new();


    //외부 컨트롤러 매니저 변수
    private IGridFunc gridManager;
    private IPartyFunc partyManager;
    private ISystemFunc systemController;

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

        SetModeUI();
        
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

        if (!Application.isPlaying) return;
        else
        {
            foreach (Type mode in systemController.GetModeList())
            {
                if (onActionUI.TryGetValue(mode, out Action action))
                {
                    SystemEvent.Unsubscribe(mode, action);
                }
            }
        }

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
        systemController = system;
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

    /// <summary>
    /// UI를 끄고 키는 public 함수
    /// bool을 통해 UI를 활성화할지 비활성화 할지 결정.
    /// Type을 통해 제어할 UI 종류를 결정
    /// </summary>
    /// <param name="OnOff">True == UI 켜기, False == UI 끄기</param>
    /// <param name="gamemode">GameModeBase 기반 클래스 타입</param>
    public void UIModeChange(bool OnOff, Type gamemode)
    {
        if (OnOff) {
            uiControlList[gamemode].UIOn();
        }
        else
        {
            uiControlList[gamemode].UIOff();
        }
            
    }

    //경로 분석 UI toggle 입력
    public void PathCheckUIToggle(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        worldUISetter.PathCheckUIToggle();


    }

    public void TabModeChange(Type tabType)
    {
        switch (tabType)
        {
            case var t when t == typeof(InventoryTabType): SetTabInput<InventoryTabType>(); break;
            case var t when t == typeof(PartyTabType): SetTabInput<PartyTabType>(); break;
            case var t when t == typeof(HeroTabType): SetTabInput<HeroTabType>(); break;
            default:
                Debug.LogWarning("알 수 없는 타입");
                break;
        }

    }


    private void SetTabInput<T>()
    {

        //만약 현재 게임 모드가 시스템 게임모드라면
        if (systemController.GetGameMode() is SystemMode)
        {

            Debug.Log($"인벤토리 입력 시 탭 타입 : {systemController.GetGameMode()}");
            Debug.Log($"인벤토리 입력 시 탭 타입 : {systemUISetter.GetCurTabType()}, {typeof(T).Name}, {systemUISetter.GetCurTabType() == typeof(T)}");
            //만약 현재 탭 포커스가 전환 대상 탭모드라면
            if (systemUISetter.GetCurTabType() == typeof(T))
            {
                systemUISetter.UIOff();
                systemController.RemoveCurGameMode();
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
            systemController.SetGameMode(typeof(SystemMode));
        }
    }

    /// <summary>
    /// 게임 모드 이벤트와 ui onOff함수를 연결하는 함수
    /// </summary>
    private void SetModeUI()
    {
        
        SubscribeModeUIEvent<WorldMode>(
            onUI: new Type[] { typeof(WorldMode) },
            offUI: new Type[] { typeof(CivilMode), typeof(BattleMode), typeof(SystemMode), typeof(DialogMode), typeof(PauseMode) }
        );

        SubscribeModeUIEvent<CivilMode>(
            onUI: new Type[] { typeof(CivilMode) },
            offUI: new Type[] { typeof(WorldMode), typeof(BattleMode), typeof(SystemMode), typeof(DialogMode), typeof(PauseMode) }
        );

        SubscribeModeUIEvent<BattleMode>(
            onUI: new Type[] { typeof(BattleMode) },
            offUI: new Type[] { typeof(WorldMode), typeof(CivilMode), typeof(SystemMode), typeof(DialogMode), typeof(PauseMode) }
        );

        SubscribeModeUIEvent<SystemMode>(
            onUI: new Type[] { typeof(SystemMode) },
            offUI: new Type[] { typeof(PauseMode) }
        );

        SubscribeModeUIEvent<DialogMode>(
            onUI: new Type[] { typeof(DialogMode) },
            offUI: new Type[] { typeof(WorldMode), typeof(CivilMode), typeof(BattleMode), typeof(SystemMode), typeof(PauseMode) }
        );

        SubscribeModeUIEvent<PauseMode>(
            onUI: new Type[] { typeof(PauseMode) },
            offUI: Array.Empty<Type>()
        );

    }

    private void SubscribeModeUIEvent<T>(Type[] onUI, Type[] offUI)
    {

        Action onModeUIChange = null;
        foreach (var type in onUI)
        {
            var localType = type; // 명시적 로컬 복사본 (구버전 컴파일러 대비, 가독성 목적)
            onModeUIChange += () => uiControlList[localType].UIOn();
        }
        foreach (var type in offUI)
        {
            var localType = type;
            onModeUIChange += () => uiControlList[localType].UIOff();
        }

        if (onModeUIChange == null)
            return; // 등록할 게 없으면 구독하지 않음
        onActionUI.Add(typeof(T), onModeUIChange);
        SystemEvent.Subscribe<T>(onActionUI[typeof(T)]);
    }

}
