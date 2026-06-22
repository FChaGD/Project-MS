using System;
using System.Collections.Generic;
using System.Reflection;
using SystemDefinition;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

#region Input Action 클래스
//입력 액션 갱신 규칙
//1. 맵핑을 새로 정의했다면 Events 클래스 내에서 맵핑을 클래스로 만들 것(Serializable을 붙힐 것)
//2. 액션 맵 클래스 내부에 public UnityEvent<InputAction.CallbackContext> 변수로 액션을 정의할 것
//3. Events 클래스에서 만든 클래스는 반드시 public으로 클래스 내부에 변수로 선언할 것
//4. 컨트롤러 스크립트 Awake()에서 액션 맵의 FindActionMap()으로 액션 맵을 가져올 것

[Serializable]
public class Events
{

    [Serializable]
    public class CameraEvent
    {
        //Inspector에 노출될 UnityEvent
        [Header("Camera Events")]
        public UnityEvent<InputAction.CallbackContext> CameraZoom;
        public UnityEvent<InputAction.CallbackContext> CameraAnchor;
        public UnityEvent<InputAction.CallbackContext> CameraDragPress;
        public UnityEvent<InputAction.CallbackContext> CameraDragMove;
        public UnityEvent<InputAction.CallbackContext> CameraFocus;
    }

    [Serializable]
    public class WorldEvent
    {
        //Inspector에 노출될 UnityEvent
        [Header("World Events")]
        public UnityEvent<InputAction.CallbackContext> PathDecide;
        public UnityEvent<InputAction.CallbackContext> MoveStart;
        public UnityEvent<InputAction.CallbackContext> PathCheckUI;
    }

    [Serializable]
    public class SystemUIEvent
    {
        //Inspector에 노출될 UnityEvent
        [Header("System Events")]
        public UnityEvent<InputAction.CallbackContext> Inventory;
        public UnityEvent<InputAction.CallbackContext> Map;
        public UnityEvent<InputAction.CallbackContext> Hero;
        public UnityEvent<InputAction.CallbackContext> Quest;
        public UnityEvent<InputAction.CallbackContext> Party;
        public UnityEvent<InputAction.CallbackContext> Pause;
        public UnityEvent<InputAction.CallbackContext> Group;
    }


    public CameraEvent Camera;
    public WorldEvent World;
    public SystemUIEvent SystemUI;

}
#endregion

public class InputController : IManageSetterFunc, IInputFunc
{
    //인풋 액션 에셋
    [SerializeField] private InputActionAsset inputActionAsset;

    [SerializeField]
    private InputSystemUIInputModule inputSystemUIInputModule;

    public Events inputEvents;

    // 자동 바인딩으로 수집된 (액션, 이벤트) 쌍 저장
    private List<(InputAction action, UnityEvent<InputAction.CallbackContext> evt)> boundActions = new();

    //액션 맵
    private InputActionMap cameraMap;
    private InputActionMap worldInputMap;
    private InputActionMap systemUIMap;

    //외부 매니저 컨트롤러 변수
    private ISystemFunc systemController;

    //게임 모드 별 액션 맵 활성화/비활성화 맵핑
    private Dictionary<Type, Action> onActionMap = new();

    void Awake()
    {
        //액션 맵 받아오기
        cameraMap = inputActionAsset.FindActionMap("Camera");
        worldInputMap = inputActionAsset.FindActionMap("World");
        systemUIMap = inputActionAsset.FindActionMap("SystemUI");


        //액션에 이벤트 연결
        AutoBind();


#warning [플레이어 입력] 액션 맵의 초기 활성화 로직 작성 필요
#warning [플레이어 입력] 액션 맵 활성화 로직 구현 필요

        //게임 모드 전환 이벤트 별 액션 맵 활성화/비활성화 맵핑 등록
        SetModeMap();


        cameraMap.Enable();
        worldInputMap.Enable();
        systemUIMap.Enable();

    }

    void OnDestroy()
    {
        if (!Application.isPlaying) return;
        else
        {
            foreach (Type mode in systemController.GetModeList())
            {
                if (onActionMap.TryGetValue(mode, out Action action))
                {
                    SystemEvent.Unsubscribe(mode, action);
                }
            }

        }
        // 메모리 누수 방지 — 콜백 해제
        AutoUnbind();
    }

    public override void SystemSetter(ISystemFunc system)
    {
        systemController = system;
    }


