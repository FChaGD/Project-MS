using BattleDefinition;
using CharacterDefinition;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터 행동 순서를 정렬하는 하위 컴포넌트 클래스
/// </summary>
public class BattleActOrderer : MonoBehaviour
{
    #region 변수 생성 구역
    //배틀 컨트롤러 정보 컨테이너
    private BattleContainer _battleContainer;

    private List<BattleOrderContainer> _battleOrderContainerList = new List<BattleOrderContainer>();

    #endregion

    #region 유니티 라이프사이클 관련

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    #endregion

    #region public 생성 구역

    public void Init(BattleContainer battleContainer)
    {
        _battleContainer = battleContainer;
    }

    /// <summary>
    /// 전투 화면에서 캐릭터 행동 순서를 UI에 업데이트하는 함수
    /// </summary>
    public void UpdateCharacterOrderUIInfo()
    {
#warning [배틀] 캐릭터 행동 UI 추가 및 업데이트 구현 필요
    }

    public void SetAutoCharacterOrder()
    {
        ClearCharacterOrderList();
        foreach (var character in _battleContainer.heroList)
        {
            BattleOrderContainer orderContainer = new BattleOrderContainer();
            orderContainer.orderCharacter = character;
            orderContainer.orderAction = "DefaultAction"; // 기본 행동 설정
            _battleOrderContainerList.Add(orderContainer);
        }
        foreach (var character in _battleContainer.enemyList)
        {
            BattleOrderContainer orderContainer = new BattleOrderContainer();
            orderContainer.orderCharacter = character;
            orderContainer.orderAction = "DefaultAction"; // 기본 행동 설정
            _battleOrderContainerList.Add(orderContainer);
        }

#warning [배틀] 행동 순서를 정렬하는 로직 추가 필요

    }


    #endregion

    #region private 생성 구역

    private void ClearCharacterOrderList()
    {
        _battleOrderContainerList.Clear();
    }

    #endregion

}
