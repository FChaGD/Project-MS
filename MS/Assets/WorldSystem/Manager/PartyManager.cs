using UnityEngine;
using CharacterDefinition;
using SystemDefinition;
using System.Collections.Generic;
using ItemDefinition;

public class PartyManager : IManageSetterFunc
{
    private List<PartyStruct> crewList;
    private List<EquipableItem> equipableItemList;
    private List<BattleUseItem> battleuseItemList;
    private int gold;
    private int maxFood;
    private int food;



    void Start()
    {
        crewList = new List<PartyStruct>();
        equipableItemList = new List<EquipableItem>();
        battleuseItemList = new List<BattleUseItem>();
        gold = 0;
        food = 0;
    }



}
