using UnityEngine;
using CharacterDefinition;
using TMPro;
using System.Text;

public class BattleCharacterContainer : MonoBehaviour
{
    #region 변수 생성 구역
    private CharacterBase characterInfo;

    [SerializeField]
    private GameObject characterAsset;
    [SerializeField]
    private GameObject hpInfo;
    [SerializeField]
    private GameObject resourceInfo;

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
    
    public void SetCharacterInfo(CharacterBase character)
    {
        characterInfo = character;
        SetVisualInfo();
    }

    #endregion

    #region private 생성 구역
    private void SetVisualInfo()
    {
        Debug.Log($"체력 정보 : {characterInfo.curhp}/{characterInfo.characterDataTable.characterData.hpMax}");
        StringBuilder hptext = new StringBuilder();
        hptext.Append(characterInfo.curhp);
        hptext.Append("/");
        hptext.Append(characterInfo.characterDataTable.characterData.hpMax);
        if(hpInfo.GetComponent<TextMeshPro>() == null)
        {
            Debug.LogError("체력 정보 텍스트 컴포넌트가 없습니다.");
            return;
        }
        hpInfo.GetComponent<TextMeshPro>().text = hptext.ToString();


        StringBuilder resourceText = new StringBuilder();
        resourceText.Append(characterInfo.curmp);
        resourceText.Append("/");
        resourceText.Append(characterInfo.characterDataTable.characterData.mpMax);
        resourceInfo.GetComponent<TextMeshPro>().text = resourceText.ToString();
    }
    #endregion

}
