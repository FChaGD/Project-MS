using FieldDefinition;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(WorldCameraMovement))]
[RequireComponent(typeof(CameraSettingController))]
public class CameraController : IManageSetterFunc, ICameraFunc
{
    //카메라 부가 컴포넌트 변수
    WorldCameraMovement worldCameraMovement;
    CameraSettingController cameraSettingController;


    //외부 참조 요소
    private IGridFunc gridManager;

    //카메라 세팅 정보
    private CameraSettings cameraSettings;


    void Awake()
    {
        worldCameraMovement = GetComponent<WorldCameraMovement>();
        cameraSettingController = GetComponent<CameraSettingController>();

        cameraSettings = new CameraSettings();
        worldCameraMovement.Init(cameraSettings);
        cameraSettingController.Init(cameraSettings);

    }

    //그리드매니저 받아오기
    public override void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        cameraSettingController.CameraSizeCheck();

        //타일 사이즈 가져오기
        Vector2[] minmax = gridManager.GetMaxMinXY();
        cameraSettingController.MinMaxSet(minmax);
        worldCameraMovement.CameraFixedMove();

        

    }


    public void CameraUpdate()
    {
#warning [미완성] 배틀 카메라 추가시 수정 필요
        worldCameraMovement.CameraUpdate();

    }
    

    //카메라 줌인아웃 입력
    public void OnCameraZoom(InputAction.CallbackContext context)
    {

#warning [미완성] 시스템 상태에 따른 대책 정의 필요
        //마우스 휠 입력을 기반으로 세팅 정보 수정
        Vector2 scroll = context.ReadValue<Vector2>();
        cameraSettingController.OnCameraZoom(scroll);
        //현재 카메라 높이 너비 구하기
        cameraSettingController.CameraSizeCheck();
        

        //수정된 정보 기반으로 카메라 조정
#warning [미완성] 배틀 카메라 추가시 수정필요
        worldCameraMovement.OnCameraZoom();
        cameraSettingController.CameraSizeCheck();



    }

    //카메라 고정 입력
    public void CameraAnchorSet(InputAction.CallbackContext context)
    {

#warning [미완성] 시스템 상태에 따른 대책 정의 필요
        //입력 실행중 확인
        if (!context.performed) return;

        // 카메라 고정 전환
        cameraSettingController.CameraAnchorSet();

        //카메라 변경
        worldCameraMovement.CameraAnchorSet();

    }

    // 드래그이동 클릭 입력
    public void OnDragPress(InputAction.CallbackContext context)
    {
#warning [미완성] 시스템 상태에 따른 대책 정의 필요
        worldCameraMovement.OnDragPress(context.ReadValueAsButton());
    }

    //마우스 드래그 입력
    public void OnDragDelta(InputAction.CallbackContext context)
    {
#warning [미완성] 시스템 상태에 따른 대책 정의 필요
        worldCameraMovement.OnDragDelta(context.ReadValue<Vector2>());
    }

    //플레이어 캐릭터에게 카메라 이동 입력
    public void OnFocusCamera(InputAction.CallbackContext context)
    {
#warning [미완성] 시스템 상태에 따른 대책 정의 필요
        worldCameraMovement.OnFocusCamera();
    }

    
}
