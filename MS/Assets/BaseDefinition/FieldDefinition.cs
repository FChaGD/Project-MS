using GroupDefinition;
using System;
using UnityEngine;

namespace FieldDefinition
{
    //타일 종류 Enum 정의
    public enum TileType
    {
        Field, // 들판
        Mountain, // 산맥
        Forest, // 숲
        River, // 강
        Ocean, // 바다
        Swamp, // 늪지
        Desert, // 사막
        Civil, // 문명화
    }

    // 필드 데이터 집합 클래스
    public class FieldData
    {
        private bool hasRoad;
        private bool canPass;
        private TileType tileType;
        private string[] eventList;
        private InfluenceData areaData;
        
    }

    //영향력 관련 정보 클래스
    public class InfluenceData
    {
        //영토로 삼고 있는 국가와 영향력
        private ContryType base_group;
        private float baseGroupInfluPower;
        //지역을 실질적으로 다스리는 조직과 영향력
        private GroupType sub_group;
        private float subGroupInfluPower;
        //지역에 영향을 끼치는 종교단체
        private ReligionType religion_group;

        public ContryType Base_Group {
            get { return base_group; }
            set { base_group = value; }
        }
        public GroupType Sub_Group
        {
            get { return sub_group; }
            set { sub_group = value; }
        }
        public ReligionType Religion_Group
        {
            get { return religion_group; }
            set { religion_group = value; }

        }
    }

    //GridManager가 외부에 제공하는 함수
    public interface IGridFunc
    {
        Vector2Int GetTileVectorAtWorldPosition(Vector3 worldPos);
        Vector2[] getMaxMinXY();
        Vector2 getCellsize();
        Vector3 GetCenter(Vector2Int cellPosition);

    }
    public interface IEncounterFunc
    {

    }

    //MovementController가 외부에 제공하는 함수
    public interface IMovementFuc
    {
        void AddMovementEvent(Action action);
        void RemoveMovementEvent(Action action);
    }


}