using FieldDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class MovementComponent : MonoBehaviour
{
    //외부 참조 요소
    public Action CameraUpdate;
    public Action<Vector2Int> RemoveAtPathFunc;
    public Func<Vector2Int, Vector2> GetCenter;
    //외부 클래스 이벤트 발생용
    public Action Movement = () => { };

    //플레이어 이동 관련 변수
    private Transform playerTransform;
    public bool isMoving = false;

    //이동 관련 정보 클래스
    private MovementInfo _movementInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = gameObject.GetComponent<Transform>();
    }

    public void Init(MovementInfo movementInfo)
    {
        _movementInfo = movementInfo;
    }

    public void SetPlayertLocation(Vector3 vector)
    {

        playerTransform.Translate(vector);
    }

    //이동 중지
    public void StopMovement()
    {
        StopAllCoroutines();
        isMoving = false;
    }

    //움직임 시작 입력
    public void MoveStart()
    {

        //경로가 비어있다면
        if (_movementInfo.movePath.Count == 0)
        {
            return;
        }

        if (!isMoving)
        {
            StartCoroutine(nameof(MoveAlongPath));
        }

    }

    //경로 이동하기
    private IEnumerator MoveAlongPath()
    {
        isMoving = true;
        int currentIndex = 0;
        int count = _movementInfo.movePath.Count;
        while (currentIndex < count)
        {
            Debug.Log($"몇번째 이동인가? {currentIndex}");
            Vector2 target = GetCenter(_movementInfo.movePath[0]);
            yield return StartCoroutine(MoveTo(target));
            RemoveAtPathFunc(_movementInfo.movePath[0]);
            _movementInfo.movePath.RemoveAt(0);
            currentIndex++;

        }
        isMoving = false;

    }

    //현재 위치에서 다음 좌표로 이동
    private IEnumerator MoveTo(Vector2 target)
    {
        Debug.Log($"이동 타겟 : {target}");

        Vector2 start = transform.position;

        float distance = Vector2.Distance(start, target);
        float duration = distance / _movementInfo.moveSpeed;

        float elapsed = 0f;
        int sec = 1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector2.Lerp(start, target, t);
            CameraUpdate();

            //1초마다 이동 이벤트 발생
            if (elapsed >= sec)
            {
                sec++;
                Movement();
            }

            yield return null;
        }
        transform.position = target;
    }


}


