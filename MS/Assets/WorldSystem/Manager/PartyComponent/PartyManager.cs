using UnityEngine;
using CharacterDefinition;
using SystemDefinition;
using System.Collections.Generic;
using ItemDefinition;
using FieldDefinition;
using System;

[RequireComponent(typeof(PartyGoldDeclarer), typeof(PartyFoodDeclarer))]
public class PartyManager : IManageSetterFunc, IPartyFunc
{
    //하위 컴포넌트 변수
    private PartyGoldDeclarer goldDeclarer;
    private PartyFoodDeclarer foodDeclarer;


    //외부 컴포넌트 변수
    private IMovementFuc movementController;

    //파티 관리 정보
    [SerializeField]
    private PartyInfo _partyInfo;

    void Awake()
    {
        goldDeclarer = GetComponent<PartyGoldDeclarer>();
        foodDeclarer = GetComponent<PartyFoodDeclarer>();

        _partyInfo.crewList = new List<PartyStruct>();
        _partyInfo.equipableItemList = new List<EquipableItem>();
        _partyInfo.battleuseItemList = new List<BattleUseItem>();

#warning [파티] 플레이어 정보 불러오는 기능 필요

        goldDeclarer.Init(_partyInfo);
        foodDeclarer.Init(_partyInfo);
    }



    public override void MoveControlSetter(IMovementFuc movecontrol)
    {
        movementController = movecontrol;
    }

    //식량 및 골드 업데이트 함수 등록
    public void UpdateSet(Action<int, int> foodAction, Action<int> goldAction)
    {
        goldDeclarer.goldUpdateAction += goldAction;
        foodDeclarer.foodUpdateAction += foodAction;

        goldAction?.Invoke(_partyInfo.gold);
        foodAction?.Invoke(_partyInfo.food, _partyInfo.maxFood);

    }

    //식량 및 골드 업데이트 함수 해제
    public void UnBindUpdateSet(Action<int, int> foodAction, Action<int> goldAction)
    {
        goldDeclarer.goldUpdateAction -= goldAction;
        foodDeclarer.foodUpdateAction -= foodAction;
    }

}
