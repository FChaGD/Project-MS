using BattleDefinition;
using UnityEngine;

public class BattleTurnDeclarer : MonoBehaviour
{
    private BattleContainer _battleContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(BattleContainer battleContainer)
    {
        _battleContainer = battleContainer;
    }

}
