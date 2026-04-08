using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    private SpriteRenderer background;
    private Camera battleCamera;

    void Awake()
    {
        background = GetComponent<SpriteRenderer>();
        battleCamera = GameObject.FindWithTag("BattleCamera").GetComponent<Camera>();
    }

    void Start()
    {
        SetFieldSize();
    }

    void OnEnable()
    {
        Debug.Log($"OnEnable 시작");
        SetFieldSize();
    }

    public void SetFieldSize()
    {
        float spritex = background.sprite.bounds.size.x;
        float spritey = background.sprite.bounds.size.y;

        Vector3 bottomLeft = battleCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = battleCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float screenX = topRight.x - bottomLeft.x;
        float screenY = topRight.y - bottomLeft.y;
        
        transform.localScale = new Vector3(screenX / spritex, screenY / spritey, transform.localScale.z);

        Debug.Log($"X 스케일 : {screenX / spritex}, Y 스케일 : {screenY / spritey}, 현재 스케일 {transform.localScale}");
    }
}
