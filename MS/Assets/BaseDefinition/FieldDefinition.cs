using System;

namespace FieldDefinition
{
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
        private TileType tileType;
        private string[] eventList;
        private InfluenceData areaData;

        public FieldData() {
            
        }
        
    }

    //영향력 관련 정보 클래스
    public class InfluenceData
    {
        private string base_group;
        private string sub_group;
        public string Base_Group {
            get { return base_group; }
            set { base_group = value; }
        }
        public string Sub_Group
        {
            get { return sub_group; }
            set { sub_group = value; }
        }

        public InfluenceData()
        {

        }
    }
}