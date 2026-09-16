using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    // 플레이어 이동 속도 
    [SerializeField] private float _speed;
    // wall 타일맵을 연결할 변수
    [SerializeField] private Tilemap _tilemap;
    // 타일의 중심에 도착했다고 판정할 오차 범위
    private const float _judgeCenterArrivedRange = 0.01f;
    
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
        UserKeyInput();
        
        Move();
    }

    // 플레이어 키보드 입력 예약을 받는다. 
    private void UserKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _nextDirection = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _nextDirection = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _nextDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _nextDirection = Vector3.right;
        }
    }
    
    private void Move()
        {
            // 다음 타일의 정중앙을 향해 이동
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition,  _speed * Time.deltaTime);
            
            // 타일 정중앙 도착시 다음 목표 설정 후 업데이트
            if(Vector3.Distance(transform.position, _targetPosition) <= _judgeCenterArrivedRange)
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
            }
        }
    
    private bool IsBlocked(Vector3 direction)
    {
        // 다음 프레임에 이동할 위치를 계산하고 그 위치의 블록 확인
        Vector3 nextPosition = _targetPosition + direction;
        Vector3Int nextMapBlock = _tilemap.WorldToCell(nextPosition);
        
        // 그 블록이 막혀있나?
        return _tilemap.HasTile(nextMapBlock);
    }
}