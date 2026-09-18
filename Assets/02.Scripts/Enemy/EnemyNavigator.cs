using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemyNavigator : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private float _speed = 4f;

    private const float JudgeCenterArrivedRange = 0.01f;
    private ITargetBrain _currentBrain; // 현재 장착된 인터페이스

    private Vector3 _currentDirection = Vector3.zero;
    private Vector3 _targetPosition;
    private float _gridSize = 1f;

    private bool _isStunned = false;
    private float _stunTimer = 0f;

    private Animator _animator;

    // 길찾기용 4방향 배열 선언
    private readonly Vector3Int[] _directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

    private void Start()
    {
        _currentBrain = GetComponent<ITargetBrain>();
        _animator = GetComponent<Animator>();

        // 시작할 때 타일 정중앙에 맞춤.
        Vector3Int tilePosition = _tilemap.WorldToCell(transform.position);
        _targetPosition = _tilemap.GetCellCenterWorld(tilePosition);
        transform.position = _targetPosition;
    }

    private void Update()
    {
        // 게임 시작 타이머가 끝나지 않았다면 적도 움직이지 않음 (대기)
        if (GameManager.Instance != null && !GameManager.Instance.IsGameStarted)
        {
            return;
        }

        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f)
            {
                _isStunned = false;
                if (_animator != null)
                {
                    _animator.SetBool("IsStunned", false);
                }

                Debug.Log("스턴 끝! 다시 쫓아간다.");
            }
        }

        MoveToNextTile();
    }

    // 1. 이동만 담당
    private void MoveToNextTile()
    {
        // 목표 타일 정중앙으로 이동한다.
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        // 타일 정중앙에 도착했다면?
        if (Vector3.Distance(transform.position, _targetPosition) < JudgeCenterArrivedRange)
        {
            transform.position = _targetPosition;

            if (_isStunned)
            {
                _currentDirection = Vector3.zero;
            }
            else
            {
                // 두뇌에게 다음 목적지를 물어보고, 길을 찾는다!
                _currentDirection = DecideNextDirection();
                RotateEnemy(_currentDirection);
            }

            _targetPosition = transform.position + (_currentDirection * _gridSize);
        }
    }

    // 2. 두뇌에게 목적지를 묻고, 첫 번째 스텝(방향)을 찾는 녀석
    private Vector3 DecideNextDirection()
    {
        if (_player == null || _currentBrain == null) return Vector3.zero;

        // 두뇌야, 나 최종적으로 어디로 가야 해?
        Vector3 finalDestination = _currentBrain.TargetPosition(_player, this);

        // 그 목적지까지 가는 최단 경로의 '첫 번째 방향'을 계산해서 줘!
        return BFSDirection(finalDestination);
    }

    // 3. 길찾기 알고리즘
    private Vector3 BFSDirection(Vector3 targetPos)
    {
        Vector3Int startTile = _tilemap.WorldToCell(transform.position); // 현재 적 위치
        Vector3Int targetTile = _tilemap.WorldToCell(targetPos); // 플레이어(타겟) 위치

        // 이미 도착한 상태 => 정지
        if (startTile == targetTile) return Vector3.zero;

        // 길을 찾기 위한 2가지 필수 도구
        Queue<Vector3Int> searchQueue = new Queue<Vector3Int>(); // 앞으로 탐색할 타일들의 대기열
        Dictionary<Vector3Int, Vector3Int>
            pathTrace = new Dictionary<Vector3Int, Vector3Int>(); // 발자국 기록장 (현재 타일은 어디서부터 왔는가?)

        // 시작점 세팅
        searchQueue.Enqueue(startTile);
        pathTrace.Add(startTile, startTile);

        // 목적지를 찾을 때까지 주변을 탐색.
        while (searchQueue.Count > 0)
        {
            Vector3Int currentTile = searchQueue.Dequeue(); // 대기열에서 하나 꺼내기

            // 목적지를 찾았다면 탐색 중지
            if (currentTile == targetTile) break;

            // 상하좌우 4방향을 찔러
            foreach (Vector3Int direction in _directions)
            {
                Vector3Int nextTile = currentTile + direction;

                // 벽으로 막혀있지 않고 && 아직 발자국이 없는 곳이라면?
                if (!_tilemap.HasTile(nextTile) && !pathTrace.ContainsKey(nextTile))
                {
                    searchQueue.Enqueue(nextTile); // 대기열에 넣고
                    pathTrace.Add(nextTile, currentTile); // nextTile은 currentTile에서 왔다
                }
            }
        }

        // 목적지에서부터 시작점까지 거꾸로 간다.
        if (pathTrace.ContainsKey(targetTile)) // 목적지까지 가는 길이 있다면
        {
            Vector3Int trackCell = targetTile;

            // 시작점의 '바로 다음 칸'이 나올 때까지 거꾸로 올라감.
            while (pathTrace[trackCell] != startTile)
            {
                trackCell = pathTrace[trackCell]; // 이전 발자국으로 이동
            }

            // 시작점에서 그 '바로 다음 칸'으로 가려면 어느 방향인지 계산해서 반환
            return (Vector3)(trackCell - startTile);
        }

        // 길이 완전히 막혀서 못 갈 때의 안전장치 (그냥 빈 곳 아무 데나 가기)
        foreach (Vector3Int dir in _directions)
        {
            if (!_tilemap.HasTile(startTile + dir) && (Vector3)dir != -_currentDirection) return (Vector3)dir;
        }

        return -_currentDirection; // 꼼짝 못하면 제자리 U턴
    }


    // 스턴 적용 메서드
    public void Stun(float stunTime)
    {
        if (_isStunned) return;

        _isStunned = true;
        _stunTimer = stunTime;

        if (_animator != null)
        {
            _animator.SetBool("IsStunned", true);
        }

        Debug.Log("나 기절!");
    }

    // 적 회전 전용 함수
    private void RotateEnemy(Vector3 dir)
    {
        if (dir == Vector3.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 스프라이트가 위(UP)를 보고 있다면 -90도 보정
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}