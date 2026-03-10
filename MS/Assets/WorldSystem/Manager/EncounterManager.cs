using UnityEngine;
using SystemDefinition;
using FieldDefinition;

public class EncounterManager : IManageSetterFunc, IEncounterFunc
{
    //외부 시스템 요소
    private ISystemFunc systemManager;
    private IMovementFuc movementController;
    
    void Start()
    {
        movementController.AddMovementEvent(EncounterCalc);
    }

    private void OnDestroy()
    {
        movementController.RemoveMovementEvent(EncounterCalc);
    }


    public override void SystemSetter(ISystemFunc system)
    {
        systemManager = system;
    }

    public override void MoveControlSetter(IMovementFuc movecontrol)
    {
        movementController = movecontrol;
    }


    private void EncounterCalc()
    {
        Debug.Log("인카운터 테스팅");
    }



}
