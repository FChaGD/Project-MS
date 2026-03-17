using System.Collections;
using SystemDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldCameraMovement : MonoBehaviour
{
    private CameraController cameraController;
    private CameraSettings _cameraSettings;

    //카메라와 카메라 목표
    private Camera selfCamera;
    private Transform cameraTransform;
    private Transform cameraTarget;

    //카메라 이동 여부
    private bool isFixedActivating = false;
    private Coroutine coroutine;

    //자유 카메라 위치
    [Header("자유 카메라 위치")]
    [SerializeField]
    private Vector3 freeCameraTransform;

    //드래그 이동 관련
    private bool isDragging = false;
    private Vector2 dragDirection;

    private void Awake()
    {
        selfCamera = GetComponent<Camera>();
        cameraTransform = GetComponent<Transform>();
        cameraController = GetComponent<CameraController>();

    }

    void Start()
    {

        //카메라 타겟 가져오기
        cameraTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        //카메라 드레그 검사
        if (!_cameraSettings.anchorSet && isFixedActivating == false)
        {
            CameraPushMove();
        }
    }

    public void Init(CameraSettings cameraSettings)
    {
        _cameraSettings = cameraSettings;
    }


    public void CameraUpdate()
    {
        //카메라 위치 업데이트
        if (_cameraSettings.anchorSet)
        {
            CameraFixedMove();
        }

    }

    public void OnCameraZoom()
    {
        selfCamera.orthographicSize = _cameraSettings.distance;
    }

    //카메라 고정 입력
    public void CameraAnchorSet()
    {

        //카메라 고정이 풀렸다면
        if (!_cameraSettings.anchorSet)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                isFixedActivating = false;
            }
            selfCamera.transform.position = freeCameraTransform;

        }
        //카메라 고정이 되었다면
        else
        {
            CameraFixedMove();
        }

    }

    // 드래그 클릭 여부 확인
    public void OnDragPress(bool buttonContext)
    {
        isDragging = buttonContext;
    }

    //마우스 드래그시 이동
    public void OnDragDelta(Vector2 input)
    {
        dragDirection = input;
        if (!_cameraSettings.anchorSet)
            CameraDragMove();
    }

    //플레이어 캐릭터에게 카메라 이동 입력
    public void OnFocusCamera()
    {
        selfCamera.transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, -10);
        freeCameraTransform = selfCamera.transform.position;
    }

    //고정 카메라 이동
    public void CameraFixedMove()
    {
        // 캐릭터 고정 이동
        if (!isFixedActivating)
        {
            coroutine = StartCoroutine(FixedLerpMove());
        }

    }

    private IEnumerator FixedLerpMove()
    {
        isFixedActivating = true;
        float cameraMoveDistance = Vector2.Distance(cameraTransform.position, cameraTarget.position);
        float duration = cameraMoveDistance / 20;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            yield return null;
            cameraMoveDistance = Vector2.Distance(cameraTransform.position, cameraTarget.position);
            duration = cameraMoveDistance / 20;
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            Vector3 cameraPosition = Vector3.Lerp(cameraTransform.position, new Vector3(cameraTarget.position.x, cameraTarget.position.y, -10), t);
            cameraTransform.position = cameraPosition;

        }
        isFixedActivating = false;
    }

    //카메라 경계를 넘어 마우스 커서 위치시 카메라 이동
    private void CameraPushMove()
    {
        //마우스 위치 잡기
        //마우스가 Null이면 화면 밀기 종료
        if (Mouse.current == null)
        {
            return;
        }
        Vector2 mousevalue = Mouse.current.position.value;
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(mousevalue);


        Vector2 moveDir = Vector2.zero;
        // 화면의 3%
        float edge = 0.03f;
        //좌우 검사
        if (mousePos.x <= edge)
        {
            moveDir.x = -1;
        }
        else if (mousePos.x >= 1f - edge)
        {
            moveDir.x = 1;
        }

        //상하 검사
        if (mousePos.y <= edge)
        {
            moveDir.y = -1;
        }
        else if (mousePos.y >= 1f - edge)
        {
            moveDir.y = 1;
        }

        if (moveDir == Vector2.zero)
        {
            //화면 경계 마우스로 화면 밀기 종료
            return;
        }

        //맵 경계 체크
        Vector2 direction = CameraPositionCheck(moveDir * Time.deltaTime, _cameraSettings.pushSpeed);

        transform.Translate(direction * _cameraSettings.pushSpeed, Space.World);

        ////자유 카메라 위치 저장
        freeCameraTransform = cameraTransform.position;

    }

    //카메라 드래그 이동
    private void CameraDragMove()
    {
        if (isDragging)
        {

            //맵 경계 체크
            dragDirection = CameraPositionCheck((-1) * dragDirection, _cameraSettings.dragSpeed);

            Vector2 move = dragDirection * _cameraSettings.dragSpeed;
            transform.Translate(move, Space.World);

            //자유 카메라 위치 저장
            freeCameraTransform = cameraTransform.position;

        }
    }

    //카메라 이동 가능 범위 제한
    private Vector2 CameraPositionCheck(Vector2 direction, float speed)
    {
        bool top = true;
        bool down = true;
        bool left = true;
        bool right = true;

        //좌우 확인
        if (cameraTransform.position.x + direction.x * speed < _cameraSettings.min.x + _cameraSettings.cameraHalfWidth - (_cameraSettings.cameraHalfWidth * 0.3))
        {
            //좌측이 경계에 닿았을 때
            left = false;
        }
        if (cameraTransform.position.x + direction.x * speed > _cameraSettings.max.x - _cameraSettings.cameraHalfWidth + (_cameraSettings.cameraHalfWidth * 0.3))
        {
            //우측이 경계에 닿았을 때
            right = false;
        }

        //상하 확인
        if (cameraTransform.position.y + direction.y * speed > _cameraSettings.max.y - _cameraSettings.cameraHalfHeight + (_cameraSettings.cameraHalfHeight * 0.3))
        {
            //상단이 경계에 닿았을 때
            top = false;
        }
        if (cameraTransform.position.y + direction.y * speed < _cameraSettings.min.y + _cameraSettings.cameraHalfHeight - (_cameraSettings.cameraHalfHeight * 0.3))
        {
            //하단이 경계에 닿았을 때
            down = false;
        }


        // 카메라 이동 금지 설정
        if (!top && direction.y > 0)
        {
            direction.y = 0;
        }
        if (!down && direction.y < 0)
        {
            direction.y = 0;
        }
        if (!left && direction.x < 0)
        {
            direction.x = 0;
        }
        if (!right && direction.x > 0)
        {
            direction.x = 0;
        }

        return direction;

    }

}
