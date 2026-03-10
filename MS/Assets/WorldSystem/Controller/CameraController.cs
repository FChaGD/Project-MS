using FieldDefinition;
using System.Collections;
using SystemDefinition;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class CameraController : IManageSetterFunc
{
    //카메라와 카메라 목표
    public Camera selfCamera;
    public Transform cameraTarget;

    //외부 참조 요소
    private IGridFunc gridManager;


    //카메라의 위치
    private Transform cameraTransform;

    [Header("줌인, 줌아웃")]
    //카메라 줌인아웃 관련
    [SerializeField]
    private float maxZoom = 30.0f;
    [SerializeField]
    private float minZoom = 3.0f;
    [SerializeField]
    private float distance = 5.0f;
    [SerializeField]
    private float zoomSpeed = 2.0f;

    //카메라 고정 상태
    private bool isFixedActivating = false;
    private Coroutine coroutine;
    private bool anchorSet = false;
    [SerializeField]
    private Vector3 freeCameraTransform;

    //카메라 자율 이동
    [Header("카메라 드래그 속도")]
    [SerializeField]
    private float dragSpeed = 0.02f;
    [SerializeField]
    private float pushSpeed = 1000f;
    private bool isDragging = false;
    private bool isPushing;
    private Vector2 dragDirection;

    //카메라 월드 경계 정하기
    private Vector2 max;
    private Vector2 min;
    private float halfHeight;
    private float halfWidth;
    private bool topdown = true;
    private bool leftright = true;

    void Awake()
    {
        selfCamera = GetComponent<Camera>();
        cameraTransform = gameObject.GetComponent<Transform>();
        CameraSizeCheck();
        freeCameraTransform = cameraTransform.position;
    }

    //그리드매니저 받아오기
    public override void GridSetter(IGridFunc grid)
    {
        gridManager = grid;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //타일 사이즈 가져오기
        Vector2[] minmax = gridManager.getMaxMinXY();
        min = minmax[0];
        max = minmax[1];
        CameraFixedMove();

        //카메라 타겟 가져오기
        cameraTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        //카메라 위치 업데이트
        if (anchorSet)
        {
            CameraFixedMove();
        }
        else
        {
            CameraFreeMove();
        }

    }

    //자유 카메라 이동
    private void CameraFreeMove()
    {
        CameraPushMove();
        if (!isPushing)
        {
            CameraDragMove();
        }
    }

    //카메라 줌인아웃 조작
    public void OnCameraZoom(InputAction.CallbackContext context)
    {
        Vector2 scroll = context.ReadValue<Vector2>();
        distance = Mathf.Clamp(distance - scroll.y * zoomSpeed, minZoom, maxZoom);
        selfCamera.orthographicSize = distance;

        //현재 카메라 높이 너비 구하기
        CameraSizeCheck();

    }

    //카메라 폭 넓이 체크
    private void CameraSizeCheck()
    {
        halfHeight = selfCamera.orthographicSize;
        halfWidth = halfHeight * selfCamera.aspect;
    }

    //카메라 고정 확인
    public void CameraAnchorSet(InputAction.CallbackContext context)
    {
        //입력 실행중 확인
        if (!context.performed) return;

        // 카메라 고정 전환
        anchorSet = !anchorSet;


        Debug.Log(anchorSet);
        
        //카메라 고정이 풀렸다면
        if (!anchorSet)
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
                isFixedActivating = false;
            }
            selfCamera.transform.position = freeCameraTransform;
        }

    }

    // 드래그 클릭 여부 확인
    public void OnDragPress(InputAction.CallbackContext context)
    {
        isDragging = context.ReadValueAsButton();
    }

    //마우스 이동량 저장
    public void OnDragDelta(InputAction.CallbackContext context)
    {
        dragDirection = context.ReadValue<Vector2>();
    }

    //플레이어 캐릭터에게 카메라 이동
    public void OnFocusCamera(InputAction.CallbackContext context)
    {
        selfCamera.transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, -10);
        freeCameraTransform = selfCamera.transform.position;
    }

    //고정 카메라 이동
    private void CameraFixedMove()
    {
        // 캐릭터 고정 이동
        if (!isFixedActivating)
        {
            coroutine = StartCoroutine(LerpMove());
        }

    }

    private IEnumerator LerpMove()
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

    //카메라 경계와 마우스 커서 일치 카메라 이동
    private void CameraPushMove()
    {
        isPushing = false;

        if (Mouse.current == null) return;
        Vector2 mousevalue = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(mousevalue);

        Vector3 moveDir = Vector3.zero;
        // 화면의 3%
        float edge = 0.03f; 
        //좌우 검색
        if (mousePos.x <= edge)
        {
            moveDir.x = -1;
            isPushing = true;
        }
        else if (mousePos.x >= 1f - edge)
        {
            moveDir.x = 1;
            isPushing = true;
        }


        if (mousePos.y <= edge)
        {
            moveDir.y = -1;
            isPushing = true;
        }
        else if (mousePos.y >= 1f - edge)
        {
            moveDir.y = 1;
            isPushing = true;
        }

        if (moveDir == Vector3.zero)
            return;

        //맵 경계 체크
        float xmove = moveDir.x * Time.deltaTime;
        float ymove = moveDir.y * Time.deltaTime;

        CameraPositionCheck(ref xmove, ref ymove,  moveDir * Time.deltaTime, pushSpeed);
        transform.Translate(new Vector3(xmove, ymove,0)* pushSpeed, Space.World);

        ////자유 카메라 위치 저장
        freeCameraTransform = cameraTransform.position;

    }


    //카메라 드래그 이동
    private void CameraDragMove()
    {
        if (isDragging)
        {

            float xmove = -dragDirection.x;
            float ymove = -dragDirection.y;

            //맵 경계 체크
            CameraPositionCheck(ref xmove, ref ymove, (-1 )* dragDirection, dragSpeed);

            Vector3 move = new Vector3(xmove, ymove, 0)*dragSpeed;
            transform.Translate(move, Space.World);

            //자유 카메라 위치 저장
            freeCameraTransform = cameraTransform.position;

        }
    }

    //카메라 위치 확인
    private void CameraPositionCheck(ref float xmove, ref float ymove, Vector2 direction, float speed)
    {
        topdown = true;
        leftright = true;

        //좌우 확인
        if (cameraTransform.position.x + direction.x * speed < min.x + halfWidth && cameraTransform.position.x + direction.x * speed > max.x - halfWidth)
        {
            //좌우 둘다 경계에 닿았을 때
            leftright = false;
        }
        else if (cameraTransform.position.x + direction.x * speed < min.x + halfWidth)
        {
            //좌측이 경계에 닿았을 때
            if (xmove > 0)
            {
                leftright = true;
            }
            else
            {
                leftright = false;
            }
        }
        else if (cameraTransform.position.x + direction.x * speed > max.x - halfWidth)
        {
            //우측이 경계에 닿았을 때
            if (xmove < 0)
            {
                leftright = true;
            }
            else
            {
                leftright = false;
            }
        }

        //상하 확인
        if (cameraTransform.position.y + direction.y * speed < min.y + halfHeight && cameraTransform.position.y + direction.y * speed > max.y - halfHeight)
        {
            //상하 모두 경계에 닿았을 때
            topdown = false;
        }
        else if (cameraTransform.position.y + direction.y * speed < min.y + halfHeight)
        {
            //하단만 경계에 닿았을 때
            if (ymove > 0)
            {
                topdown = true;
            }
            else
            {
                topdown = false;
            }
        }
        else if (cameraTransform.position.y + direction.y * speed > max.y - halfHeight)
        {
            //상단만 경계에 닿았을 때
            if (ymove < 0)
            {
                topdown = true;
            }
            else
            {
                topdown = false;
            }
        }



        // 카메라 이동 금지 설정
        if (!leftright)
        {
            xmove *= 0;
        }
        if (!topdown)
        {
            ymove *= 0;
        }

    }
}
