using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


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
        public UnityEvent<InputAction.CallbackContext> Character;
        public UnityEvent<InputAction.CallbackContext> Quest;
        public UnityEvent<InputAction.CallbackContext> Party;
        public UnityEvent<InputAction.CallbackContext> Pause;
    }


    public CameraEvent Camera;
    public WorldEvent World;
    public SystemUIEvent SystemUI;

}


public class InputController : MonoBehaviour
{
    //인풋 액션 에셋
    [SerializeField] private InputActionAsset inputActionAsset;

    public Events events;

    // 자동 바인딩으로 수집된 (액션, 이벤트) 쌍 저장
    private List<(InputAction action, UnityEvent<InputAction.CallbackContext> evt)> boundActions = new();

    //액션 맵
    private InputActionMap cameraMap;
    private InputActionMap worldInputMap;

    void Awake()
    {
        //액션 맵 받아오기
        cameraMap = inputActionAsset.FindActionMap("Camera");
        worldInputMap = inputActionAsset.FindActionMap("World");


        //액션에 이벤트 연결
        AutoBind();


#warning [플레이어 입력] 액션 맵의 초기 활성화 로직 작성 필요
#warning [플레이어 입력] 액션 맵 활성화 로직 구현 필요

        cameraMap.Enable();
        worldInputMap.Enable();

    }

    void OnDestroy()
    {
        // 메모리 누수 방지 — 콜백 해제
        AutoUnbind();
    }

    //액션에 이벤트 연결
    private void AutoBind()
    {
        // Events 클래스의 모든 필드 순회 (CameraEvent, WorldEvent ...)
        foreach (var mapField in typeof(Events).GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            object mapInstance = mapField.GetValue(events);
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

    public void EnableMap(string mapName)
    {
        inputActionAsset.FindActionMap(mapName)?.Enable();
    }

    public void DisableMap(string mapName)
    {
        inputActionAsset.FindActionMap(mapName)?.Disable();
    }

}
