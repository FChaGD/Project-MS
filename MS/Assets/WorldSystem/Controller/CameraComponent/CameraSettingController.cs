using SystemDefinition;
using UnityEngine;

public class CameraSettingController : MonoBehaviour
{
    private CameraController cameraController;
    private CameraSettings _cameraSettings;


    //카메라
    private Camera selfCamera;

    [Header("줌인, 줌아웃")]
    //카메라 줌인아웃 관련
    [SerializeField]
    private float maxZoom = 50.0f;
    [SerializeField]
    private float minZoom = 3.0f;
    [SerializeField]
    private float distance = 5.0f;
    [SerializeField]
    private float zoomSpeed = 1f;


    //카메라 고정 상태
    private bool anchorSet = false;


    //카메라 자율 이동
    [Header("카메라 드래그 속도")]
    [SerializeField]
    private float dragSpeed = 0.005f;
    [SerializeField]
    private float pushSpeed = 1000f;

    //카메라 월드 경계 정하기
    private Vector2 max;
    private Vector2 min;
    private float cameraHalfHeight;
    private float cameraHalfWidth;

    private void Awake()
    {
        selfCamera = GetComponent<Camera>();
    }

    void Start()
    {
        cameraController = GetComponent<CameraController>();

        CameraSet();

        //카메라 드래그 이동 및 화면 밀기 이동 속도 재조정
        dragSpeed = Mathf.Lerp(0.005f, 0.1f, (distance - minZoom) / (maxZoom - minZoom));
        pushSpeed = Mathf.Lerp(5f, 150f, (distance - minZoom) / (maxZoom - minZoom));
        _cameraSettings.dragSpeed = dragSpeed;
        _cameraSettings.pushSpeed = pushSpeed;
    }
    public void Init(CameraSettings cameraSettings)
    {
        _cameraSettings = cameraSettings;
    }

    //세계의 경계 받아오기
    public void MinMaxSet(Vector2[] minmax)
    {
        min = minmax[0];
        max = minmax[1];

        _cameraSettings.min = min;
        _cameraSettings.max = max;
    }

    //줌 카메라 정보 입력
    public void OnCameraZoom(Vector2 scroll)
    {
        //카메라 거리정보 수정
        _cameraSettings.distance = Mathf.Clamp(_cameraSettings.distance - scroll.y * _cameraSettings.zoomSpeed, _cameraSettings.minZoom, _cameraSettings.maxZoom);

        //카메라 드래그 이동 및 화면 밀기 이동 속도 재조정
        _cameraSettings.dragSpeed = Mathf.Lerp(0.005f, 0.1f, (_cameraSettings.distance - _cameraSettings.minZoom) / (_cameraSettings.maxZoom - _cameraSettings.minZoom));
        _cameraSettings.pushSpeed = Mathf.Lerp(5f, 150f, (_cameraSettings.distance - _cameraSettings.minZoom) / (_cameraSettings.maxZoom - _cameraSettings.minZoom));


    }

    //카메라 폭 넓이 체크
    public void CameraSizeCheck()
    {
        cameraHalfHeight = selfCamera.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * selfCamera.aspect;

        _cameraSettings.cameraHalfHeight = cameraHalfHeight;
        _cameraSettings.cameraHalfWidth = cameraHalfWidth;
    }

    //카메라 고정 입력
    public void CameraAnchorSet()
    {
        // 카메라 고정 전환
        _cameraSettings.anchorSet = !_cameraSettings.anchorSet;

    }

    private void CameraSet()
    {
        //카메라 줌인아웃 관련
        _cameraSettings.maxZoom = maxZoom;
        _cameraSettings.minZoom = minZoom;
        _cameraSettings.distance = distance;
        _cameraSettings.zoomSpeed = zoomSpeed;


        //카메라 고정 상태
        _cameraSettings.anchorSet = anchorSet;


        //카메라 자율 이동
        _cameraSettings.dragSpeed = dragSpeed;
        _cameraSettings.pushSpeed = pushSpeed;

        //카메라 월드 경계 정하기
        _cameraSettings.max = max;

        _cameraSettings.min = min;
        _cameraSettings.cameraHalfHeight = cameraHalfHeight;
        _cameraSettings.cameraHalfWidth = cameraHalfWidth;
    }


}