    //액션에 이벤트 연결
    private void AutoBind()
    {
        // Events 클래스의 모든 필드 순회 (CameraEvent, WorldEvent ...)
        foreach (var mapField in typeof(Events).GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            object mapInstance = mapField.GetValue(inputEvents);
            if (mapInstance == null) continue;

            // 필드명에서 액션 맵 이름 추출 ("Camera", "World" ...)
            string mapName = mapField.Name;
            InputActionMap actionMap = inputActionAsset.FindActionMap(mapName);
            if (actionMap == null)
            {
                Debug.LogWarning($"[InputController] ActionMap '{mapName}' 을 찾을 수 없습니다.");
                continue;
            }

            // 각 맵 클래스 안의 UnityEvent 필드 순회 (CameraZoom, PathDecide ...)
            foreach (var actionField in mapInstance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (actionField.FieldType != typeof(UnityEvent<InputAction.CallbackContext>))
                    continue;

                string actionName = actionField.Name;
                InputAction action = actionMap.FindAction(actionName);
                if (action == null)
                {
                    Debug.LogWarning($"[InputController] Action '{actionName}' 을 '{mapName}' 맵에서 찾을 수 없습니다.");
                    continue;
                }

                var evt = (UnityEvent<InputAction.CallbackContext>)actionField.GetValue(mapInstance);

                // 콜백 연결
                action.started += evt.Invoke;
                action.performed += evt.Invoke;
                action.canceled += evt.Invoke;

                // 나중에 해제하기 위해 저장
                boundActions.Add((action, evt));

            }
        }
    }

    //콜백 해제
    private void AutoUnbind()
    {
        foreach (var (action, evt) in boundActions)
        {
            action.started -= evt.Invoke;
            action.performed -= evt.Invoke;
            action.canceled -= evt.Invoke;
        }
        boundActions.Clear();
    }

    /// <summary>
    /// 게임 모드 전환 이벤트 별 액션 맵 활성화/비활성화 맵핑 등록
    /// </summary>
    private void SetModeMap()
    {

        //월드 게임 모드 진입 시 액션 맵 설정
        SubscribeModeMapEvent<WorldMode>(
            enableMap: new[] { "World", "Camera", "SystemUI" },
            disableMap: Array.Empty<string>()
        );

        //시내 게임 모드 진입 시 액션 맵 설정
        SubscribeModeMapEvent<CivilMode>(
            enableMap: new[] { "SystemUI" },
            disableMap: new[] { "Camera", "World" }
        );

        //전투 게임 모드 진입 시 액션 맵 설정
        SubscribeModeMapEvent<BattleMode>(
            enableMap: new[] { "SystemUI" },
            disableMap: new[] { "Camera", "World" }
        );

        //시스템 UI 게임 모드 진입 시 액션 맵 설정
        SubscribeModeMapEvent<SystemMode>(
            enableMap: new[] { "SystemUI" },
            disableMap: new[] { "Camera", "World" }
        );

        //대화 게임 모드 진입 시 액션 맵 설정
        SubscribeModeMapEvent<DialogMode>(
            enableMap: new[] { "SystemUI" },
            disableMap: new[] { "Camera", "World" }
        );

        //일시정지 게임 모드 진입 시 액션 맵 설정
        SubscribeModeMapEvent<PauseMode>(
            enableMap: new[] { "SystemUI" },
            disableMap: new[] { "Camera", "World" }
        );


    }

    private void EnableMap(string mapName)
    {
        inputActionAsset.FindActionMap(mapName)?.Enable();
    }

    private void DisableMap(string mapName)
    {
        inputActionAsset.FindActionMap(mapName)?.Disable();
    }

    private void SubscribeModeMapEvent<T>(String[] enableMap, String[] disableMap)
    {
        Action onModeMapChange = null;
        foreach (var map in enableMap)
        {
            var localMap = map; // 명시적 로컬 복사본 (구버전 컴파일러 대비, 가독성 목적)
            onModeMapChange += () => EnableMap(localMap);
        }
        foreach (var map in disableMap)
        {
            var localMap = map;
            onModeMapChange += () => DisableMap(localMap);
        }

        if (onModeMapChange == null)
            return; // 등록할 게 없으면 구독하지 않음

        onActionMap.Add(typeof(T), onModeMapChange);
        SystemEvent.Subscribe<T>(onActionMap[typeof(T)]);
    }
}
