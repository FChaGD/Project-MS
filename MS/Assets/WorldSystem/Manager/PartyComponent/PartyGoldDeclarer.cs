using FieldDefinition;
using System;
using UnityEngine;

public class PartyGoldDeclarer : MonoBehaviour
{
    //외부 발생 이벤트
    public Action<int> goldUpdateAction;

    //파티 공용 정보
    private PartyInfo _partyInfo;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Init(PartyInfo partyInfo)
    {
        _partyInfo = partyInfo;
    }

    public void GoldUpdate(int addValue)
    {
        int beforeValue = _partyInfo.gold;
        _partyInfo.gold = Mathf.Max(0, _partyInfo.gold + addValue);
        if (beforeValue == _partyInfo.gold)
        {
            return;
        }
        goldUpdateAction?.Invoke(_partyInfo.gold);
    }

}
