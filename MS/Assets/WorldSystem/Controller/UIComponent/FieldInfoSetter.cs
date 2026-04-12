using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using UnityEngine.Tilemaps;

[HideInInspector]
public class FieldInfoSetter : MonoBehaviour
{

    //타일맵 UI
    private Dictionary<GridUIType, Tile> spriteTable;
    private Tilemap uiTilemap;
    [SerializeField]
    private List<Vector2Int> movePath;

    //타일맵 레이어 조정
    [Header("타일맵 레이어 Z 값")]
    [SerializeField]
    [Range(-10, 0)]
    private int zValue = -1;

    private void Awake()
    {
        spriteTable = new Dictionary<GridUIType, Tile>();
        movePath = new List<Vector2Int>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tilemap[] tilemaps = GameObject.FindWithTag("Map").GetComponentsInChildren<Tilemap>();
        foreach (var tmp in tilemaps)
        {
            if (tmp.name == "UITilemap")
            {
                uiTilemap = tmp;
                break;
            }
        }
        SetSpriteTable();

        //타일 UI 초기화
        uiTilemap.GetComponent<Transform>().Translate(new Vector3(0, 0, zValue));
        uiTilemap.ClearAllTiles();
    }

    //길 추가
    public void AddSelectedPathTile(Vector2Int xy)
    {
        movePath.Add(xy);
        Vector3Int tmp = new Vector3Int(xy.x, xy.y, zValue);
        uiTilemap.SetTile(tmp, spriteTable[GridUIType.Path]);

    }

    //중간 제거
    public void RemoveSelectedPathRangeTile(Vector2Int xy)
    {
        Vector3Int tmp;
        int maxIndex = movePath.Count;
        for (int i = movePath.IndexOf(xy); i < maxIndex; i++)
        {

            //앞선 경로에 지우려는 경로와 동일한 좌표가 있을 경우 패스
            //지우려는 경로 이후로 있는 모든 경로는 지우기
            if (movePath.IndexOf(movePath[i]) >= movePath.IndexOf(xy))
            {
                tmp = new Vector3Int(movePath[i].x, movePath[i].y, zValue);
                uiTilemap.SetTile(tmp, spriteTable[GridUIType.None]);
            }
        }

        movePath.RemoveRange(movePath.IndexOf(xy), movePath.Count - movePath.IndexOf(xy));
    }

    //특정 위치 제거
    public void RemoveSelectedPathTile(Vector2Int xy)
    {
        int i = movePath.IndexOf(xy);
        Vector3Int tmp = new Vector3Int(movePath[i].x, movePath[i].y, zValue);
        if (movePath.IndexOf(xy) == movePath.LastIndexOf(xy))
        {
            uiTilemap.SetTile(tmp, spriteTable[GridUIType.None]);
        }
        movePath.Remove(xy);
    }


#warning[UI 타일맵] UI 스프라이트 정리시 수정 필요
    //UI 수정 테이블
    private void SetSpriteTable()
    {
        Tile tmp = Resources.Load<Tile>("Tiles/UITile/None");
        spriteTable.Add(GridUIType.None, tmp);
        tmp = Resources.Load<Tile>("Tiles/UITile/Selected");
        spriteTable.Add(GridUIType.Path, tmp);
    }

}
