using BattleDefinition;
using CharacterDefinition;
using FieldDefinition;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SystemDefinition
{


    #region 매니저 컨트롤러 정보 컨테이너 정의
    [System.Serializable]
    public class CameraSettings
    {
        //카메라 오브젝트
        public GameObject worldCamera;
        public GameObject battleCamera;

        //포커스된 카메라
        public GameObject foucsedCamera;

        //world 카메라 줌인아웃 관련
        public float maxZoom;
        public float minZoom;
        public float distance;
        public float zoomSpeed;

        //world 카메라 고정 상태
        public bool anchorSet;


        //world 카메라 자율 이동
        public float dragSpeed;
        public float pushSpeed;

        //world 카메라 월드 경계 정하기
        public Vector2 max;
        public Vector2 min;
        public float cameraHalfHeight;
        public float cameraHalfWidth;
    }

    [Serializable]
    public class SystemInfoContainer
    {
        /// <summary>
        /// 실행중인 게임 모드 스택
        /// </summary>
        public Stack<Type> gameMode;

        //모드 전환 딕셔너리
        public Dictionary<Type, BaseGameMode> modeFactory;

        /// <summary>
        /// 적제된 게임 모드 리스트. 시스템에서 관리하는 게임 모드 클래스 타입을 넣어주세요.
        /// </summary>
        public List<Type> modeList;
    }
    #endregion

    #region 게임 시스템 모드
    //게임 시스템의 상태
    /*
    BaseGameMode - 게임 시스템의 모든 상태의 조상 클래스
    FloorMode - 제일 기초가 되는 기반 게임모드
    └─WorldMode - 월드 상태
    └─CivilMode - 마을, 거점 등 인물 상태
    ChangeableMode - 다른 시스템과 바뀔수 있는 게임모드
    └─BattleMode - 전투 모드
    └─SystemMode - 화면 가득 채운 UI 상태
    └─DialogMode - 대화창 상태
    EndPointMode - 다른 시스템이 올라갈 수 없는 게임모드
    └─PauseMode - 게임 일시 정지
     */

    #region 조상 추상 클래스 설정
    //기본 시스템 클래스
    [Serializable]
    public abstract class BaseGameMode
    {
        protected ICameraFunc cameraController;

        public BaseGameMode(ICameraFunc camera)
        {
            cameraController = camera;
        }

        public abstract void SetUp();

        public abstract void EndSet();
    }

    //다른 시스템 위에 올라갈 수 없는 변경해야하는 기준점
    public abstract class FloorMode : BaseGameMode
    {
        public FloorMode(ICameraFunc camera) : base(camera)
        {

        }
    }

    //다른 시스템위에 올라갈 수 있는 게임 모드
    public abstract class CoverableMode : BaseGameMode
    {
        public CoverableMode(ICameraFunc camera) : base(camera)
        {

        }
    }

    //변경 시 바로 바뀌는 게임 모드
    public abstract class ChangableMode : CoverableMode
    {
        public ChangableMode(ICameraFunc camera) : base(camera)
        {

        }
    }

    //더이상 게임 모드가 올라갈 수 없는 마무리 게임 모드
    public abstract class EndPointMode : CoverableMode
    {
        public EndPointMode(ICameraFunc camera) : base(camera)
        {

        }
    }
    #endregion
    #region 모드 클래스 정의
#warning [시스템] 게임 모드에 따른 카메라 변경 구현 필요

    #region FloorMode 상속 모드
    //월드 모드
    public class WorldMode : FloorMode
    {
        public WorldMode(ICameraFunc camera) : base(camera)
        {

        }

        public override void SetUp()
        {

        }

        public override void EndSet()
        {
            
        }
    }

    //마을, 거점 등 인물 상태
    public class CivilMode : FloorMode
    {
        public CivilMode(ICameraFunc camera) : base(camera)
        {

        }

        public override void SetUp()
        {

        }

        public override void EndSet()
        {

        }
    }

    #endregion
    #region ChangableMode 상속 모드

    //전투 모드
    public class BattleMode : ChangableMode
    {
        public BattleMode(ICameraFunc camera) : base(camera)
        {

        }

        public override void SetUp()
        {

        }

        public override void EndSet()
        {

        }
    }

    //화면 가득 채운 UI 상태
    public class SystemMode : ChangableMode
    {
        public SystemMode(ICameraFunc camera) : base(camera)
        {

        }

        public override void SetUp()
        {

        }
        public override void EndSet()
        {

        }
    }

    //대화창 상태
    public class DialogMode : ChangableMode
    {
        public DialogMode(ICameraFunc camera) : base(camera)
        {

        }

        public override void SetUp()
        {

        }
        public override void EndSet()
        {

        }

    }
    #endregion
    #region EndPointMode 상속 모드
    //게임 일시 정지
    public class PauseMode : EndPointMode
    {
        public PauseMode(ICameraFunc camera) : base(camera)
        {

        }

        public override void SetUp()
        {

        }

        public override void EndSet()
        {

        }

    }
    #endregion
    #endregion
    #endregion

    #region 인터페이스 구현
    //UI매니저 인터페이스
    public interface IUIFunc
    {
        void AddSelectedPathTile(Vector2Int xy);
        void RemoveSelectedPathRangeTile(Vector2Int xy);
        void RemoveSelectedPathTile(Vector2Int xy);

        /// <summary>
        /// 모드를 기반으로 UI를 끄고 킵니다.
        /// </summary>
        /// <param name="OnOff">True = UI 켜기, False = UI 끄기</param>
        /// <param name="gamemode">제어 대상 게임 모드</param>
        void UIModeChange(bool OnOff, Type gamemode);

        /// <summary>
        /// TabType을 받아 SystemUI를 전환합니다.
        /// </summary>
        /// <param name="tabType">TabType 기반 클래스의 타입</param>
        void TabModeChange(Type tabType);
    }

    public interface ISystemFunc
    {
        BaseGameMode GetGameMode();

        /// <summary>
        /// 요청 게임모드로 전환
        /// </summary>
        /// <param name="request">전환할 대상(BaseGameMode 기반 클래스 타입)</param>
        void SetGameMode(Type request);

        /// <summary>
        /// 현재 게임모드를 한단계 종료 
        /// </summary>
        void RemoveCurGameMode();

        /// <summary>
        /// 사용중인 게임 모드 리스트 반환
        /// </summary>
        /// <returns>Type 배열</returns>
        Type[] GetModeList();
    }

    public interface ICameraFunc
    {
        void CameraUpdate();

        void FocusWorldCamera();

        void FocusBattleCamera();
    }

    public interface IInputFunc
    {

    }

    #endregion

#region 매니저, 컨트롤러간 연결용 클래스
    public class IManageSetterFunc : MonoBehaviour
    {
        public virtual void GridSetter(IGridFunc grid)
        {
            return;
        }

        public virtual void UISetter(IUIFunc ui)
        {
            return;
        }

        public virtual void SystemSetter(ISystemFunc system)
        {
            return;
        }

        public virtual void PartySetter(IPartyFunc party)
        {
            return;
        }

        public virtual void EncounterSetter(IEncounterFunc encounter)
        {
            return;
        }

        public virtual void InputSetter(IInputFunc input)
        {
            return;
        }

        public virtual void MoveControlSetter(IMovementFuc movecontrol)
        {
            return;
        }

        public virtual void CameraControlSetter(ICameraFunc cameracontrol)
        {
            return;
        }

        public virtual void BattleControlSetter(IBattleFunc battlecontrol)
        {
            return;
        }

    }
    #endregion


    #region 시스템 이벤트 발생 클래스
    public static class SystemEvent
    {

        private static readonly Dictionary<Type, Delegate> _systemEvents = new();

        /// <summary>
        /// 시스템 변경 이벤트 구독 메서드. T는 BaseGameMode 기반 클래스 타입이어야 합니다.
        /// </summary>
        /// <typeparam name="T">BaseGameMode 기반 클래스 타입</typeparam>
        /// <param name="listener">이벤트 리스너</param>
        /// <exception cref="Exception">T가 BaseGameMode 기반 클래스가 아닌 경우 발생</exception>
        public static void Subscribe<T>(Action listener)
        {
            //게임 모드 클래스인지 검사
            if (!typeof(BaseGameMode).IsAssignableFrom(typeof(T)))
            {
                throw new Exception("Type must be a subclass of BaseGameMode");
            }
            //이벤트 딕셔너리에 액션 추가
            var type = typeof(T);
            if (_systemEvents.ContainsKey(type))
                _systemEvents[type] = Delegate.Combine(_systemEvents[type], listener);
            else
                _systemEvents[type] = listener;
        }

        /// <summary>
        /// 시스템 변경 이벤트 구독 메서드 (Type 직접 전달 버전).
        /// 런타임에 Type 변수만 있는 경우 사용합니다.
        /// </summary>
        /// <param name="type">BaseGameMode 기반 클래스 Type</param>
        /// <param name="listener">이벤트 리스너</param>
        public static void Subscribe(Type type, Action listener)
        {
            if (!typeof(BaseGameMode).IsAssignableFrom(type))
            {
                throw new Exception("Type must be a subclass of BaseGameMode");
            }
            if (_systemEvents.ContainsKey(type))
                _systemEvents[type] = Delegate.Combine(_systemEvents[type], listener);
            else
                _systemEvents[type] = listener;
        }

        /// <summary>
        /// 시스템 변경 이벤트 구독 해제 메서드. T는 BaseGameMode 기반 클래스 타입이어야 합니다.
        /// </summary>
        /// <typeparam name="T">BaseGameMode 기반 클래스 타입</typeparam>
        /// <param name="listener">이벤트 리스너</param>
        /// <exception cref="Exception">T가 BaseGameMode 기반 클래스가 아닌 경우 발생</exception>
        public static void Unsubscribe<T>(Action listener)
        {
            //게임 모드 클래스인지 검사
            if (!typeof(BaseGameMode).IsAssignableFrom(typeof(T)))
            {
                throw new Exception("Type must be a subclass of BaseGameMode");
            }
            var type = typeof(T);
            if (_systemEvents.ContainsKey(type))
            {
                try
                {
                    _systemEvents[type] = Delegate.Remove(_systemEvents[type], listener);
                    if (_systemEvents[type] == null)
                        _systemEvents.Remove(type);
                }
                catch (InvalidOperationException)
                {
                    // 종료/도메인 리로드 등 비정상 상태에서의 안전한 무시
                    _systemEvents.Remove(type);
                }
            }
        }

        /// <summary>
        /// 시스템 변경 이벤트 구독 해제 메서드 (Type 직접 전달 버전).
        /// </summary>
        /// <param name="type">BaseGameMode 기반 클래스 Type</param>
        /// <param name="listener">이벤트 리스너</param>
        public static void Unsubscribe(Type type, Action listener)
        {
            if (!typeof(BaseGameMode).IsAssignableFrom(type))
            {
                throw new Exception("Type must be a subclass of BaseGameMode");
            }
            if (_systemEvents.ContainsKey(type))
            {
                try
                {
                    _systemEvents[type] = Delegate.Remove(_systemEvents[type], listener);
                    if (_systemEvents[type] == null)
                        _systemEvents.Remove(type);
                }
                catch (InvalidOperationException)
                {
                    // 종료/도메인 리로드 등 비정상 상태에서의 안전한 무시
                    _systemEvents.Remove(type);
                }
            }
        }

        /// <summary>
        /// 시스템 변경 발생 메서드. T는 BaseGameMode 기반 클래스 타입이어야 합니다.
        /// </summary>
        /// <typeparam name="T">BaseGameMode 기반 클래스 타입</typeparam>
        /// <param name="eventData">발생시킬 이벤트 데이터</param>
        /// <exception cref="Exception">T가 BaseGameMode 기반 클래스가 아닌 경우 발생</exception>
#warning [시스템] 이벤트 발생 시 추가 데이터 전달 매개변수 DataClass eventData 추가 필요
        public static void Publish<T>()
        {
            Debug.Log($"퍼블리싱 이벤트 발생: {typeof(T).Name}");
            //게임 모드 클래스인지 검사
            if (!typeof(BaseGameMode).IsAssignableFrom(typeof(T)))
            {
                throw new Exception("Type must be a subclass of BaseGameMode");
            }

            if (_systemEvents.TryGetValue(typeof(T), out var del)) {
                Debug.Log($"이벤트 구독자 수: {((Action)del)?.GetInvocationList().Length ?? 0}");
                ((Action)del)?.Invoke();
            }
        }

        /// <summary>
        /// 시스템 변경 발생 메서드. T는 BaseGameMode 기반 클래스 타입이어야 합니다. (Type 직접 전달 버전).
        /// </summary>
        /// <param name="type">BaseGameMode 기반 클래스 Type</param>
        /// <exception cref="Exception">T가 BaseGameMode 기반 클래스가 아닌 경우 발생</exception>
#warning [시스템] 이벤트 발생 시 추가 데이터 전달 매개변수 DataClass eventData 추가 필요
        public static void Publish(Type type)
        {
            Debug.Log($"Publishing event for type: {type.Name}");
            //게임 모드 클래스인지 검사
            if (!typeof(BaseGameMode).IsAssignableFrom(type))
            {
                throw new Exception("Type must be a subclass of BaseGameMode");
            }

            if (_systemEvents.TryGetValue(type, out var del))
                ((Action)del)?.Invoke();
        }
    }


    #endregion

    //WorldMode HUD UI 제어 모노비헤이비어 정의
    public class HUDSetter : MonoBehaviour
    {

        protected TextMeshProUGUI valueText;

        void Awake()
        {
            valueText = GetComponentInChildren<TextMeshProUGUI>();

        }


        public virtual void TextSet(string value)
        {
            valueText.text = value;
        }
    }


    //UIController 하위 컴포넌트 상속용 MonBehaviour 클래스
    public abstract class BaseUIControl : MonoBehaviour
    {
        public abstract void UIOn();

        public abstract void UIOff();

        public virtual bool IsActive()
        {
            return false;
        }
    }

    //Grid 타일 UI
    public enum GridUIType
    {
        None = 0,
        Path, //플레이어 이동 경로 표시 UI

    }

    #region 탭 UI 타입 정의
    [Serializable]
    public abstract class TabType
    {
        public string Name { get; set; }
        protected TabType(string name) { Name = name; }

    }

    [Serializable]
    public class InventoryTabType : TabType
    {
        public InventoryTabType(string name) : base(name) { } 
    }

    [Serializable]
    public class PartyTabType : TabType
    {
        public PartyTabType(string name) : base(name) { }
    }

    [Serializable]
    public class HeroTabType : TabType
    {
        public HeroTabType(string name) : base(name) { }
    }
    #endregion

    [Serializable]
    public struct SystemTabEntry
    {
        [SerializeReference]
        public TabType tabType;
        public GameObject tabPrefeb;
    }


}
