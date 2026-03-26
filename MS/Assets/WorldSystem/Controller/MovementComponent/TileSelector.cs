using FieldDefinition;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR;

public class TileSelector : MonoBehaviour
{
    //타일값 받아오기
    public Func<Vector3, Vector2Int> GetTileVectorAtWorldPosition;
    //ui 매니저 요청
    public Action<Vector2Int> AddSelectedPathTile;
    public Action<Vector2Int> RemoveSelectedPathRangeTile;
    //그리드 매니저 요청
    public Func<Vector2[]> GetMaxMinXY;

    //타일 선택시 위치를 확인할 카메라
    private Camera cameraOfPlayer;

    //월드 최대 및 최소 크기;
    private Vector2 min;
    private Vector2 max;
    private Vector2Int cellcount;
    private Vector2 cellsize;

    //이동 관련 정보 클래스
    private MovementInfo _movementInfo;

    //A* 알고리즘 관련 변수
    [Header("A* 알고리즘 허용 여부")]
    [SerializeField]
    [Tooltip("대각선 이동 허용 여부")]
    private bool allowDiagonal = true;
    [SerializeField]
    [Tooltip("코너 돌기 허용 여부")]
    private bool dontCrossCorner = true;
    private Dictionary<Vector2Int, PathFinderNode> NodeArray;
    private PathFinderNode StartNode, TargetNode, CurNode;
    private List<PathFinderNode> OpenList, ClosedList;
    private List<PathFinderNode> FinalNodeList;


    public void Init(MovementInfo movementInfo)
    {
        _movementInfo = movementInfo;
        _movementInfo.movePath = new List<Vector2Int>();
    }

    public void SetMinMax(Vector2[] minmax)
    {
        min = minmax[0];
        max = minmax[1];
        cellcount = new Vector2Int((int)minmax[2].x, (int)minmax[2].y);
        cellsize = minmax[3];
    }

    void Start()
    {
        cameraOfPlayer = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

    }

    //선택 타일 경로 추가 제거 함수
    public void MovePathAddReMoveFunc()
    {
        //마우스 커서 위치 받아오기
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cameraOfPlayer.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;

        //선택한 타일
        Vector2Int targetTile = GetTileVectorAtWorldPosition(worldPos);


        //월드에서 벗어난 클릭이라면 작동 X
        if (targetTile.x < min.x || targetTile.x > max.x - 1 || targetTile.y < min.y || targetTile.y > max.y - 1)
        {
            return;
        }
        Debug.Log($"선택한 타일 : {targetTile}, Min : {min}, Max : {max}");

        //시작 타일
        Vector2Int presentTile = new Vector2Int(0, 0);
        if (_movementInfo.movePath.Count == 0)
        {
            presentTile = GetTileVectorAtWorldPosition(transform.position);
        }
        else
        {
            presentTile = _movementInfo.movePath[_movementInfo.movePath.Count-1];
        }


        //선택한 타일이 이미 리스트에 존재하는가?
        if (_movementInfo.movePath.Contains(targetTile))
        {
            //타일 제거
            RemoveMiddlePathFunc(targetTile);
            return;
        }

        if (presentTile == targetTile)
        {
            //만약 플레이어 캐릭터가 위치한 타일을 선택했다면
            return;
        }

        //리스트에도 없고 플레이어 위치도 아나라면 경로 추가
        if (!_movementInfo.movePath.Contains(targetTile))
        {
            AStarPathFinding(presentTile, targetTile);
            Debug.Log($"A* 알고리즘 종료, 배열 길이 : {FinalNodeList.Count}");
            AddPathbyAStar();

        }

    }



