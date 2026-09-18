using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    // 플레이어 이동 속도 
    [SerializeField] private float _speed;

    // wall 타일맵을 연결할 변수
    [SerializeField] private Tilemap _tilemap;

    // 타일의 중심에 도착했다고 판정할 오차 범위
    private const float JudgeCenterArrivedRange = 0.01f;

    // 플레이어의 방향벡터, 예약된 다음 방향 분리
    private Vector3 _currentDirection = Vector3.up;
    private Vector3 _nextDirection = Vector3.up;

    // 플레이어가 다음으로 도착해야할 타일의 정중앙 좌표
    private Vector3 _targetPosition;

    // 게임 시작시 플레이어의 위치를 타일의 정중앙에 맞추기
    private void Start()
    {
        Vector3Int startBlock = _tilemap.WorldToCell(transform.position);
        _targetPosition = _tilemap.GetCellCenterWorld(startBlock);
        transform.position = _targetPosition;
    }

    private void Update()
    {
        // 게임 시작 타이머가 끝나지 않았다면 아무것도 안 함 (이동 정지)
        if (GameManager.Instance != null && !GameManager.Instance.IsGameStarted)
        {
            return;
        }

        // 게임 오버이거나 연료가 바닥났다면 정지
        if (GameManager.Instance != null && (GameManager.Instance.IsGameOver || GameManager.Instance.IsFuelEmpty))
        {
            return;
        }

        UserKeyInput();
        Move();
    }

    // 플레이어 키보드 입력 예약을 받는다. 
    private void UserKeyInput()
    {
        // 방금 누른 키 저장하는 변수
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputDirection = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputDirection = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputDirection = Vector3.right;
        }

        if (inputDirection == Vector3.zero) return;

        // 누른 방향이 현재 방향과 정반대일 때
        if (IsOpposite(inputDirection, _currentDirection))
        {
            // 타일 중앙까지 안 가고, 바로 유턴
            _currentDirection = inputDirection;
            _nextDirection = inputDirection;

            _targetPosition = _targetPosition + _currentDirection;

            // 유턴할 때 즉시 차체 회전 반영
            RotateCar(_currentDirection);
        }
        else
        {
            // 다음 목표 지점 예약
            _nextDirection = inputDirection;
        }
    }

    private void Move()
    {
        // 다음 타일의 정중앙을 향해 이동
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        // 타일 정중앙 도착시 다음 목표 설정 후 업데이트
        if (Vector3.Distance(transform.position, _targetPosition) <= JudgeCenterArrivedRange)
        {
            transform.position = _targetPosition;

            if (!IsBlocked(_nextDirection))
            {
                _currentDirection = _nextDirection;
            }
            else if (IsBlocked(_currentDirection))
            {
                return;
            }

            _targetPosition = _targetPosition + _currentDirection;

            // 새로운 타일로 꺾일 때 차체 회전 반영
            RotateCar(_currentDirection);
        }
    }

    // 현재 이동 중인 방향 벡터를 받아 Z축 각도를 조절.
    private void RotateCar(Vector3 dir)
    {
        if (dir == Vector3.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 스프라이트 원본이 '위(UP)'를 보고 있으므로 90도를 뺀다.
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private bool IsBlocked(Vector3 direction)
    {
        // 다음 프레임에 이동할 위치를 계산하고 그 위치의 블록 확인
        Vector3 nextPosition = _targetPosition + direction;
        Vector3Int nextMapBlock = _tilemap.WorldToCell(nextPosition);

        // 그 블록이 막혀있나?
        return _tilemap.HasTile(nextMapBlock);
    }

    private bool IsOpposite(Vector3 onedirection, Vector3 otherdirection)
    {
        return (onedirection + otherdirection) == Vector3.zero;
    }

    // Enemy Navigator에서 참조하기 좋게 함수 생성함.
    public Vector3 GetCurrentDirection()
    {
        return _currentDirection;
    }
}