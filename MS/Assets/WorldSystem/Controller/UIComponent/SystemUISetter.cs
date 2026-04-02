using System;
using System.Collections.Generic;
using SystemDefinition;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;
using System.Text;

public class SystemUISetter : BaseUIControl
{
    //UI 객체들
    [Header("캔버스 객체들")]
    [SerializeField]
    private GameObject SystemUIPanel;
    [SerializeField]
    private GameObject TabPanel;
    [SerializeField]
    private GameObject TabInfoPanel;

    //캔버스 제어객체
    [Header("탭 등록 프리팹")]
    [SerializeField] private SystemTabEntry[] tabEntries;

    //지연 생성 딕셔너리
    private Dictionary<Type, GameObject> beforeInstante = new();
    [SerializeField]
    private SerializedDictionary<Type, GameObject> instantedTab = new();

    //현재 탭
    private GameObject curTab;
    private Type curTabType;

    void Awake()
    {

        // 프리팹 매핑만 등록 (생성 X)
        foreach (var entry in tabEntries)
            beforeInstante[entry.tabType.GetType()] = entry.tabPrefeb;
        curTabType = typeof(TabType);
    }

    void Start()
    {
        //탭 버튼 리스너 등록
        Button[] buttons = TabPanel.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            // 버튼 이름 기반으로 탭 타입 매핑
            StringBuilder typeName = new StringBuilder(button.name.Replace("Button", "Type, Assembly-CSharp")); // ex) InventoryTabButton → InventoryType
            typeName.Insert(0, "SystemDefinition.");

            //각 탭 버튼에 리스너 등록
            Type tabType = Type.GetType(typeName.ToString());
            if (tabType != null)
            {
                button.onClick.AddListener(() => TabFocus(tabType));
            }


        }

    }


    //UI 켜기
    public override void UIOn()
    {
        SystemUIPanel.SetActive(true);
    }

    //UI 끄기
    public override void UIOff()
    {
        SystemUIPanel.SetActive(false);
        curTabType = typeof(TabType);
    }

    //UI가 켜져 있는지 반환
    public override bool IsActive()
    {
        return SystemUIPanel.activeInHierarchy;
    }

    public void TabFocus(Type focusTab)
    {
        //현재 요청된 탭이 이미 포커스되어 있다면 메서드 종료
        if (curTabType.GetType() == focusTab) return;

        // 이전 탭 비활성화
        curTab?.SetActive(false);

        // 이미 생성된 경우 탭 전환
        if (instantedTab.TryGetValue(focusTab.GetType(), out var instanted))
        {
            curTab = instanted;
            curTabType = focusTab;
            curTab?.SetActive(true);
            return;
        }

        // 프리팹 존재 확인
        if (!beforeInstante.TryGetValue(focusTab, out var prefab))
        {
            Debug.LogError($"등록되지 않은 탭 타입: {focusTab.Name}");
            return;
        }

        // 최초 요청 시 생성
        instantedTab[focusTab] = Instantiate(beforeInstante[focusTab], TabInfoPanel.transform, false); ;
        curTab = instantedTab[focusTab];
        curTabType = focusTab;

    }

    public Type GetCurTabType()
    {
        return curTabType;
    }

}
