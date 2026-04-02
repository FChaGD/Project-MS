using FieldDefinition;
using System;
using TMPro;
using Unity.XR.OpenVR;
using UnityEngine;

namespace SystemDefinition
{

    [System.Serializable]
    public class CameraSettings
    {
        //카메라 줌인아웃 관련
        public float maxZoom;
        public float minZoom;
        public float distance;
        public float zoomSpeed;

        //카메라 고정 상태
        public bool anchorSet;


        //카메라 자율 이동
        public float dragSpeed;
        public float pushSpeed;

        //카메라 월드 경계 정하기
        public Vector2 max;
        public Vector2 min;
        public float cameraHalfHeight;
        public float cameraHalfWidth;
    }

    #region 게임 시스템 모드
    //게임 시스템의 상태
    #region 조상 추상 클래스 설정
    //기본 시스템 클래스
    [Serializable]
    public abstract class BaseGameMode
    {
        protected IInputFunc inputController;
        protected IUIFunc uiController;
        protected ICameraFunc cameraController;

        public BaseGameMode(IInputFunc input, IUIFunc ui, ICameraFunc camera)
        {
            inputController = input;
            uiController = ui;
            cameraController = camera;
        }

        public abstract void SetUp();

        public abstract void EndSet();
    }

    //다른 시스템 위에 올라갈 수 없는 변경해야하는 기준점
    public abstract class FloorMode : BaseGameMode
    {
        public FloorMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
        {

        }
    }

    //다른 시스템위에 올라갈 수 있는 게임 모드
    public abstract class CoverableMode : BaseGameMode
    {
        public CoverableMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
        {

        }
    }

    //변경 시 바로 바뀌는 게임 모드
    public abstract class ChangableMode : CoverableMode
    {
        public ChangableMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
        {

        }
    }

    //더이상 게임 모드가 올라갈 수 없는 마무리 게임 모드
    public abstract class EndPointMode : CoverableMode
    {
        public EndPointMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
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
        public WorldMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
        {

        }

        public override void SetUp()
        {

        }

        public override void EndSet()
        {
            
        }
    }

    //전투 모드
    public class BattleMode : FloorMode
    {
        public BattleMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
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
        public CivilMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
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
    //화면 가득 채운 UI 상태
    public class SystemMode : ChangableMode
    {
        public SystemMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
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
        public DialogMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
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
        public PauseMode(IInputFunc input, IUIFunc ui, ICameraFunc camera) : base(input, ui, camera)
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
    }

    public interface ICameraFunc
    {
        void CameraUpdate();
    }

    public interface IInputFunc
    {

    }

    #endregion

    //매니저, 컨트롤러간 연결용 클래스
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

    }

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

    //기능 UI
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
    #endregion

    [Serializable]
    public struct SystemTabEntry
    {
        [SerializeReference]
        public TabType tabType;
        public GameObject tabPrefeb;
    }



}
