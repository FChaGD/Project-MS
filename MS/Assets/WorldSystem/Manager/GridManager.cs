using FieldDefinition;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour, IGridFunc
{
    private Grid grid;
    private Tilemap tilemap;

    //셀 사이즈
    private float cellWidth;
    private float cellHeight;

    //좌하단 우상단 좌표 = 최대최소 좌표
    private Vector2 maxXY;
    private Vector2 minXY;

    // 좌표 범위
    [Header("Tile Range")]
    [SerializeField]
    private int min;
    [SerializeField]
    private int max;
    public int Min => min;
    public int Max => max;

    // 테이블 데이터 (예시)
    public Dictionary<Vector2Int, TileType> typeTable = new Dictionary<Vector2Int, TileType>();
    public Dictionary<TileType, Tile> spriteTable = new Dictionary<TileType, Tile>();

    void Awake()
    {
        GameObject target = GameObject.FindWithTag("Map");
        grid = target.GetComponent<Grid>();
        Tilemap[] tilemaps = grid.GetComponentsInChildren<Tilemap>();
        foreach (var tmp in tilemaps)
        {
            if (tmp.name == "WorldTilemap")
            {
                tilemap = tmp;
                break;
            }
        }
        cellWidth = 0;
        cellHeight = 0;
    }

    void Start()
    {
        //타일 생성을 위한 정보 로드
        SetCellSize();
        SetSpriteTable();

        //타일 초기화
        tilemap.GetComponent<Transform>().Translate(new Vector3(0, 0, 0));
        tilemap.ClearAllTiles();

        //타일 배치
        GenerateTable();
        BuildTilemap();
    }

    //셀사이즈 계산후 저장
    private void SetCellSize()
    {
        cellWidth = grid.cellSize.x;
        cellHeight = grid.cellSize.y;
    }



    //타일 깔기
    void BuildTilemap()
    {
        foreach (var data in typeTable)
        {
            int x = data.Key.x;
            int y = data.Key.y;
            TileType type = data.Value;

            //타일 배치범위를 넘지 않았는가?
            if (x < min || x > max || y < min || y > max)
                continue;

            Tile tile = spriteTable[type];

            //배치
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            tilemap.SetTile(cellPos, tile);
        }
    }

    // 타일별 정보 기재
    private void SetSpriteTable()
    {
        Tile tmp = Resources.Load<Tile>("Tiles/WorldTile/들판");
        spriteTable.Add(TileType.Field, tmp);
        tmp = Resources.Load<Tile>("Tiles/WorldTile/산맥");
        spriteTable.Add(TileType.Mountain, tmp);
    }

    //좌하단 우상단 끝좌표 보내기
    public Vector2[] GetMaxMinXY()
    {
        SetCellSize();
        maxXY = new Vector2(cellWidth * max + cellWidth, cellHeight * max + cellHeight);
        minXY = new Vector2(cellWidth * min, cellHeight * min);
        Vector2[] tmp = new Vector2[2];
        tmp[0] = minXY;
        tmp[1] = maxXY;

        Debug.Log(minXY + ", " + maxXY);

        return tmp;

    }

    //월드 좌표의 타일 반환
    public Vector2Int GetTileVectorAtWorldPosition(Vector3 worldPos)
    {
        Vector3Int cellPos = grid.WorldToCell(worldPos);
        //타일맵에서 타일 좌표로 타일 받아오기
        //tilemap.GetTile(cellPos)
        return new Vector2Int(cellPos.x, cellPos.y);
    }

    //셀 사이즈 보내기
    public Vector2 GetCellsize()
    {
        SetCellSize();
        return new Vector2(cellWidth, cellHeight);
    }
    
    //셀의 중심좌표 보내기
    public Vector3 GetCenter(Vector2Int cellPosition)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(cellPosition.x, cellPosition.y, 0));
    }


    //나중에 수정할 임시 펑션
    // 테이블 생성
    void GenerateTable()
    {
        for (int i = min; i <= max; i++)
        {
            for (int j = min; j <= max; j++)
            {
                typeTable[new Vector2Int(i, j)] = TileType.Field;
            }

        }

        for (int i = 0; i < 30; i++)
        {
            int x = Random.Range(3, 9);
            int y = Random.Range(2, 8);
            typeTable[new Vector2Int(x, y)] = TileType.Mountain;

        }
    }

    /*//타일 경계 그리기
    private void OnDrawGizmos()
    {
        if (tilemap == null) return;

        Gizmos.color = Color.black;

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            Vector3 world = tilemap.GetCellCenterWorld(pos);
            Vector3 size = tilemap.layoutGrid.cellSize;

            Gizmos.DrawWireCube(world, size);
        }

    }*/

}
