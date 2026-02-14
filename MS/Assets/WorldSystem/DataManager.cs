using FieldDefinition;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Dictionary<Vector2Int, FieldData> fieldData;
    private Vector2 playerPosition;

    public GridManager gridManager;

    void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
            fieldData = new Dictionary<Vector2Int, FieldData>();
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
