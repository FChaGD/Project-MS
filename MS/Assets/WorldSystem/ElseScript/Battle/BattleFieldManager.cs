using AYellowpaper.SerializedCollections;
using CharacterDefinition;
using NUnit.Framework;
using UnityEngine;

//전투 시작시 배경화면을 해상도에 맞춰 크기를 키우는 스크립트
public class BattleFieldManager : MonoBehaviour
{
    [Header("배경화면과 카메라")]
    [SerializeField]
    private SpriteRenderer background;
    [SerializeField]
    private Camera battleCamera;

    [Header("캐릭터 배치")]
    [SerializeField]
    [SerializedDictionary("포지션 번호", "배치 위치")]
    private SerializedDictionary<int, GameObject> playerPosition = new SerializedDictionary<int, GameObject>();
    [SerializeField]
    [SerializedDictionary("포지션 번호", "배치 위치")]
    private SerializedDictionary<int, GameObject> enemyPosition = new SerializedDictionary<int, GameObject>();

    void Awake()
    {
    }

    void Start()
    {
        SetFieldSize();
    }

    void OnEnable()
    {
        SetFieldSize();
    }

    public void SetFieldSize()
    {
#warning [배틀] 해상도 변경시 크기조절하는 함수 실행 이벤트 등록 필요
        float spritex = background.sprite.bounds.size.x;
        float spritey = background.sprite.bounds.size.y;

        Vector3 bottomLeft = battleCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = battleCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float screenX = topRight.x - bottomLeft.x;
        float screenY = topRight.y - bottomLeft.y;
        
        transform.localScale = new Vector3(screenX / spritex, screenY / spritey, transform.localScale.z);

        Debug.Log($"X 스케일 : {screenX / spritex}, Y 스케일 : {screenY / spritey}, 현재 스케일 {transform.localScale}");
    }

    //전장 세팅
    /// <summary>
    /// 전장 배경화면, 아군캐릭터 배치, 적군 캐릭터를 배치합니다.
    /// </summary>
    /// <param name="heroList">전장에 배치할 아군 캐릭터, PlayableCharacter 배열이여야 합니다.</param>
    public void SetBattleField(PlayableCharacter[] heroList)
    {
        SetBackgroundImage();
        CharacterAllClear();
        foreach (var hero in heroList)
        {
            SetPlayerCharacterOnBattleField(hero);
        }
    }

    private void SetBackgroundImage()
    {
#warning [배틀] 배경 이미지 설정하는 함수 구현 필요
    }

    private void CharacterAllClear()
    {
        foreach (var position in playerPosition)
        {
            position.Value.SetActive(false);
        }

        foreach (var position in enemyPosition)
        {
            position.Value.SetActive(false);

        }
    }

    private void SetPlayerCharacterOnBattleField(PlayableCharacter hero)
    {
        playerPosition[hero.position].SetActive(true);
    }
}
