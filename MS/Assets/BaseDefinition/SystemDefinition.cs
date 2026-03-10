using FieldDefinition;
using UnityEngine;

namespace SystemDefinition
{
    //기능 UI
    public enum UIType
    {
        None = 0,
        Path, //플레이어 이동 경로 표시 UI

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
        public virtual void EncounterSetter(IEncounterFunc encounter)
        {
            return;
        }

        public virtual void MoveControlSetter(IMovementFuc movecontrol)
        {
            return;
        }
    }
}
