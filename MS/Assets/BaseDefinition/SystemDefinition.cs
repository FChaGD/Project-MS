using FieldDefinition;
using System;
using TMPro;
using UnityEngine;

namespace SystemDefinition
{
    //기능 UI
    public enum UIType
    {
        None = 0,
        Path, //플레이어 이동 경로 표시 UI

    }

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

    //게임 시스템의 상태
    public enum GameModeType
    {
        World, //월드 이동 상태
        Base, //마을, 거점 등 인물 상태
        Dialog, //대화창 상태
        Battle, //전투 상태

    }

    //UI매니저 인터페이스
    public interface IUIFunc
    {
        void AddSelectedPathTile(Vector2Int xy);
        void RemoveSelectedPathRangeTile(Vector2Int xy);
        void RemoveSelectedPathTile(Vector2Int xy);
    }

    public interface ISystemFunc
    {
        GameModeType GetGameMode();
    }

    public interface ICameraFunc
    {
        void CameraUpdate();
    }



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

        public virtual void MoveControlSetter(IMovementFuc movecontrol)
        {
            return;
        }

        public virtual void CameraControlSetter(ICameraFunc cameracontrol)
        {
            return;
        }

    }

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

}
