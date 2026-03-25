using FieldDefinition;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour, IUIFunc
{
    //내부 UI 컴포넌트 변수
    private FieldInfoSetter fieldInfoSetter;
    private WorldUISetter worldUISetter;

    //외부 컨트롤러 매니저 변수
    private IGridFunc gridManager;

    void Awake()
    {
        fieldInfoSetter = GetComponent<FieldInfoSetter>();
    }
    
    //그리드매니저 불러오기
    public void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
