using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    private Tilemap tilemap;

    [Header("Tile Mapping")]
    public TileBase groundTile;
    public TileBase mountainTile;
    public TileBase waterTile;

    //셀 사이즈
    private float cellWidth;
    private float cellHeight;

    //좌하단 우상단 좌표 = 최대최소 좌표
    private Vector2 maxXY;
    private Vector2 minXY;

    // 좌표 범위
    [Header("Tile Range")]
    [SerializeField]
    private int min = -29;
    [SerializeField]
    private int max = 30;
    public int Min => min;
    public int Max => max;

    // 테이블 데이터 (예시)
    public Dictionary<Vector2Int, int> tileTable = new Dictionary<Vector2Int, int>();

    void Awake()
    {
        GameObject target = GameObject.FindWithTag("Map");
        grid = target.GetComponent<Grid>();
        tilemap = target.GetComponentInChildren<Tilemap>();
        cellWidth = 0;
        cellHeight = 0;
    }

    void Start()
    {
        GenerateTable();
        BuildTilemap();
        SetCellSize();
    }

    //셀사이즈 계산후 저장
    private void SetCellSize()
    { 
        Debug.Log("셀 사이즈" + grid.cellSize.x + ", " + grid.cellSize.y);
        cellWidth = grid.cellSize.x;
        cellHeight = grid.cellSize.y;
        Debug.Log("셀 사이즈" + cellWidth + ", " + cellHeight);

    }

    //셀 사이즈 보내기
    public Vector2 getCellsize()
    {
        SetCellSize();
        return new Vector2(cellWidth, cellHeight);
    }

    
    //좌하단 우상단 끝좌표 보내기
    public Vector2[] getMaxMinXY()
    {
        SetCellSize();
        maxXY = new Vector2(cellWidth * max + cellWidth / 2, cellHeight * max + cellHeight / 2);
        minXY = new Vector2(cellWidth * min - cellWidth / 2, cellHeight * min - cellHeight / 2);
        Vector2[] tmp = new Vector2[2];
        tmp[0] = minXY;
        tmp[1] = maxXY;

        Debug.Log(minXY + ", " + maxXY);

        return tmp;

    }

    //타일 깔기
    void BuildTilemap()
    {
        //타일 초기화
        tilemap.ClearAllTiles();

        foreach (var data in tileTable)
        {
            int x = data.Key.x;
            int y = data.Key.y;
            int type = data.Value;

            //타일 배치범위를 넘지 않았는가?
            if (x < min || x > max || y < min || y > max)
                continue;

            // 배치할 타일은 무슨 타일인가?
            TileBase tile = GetTile(type);
            // 배치할 타일이 없다면 패스
            if (tile == null) continue;

            //배치
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            tilemap.SetTile(cellPos, tile);
        }
    }





    //나중에 수정할 임시 펑션

    // 배치할 타일은 무슨 타일인가?
    TileBase GetTile(int type)
    {
        switch (type)
        {
            case 1: return groundTile;
            case 2: return mountainTile;
            case 3: return waterTile;
        }
        return null;
    }

    // 테이블 생성
    void GenerateTable()
    {
        for (int i = -29; i <= 30; i++)
        {
            for(int j = -29; j <= 30; j++)
            {
                tileTable[new Vector2Int(i, j)] = 1;
            }

        }

        for(int i=0; i < 30; i++)
        {
            int x = Random.Range(3, 9);
            int y = Random.Range(2, 8);
            tileTable[new Vector2Int(x, y)] = 2;

        }
    }
}
