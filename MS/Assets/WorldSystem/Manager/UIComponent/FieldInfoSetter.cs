using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using UnityEngine.Tilemaps;

[HideInInspector]
public class FieldInfoSetter : MonoBehaviour
{

    //타일맵 UI
    private Dictionary<UIType, Tile> spriteTable;
    private Tilemap uiTilemap;
    private List<Vector2Int> movePath;

    private void Awake()
    {
        spriteTable = new Dictionary<UIType, Tile>();
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
        uiTilemap.GetComponent<Transform>().Translate(new Vector3(0, 0, -5));
        uiTilemap.ClearAllTiles();
    }

    //길 추가
    public void AddSelectedPathTile(Vector2Int xy)
    {
        movePath.Add(xy);
        Vector3Int tmp = new Vector3Int(xy.x, xy.y, -1);
        uiTilemap.SetTile(tmp, spriteTable[UIType.Path]);

    }

    //중간 제거
    public void RemoveSelectedPathRangeTile(Vector2Int xy)
    {
        Vector3Int tmp;
        for (int i = movePath.IndexOf(xy); i < movePath.Count; i++)
        {
            tmp = new Vector3Int(movePath[i].x, movePath[i].y, -1);
            uiTilemap.SetTile(tmp, spriteTable[UIType.None]);
        }

        movePath.RemoveRange(movePath.IndexOf(xy), movePath.Count - movePath.IndexOf(xy));
    }

    //특정 위치 제거
    public void RemoveSelectedPathTile(Vector2Int xy)
    {
        int i = movePath.IndexOf(xy);
        Vector3Int tmp = new Vector3Int(movePath[i].x, movePath[i].y, -1);
        if (movePath.IndexOf(xy) == movePath.LastIndexOf(xy))
        {
            uiTilemap.SetTile(tmp, spriteTable[UIType.None]);
        }
        movePath.Remove(xy);
    }


#warning[UI 타일맵] UI 스프라이트 정리시 수정 필요
    //UI 수정 테이블
    private void SetSpriteTable()
    {
        Tile tmp = Resources.Load<Tile>("Tiles/UITile/None");
        spriteTable.Add(UIType.None, tmp);
        tmp = Resources.Load<Tile>("Tiles/UITile/Selected");
        spriteTable.Add(UIType.Path, tmp);
    }

}
