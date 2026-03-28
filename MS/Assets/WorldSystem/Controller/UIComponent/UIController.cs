using FieldDefinition;
using System.Collections;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : IManageSetterFunc, IUIFunc
{
    //내부 UI 컴포넌트 변수
    private FieldInfoSetter fieldInfoSetter;
    private WorldUISetter worldUISetter;

    //외부 컨트롤러 매니저 변수
    private IGridFunc gridManager;
    private IMovementFuc movementController;
    private IPartyFunc partyManager;

    void Awake()
    {
        fieldInfoSetter = GetComponent<FieldInfoSetter>();
        worldUISetter = GetComponent<WorldUISetter>();
    }
    
    //그리드매니저 불러오기
    public override void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    //무브먼트 컨트롤러 불러오기
    public override void MoveControlSetter(IMovementFuc movecontrol)
    {
        movementController = movecontrol;
    }

    //파티 매니저 불러오기
    public override void PartySetter(IPartyFunc party)
    {
        partyManager = party;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitUntil(() => gridManager != null && movementController != null && partyManager != null);

        //WorldUI HUD 업데이트 함수 등록
        partyManager.UpdateSet(worldUISetter.UpdateFood, worldUISetter.UpdateGold);
        movementController.UpdateSet(worldUISetter.UpdateSpeed);

    }

    void OnDestroy()
    {
        //WorldUI HUD 업데이트 함수 해제
        partyManager.UnBindUpdateSet(worldUISetter.UpdateFood, worldUISetter.UpdateGold);
        movementController.UnBindUpdateSet(worldUISetter.UpdateSpeed);
    }

    //길 추가
    public void AddSelectedPathTile(Vector2Int xy)
    {
        fieldInfoSetter.AddSelectedPathTile(xy);

    }

    //중간 제거
    public void RemoveSelectedPathRangeTile(Vector2Int xy)
    {
        fieldInfoSetter.RemoveSelectedPathRangeTile(xy);
    }
    
    //특정 위치 제거
    public void RemoveSelectedPathTile(Vector2Int xy)
    {
        fieldInfoSetter.RemoveSelectedPathTile(xy);
    }

    //경로 분석 UI toggle 입력
    public void PathCheckUIToggle(InputAction.CallbackContext context)
    {

        //중복실행 방지
        if (!context.performed) return;

        worldUISetter.PathCheckUIToggle();


    }



}
