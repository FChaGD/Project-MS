using FieldDefinition;
using System;
using UnityEngine;

public class PartyFoodDeclarer : MonoBehaviour
{
    //외부 발생 이벤트
    public Action<int, int> foodUpdateAction;

    //파티 공용 정보
    private PartyInfo _partyInfo;

    //Food 제어 변수
    [SerializeField]
    private int systemMaxFood = int.MaxValue;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(PartyInfo partyInfo)
    {
        _partyInfo = partyInfo;

    }

    public void FoodUpdate(int addValue)
    {
        int beforeValue = _partyInfo.food;
        _partyInfo.food = Mathf.Clamp(_partyInfo.food + addValue, 0, _partyInfo.maxFood);
        if(beforeValue == _partyInfo.food)
        {
            return;
        }

        foodUpdateAction?.Invoke(_partyInfo.food, _partyInfo.maxFood);
        
    }

    public void FoodMaxUpdate(int addValue)
    {
        int beforeValue = _partyInfo.maxFood;
        _partyInfo.maxFood = Mathf.Clamp(_partyInfo.maxFood + addValue, 0, systemMaxFood);
        _partyInfo.food = Mathf.Clamp(_partyInfo.food, 0, _partyInfo.maxFood);


        if (beforeValue == _partyInfo.maxFood)
        {
            return;
        }

        foodUpdateAction?.Invoke(_partyInfo.food, _partyInfo.maxFood);
    }

}
