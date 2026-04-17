using BattleDefinition;
using UnityEngine;

public class BattleTurnDeclarer : MonoBehaviour
{
    private BattleContainer _battleContainer;
    
    void Start()
    {
        
    }

    public void Init(BattleContainer battleContainer)
    {
        _battleContainer = battleContainer;
    }

}
