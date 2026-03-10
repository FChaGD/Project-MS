using FieldDefinition;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;

public class DataManager : IManageSetterFunc
{
    private Dictionary<Vector2Int, FieldData> fieldData;
    private Vector2Int playerPosition;

    public IGridFunc gridManager;

    void Awake()
    {
        fieldData = new Dictionary<Vector2Int, FieldData>();
        playerPosition = new Vector2Int();
    }
    
    //그리드매니저 받아오기
    public override void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
            for (int i = -29; i <= 30; ++i)
            {
                for (int j = -29; j <= 30; ++j)
                {
                    fieldData[new Vector2Int(i, j)] = new FieldData();
                }
            }
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