    //경로 추가 함수
    private void AddPathFunc(Vector2Int presentTile, Vector2Int targetTile)
    {
        Debug.Log($"타일 추가, targetTile : {targetTile}");
        if (Mathf.Abs(presentTile.x - targetTile.x) <= 1 && Mathf.Abs(presentTile.y - targetTile.y) <= 1)
        {
            //플레이어가 마지막으로 선택한 경로의 -1 ~ 1 사이의 타일인지
            _movementInfo.movePath.Add(targetTile);
            AddSelectedPathTile(targetTile);
            return;
        }
        else
        {
            //리스트가 비어있거나 너무 먼 타일을 선택했을 경우
            return;
        }
    }

    //중간 경로 없애기
    private void RemoveMiddlePathFunc(Vector2Int targetTile)
    {
        _movementInfo.movePath.RemoveRange(_movementInfo.movePath.IndexOf(targetTile), _movementInfo.movePath.Count - _movementInfo.movePath.IndexOf(targetTile));
        RemoveSelectedPathRangeTile(targetTile);
    }

    //A* 알고리즘 구현
    public void AStarPathFinding(Vector2Int presentTile, Vector2Int targetTile)
    {


        // NodeArray의 크기 정해주고, canPass, x, y 대입
        NodeArray = new Dictionary<Vector2Int, PathFinderNode>();
        for (int i = cellcount.x; i <= cellcount.y; i++)
        {
            for (int j = cellcount.x; j <= cellcount.y; j++)
            {
                NodeArray[new Vector2Int(i, j)] = new PathFinderNode(true, i, j);
            }
        }


        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[new Vector2Int(presentTile.x, presentTile.y)];
        TargetNode = NodeArray[new Vector2Int(targetTile.x, targetTile.y)];

        OpenList = new List<PathFinderNode>() { StartNode };
        ClosedList = new List<PathFinderNode>();
        FinalNodeList = new List<PathFinderNode>();

        while (OpenList.Count > 0)
        {
            
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // 마지막
            if (CurNode == TargetNode)
            {

                PathFinderNode TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Reverse();

                return;
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 통과할 수 있으면서, 닫힌리스트에 없다면
        CanPassChecker(checkX, checkY);

        if (!(checkX > cellcount.y) && !(checkX < cellcount.x) && !(checkY > cellcount.y) && !(checkY < cellcount.x) && NodeArray[new Vector2Int(checkX, checkY)].canPass && !ClosedList.Contains(NodeArray[new Vector2Int(checkX, checkY)]))
        {
            // 대각선 허용시, 통과 불가능한 곳 사이로 통과 안됨
            if (allowDiagonal) {
                CanPassChecker(CurNode.x, checkY);
                CanPassChecker(checkX, CurNode.y);
                if (!NodeArray[new Vector2Int(CurNode.x, checkY)].canPass && !NodeArray[new Vector2Int(checkX, CurNode.y)].canPass) {
                    return;
                }
            }

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) {
                CanPassChecker(CurNode.x, checkY);
                CanPassChecker(checkX, CurNode.y);
                if (!NodeArray[new Vector2Int(CurNode.x, checkY)].canPass || !NodeArray[new Vector2Int(checkX, CurNode.y)].canPass){
                    return;
                }
             }

            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            PathFinderNode NeighborNode = NodeArray[new Vector2Int(checkX, checkY)];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    private void CanPassChecker(int x, int y)
    {
        if(x < cellcount.x || x > cellcount.y || y < cellcount.x || y > cellcount.y)
        {
            return;
        }

#warning [플레이어 이동] 바다, 비행등이 구현될 때 타일마다 이동 가능 여부 체크하는 함수 나중에 구현 필요

        NodeArray[new Vector2Int(x, y)].canPass = true;
    }

    private void AddPathbyAStar()
    {
        foreach(PathFinderNode tmp in FinalNodeList)
        {
            Vector2Int targetTile = new Vector2Int(tmp.x, tmp.y);
            if (_movementInfo.movePath.Count == 0)
            {
                AddPathFunc(GetTileVectorAtWorldPosition(transform.position), targetTile);
            }
            else
            { 
                AddPathFunc(_movementInfo.movePath[_movementInfo.movePath.Count-1], targetTile);
            }
        }
    }



}
