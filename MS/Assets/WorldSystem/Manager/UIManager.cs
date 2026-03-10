using FieldDefinition;
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
    private List<Vector2Int> selectedPath;

    void Awake()
    {
        spriteTable = new Dictionary<UIType, Tile>();
        selectedPath = new List<Vector2Int>();
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
        Debug.Log($"UI 매니저 : {(tilemap == null ? "화면이 고정되었습니다." : "화면이 고정되지 않았습니다.")}");
        SetSpriteTable();

        //타일 UI 초기화
        tilemap.GetComponent<Transform>().Translate(new Vector3(0, 0, -5));
        tilemap.ClearAllTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //길 추가
    public void AddSelectedPathTile(Vector2Int xy)
    {
        selectedPath.Add(xy);
        Vector3Int tmp = new Vector3Int(xy.x, xy.y, -1);
        tilemap.SetTile(tmp, spriteTable[UIType.Path]);

    }

    //중간 제거
    public void RemoveSelectedPathRangeTile(Vector2Int xy)
    {
        Vector3Int tmp;
        for(int i = selectedPath.IndexOf(xy); i < selectedPath.Count; i++)
        {
            tmp = new Vector3Int(selectedPath[i].x, selectedPath[i].y, -1);
            tilemap.SetTile(tmp, spriteTable[UIType.None]);
        }

        selectedPath.RemoveRange(selectedPath.IndexOf(xy), selectedPath.Count - selectedPath.IndexOf(xy));
    }
    
    public void RemoveSelectedPathTile(Vector2Int xy)
    {
        int i = selectedPath.IndexOf(xy);
        Vector3Int tmp = new Vector3Int(selectedPath[i].x, selectedPath[i].y, -1);
        tilemap.SetTile(tmp, spriteTable[UIType.None]);
        selectedPath.Remove(xy);
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
