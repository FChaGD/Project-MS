using FieldDefinition;
using NUnit.Framework;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Tilemaps;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UIManager : MonoBehaviour, IUIFunc
{
    private IGridFunc gridManager;

    //타일맵 UI
    private Dictionary<UIType, Tile> spriteTable;
    private Tilemap tilemap;
    private List<Vector2Int> movePath;

    void Awake()
    {
        spriteTable = new Dictionary<UIType, Tile>();
        movePath = new List<Vector2Int>();
    }
    
    //그리드매니저 불러오기
    public void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tilemap[] tilemaps = GameObject.FindWithTag("Map").GetComponentsInChildren<Tilemap>();
        foreach (var tmp in tilemaps)
        {
            if (tmp.name == "UITilemap")
            {
                tilemap = tmp;
                break;
            }
        }
        SetSpriteTable();

        //타일 UI 초기화
        tilemap.GetComponent<Transform>().Translate(new Vector3(0, 0, -5));
        tilemap.ClearAllTiles();
    }

    //길 추가
    public void AddSelectedPathTile(Vector2Int xy)
    {
        movePath.Add(xy);
        Vector3Int tmp = new Vector3Int(xy.x, xy.y, -1);
        tilemap.SetTile(tmp, spriteTable[UIType.Path]);

    }

    //중간 제거
    public void RemoveSelectedPathRangeTile(Vector2Int xy)
    {
        Vector3Int tmp;
        for(int i = movePath.IndexOf(xy); i < movePath.Count; i++)
        {
            tmp = new Vector3Int(movePath[i].x, movePath[i].y, -1);
            tilemap.SetTile(tmp, spriteTable[UIType.None]);
        }

        movePath.RemoveRange(movePath.IndexOf(xy), movePath.Count - movePath.IndexOf(xy));
    }
    
    //특정 위치 제거
    public void RemoveSelectedPathTile(Vector2Int xy)
    {
        int i = movePath.IndexOf(xy);
        Vector3Int tmp = new Vector3Int(movePath[i].x, movePath[i].y, -1);
        if(movePath.IndexOf(xy) == movePath.LastIndexOf(xy))
        {
            tilemap.SetTile(tmp, spriteTable[UIType.None]);
        }
        movePath.Remove(xy);
    }

    //나중에 수정할 펑션
    //UI 수정 테이블
    private void SetSpriteTable()
    {
        Tile tmp = Resources.Load<Tile>("Tiles/UITile/None");
        spriteTable.Add(UIType.None, tmp);
        tmp = Resources.Load<Tile>("Tiles/UITile/Selected");
        spriteTable.Add(UIType.Path, tmp);
    }

}
