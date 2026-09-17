using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Vector3 _currentDirection = Vector3.up;

    private Vector3 _targetPosition;
    private float _gridSize = 1f;

    private void Start()
    {
        // 시작할 때 자신의 위치를 첫 번째 목표 지점으로 고정
        _targetPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            _targetPosition = transform.position + (_currentDirection * _gridSize);
        }
    }
}
