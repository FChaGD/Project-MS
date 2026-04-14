using SystemDefinition;
using UnityEngine;

public class BattleUISetter : BaseUIControl
{
    [SerializeField]
    private GameObject battleUIPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void UIOff()
    {
        battleUIPanel.SetActive(false);
    }

    public override void UIOn()
    {
        battleUIPanel.SetActive(true);
    }


}
