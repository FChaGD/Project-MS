using GroupDefinition;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    [Serializable]
    public enum MovementType
    {
        Walk, //지상 이동
        Sail, //해상 이동
        Fly //공중 이동
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

    //플레이어 이동 관련 정보 클래스
    [Serializable]
    public class MovementInfo
    {

        //플레이어 이동 경로
        public List<Vector2Int> movePath;

        //이동 상태
        public MovementType movementType;

        //이동 대상 Transform
        public Transform targetTransform;

        //이동 대상의 레이어 깊이 정보
        public int layer;

        //플레이어 이동 변수
        public float MoveSecound = 8f;
        public float moveSpeed = 0.125f;
    }

    //A* 알고리즘에서 사용할 노드 클래스
    [Serializable]
    public class PathFinderNode
    {
        public PathFinderNode(bool _canPass, int _x, int _y) { canPass = _canPass; x = _x; y = _y; }

        public bool canPass;
        public PathFinderNode ParentNode;

        // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
        public int x, y, G, H;
        public int F { get { return G + H; } }
    }

    //GridManager가 외부에 제공하는 함수
    public interface IGridFunc
    {
        Vector2Int GetTileVectorAtWorldPosition(Vector3 worldPos);
        Vector2[] GetMaxMinXY();
        Vector2 GetCellsize();
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
        void StopMovement();
    }


}