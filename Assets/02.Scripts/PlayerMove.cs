using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    // 플레이어 이동 속도 
    [SerializeField] private float _speed;
    // wall 타일맵을 연결할 변수
    [SerializeField] private Tilemap _tilemap;
    // 벽의 존재 유무 판정 거리 변수
    [SerializeField] private float _checkDistance;
    // 플레이어의 방향벡터
    private Vector3 _direction = Vector3.up;
    

    private void Update()
    {
        if (IsBlocked(_direction))
        {
            return;
        }
        
        Move();
    }

    private void Move()
    {
        transform.position = transform.position + _direction * _speed * Time.deltaTime;
    }
    
    private bool IsBlocked(Vector3 direction)
    {
        // 다음 프레임에 이동할 위치를 계산하고 그 위치의 블록 확인
        Vector3 nextPosition = transform.position + direction * _checkDistance;
        Vector3Int nextMapBlock = _tilemap.WorldToCell(nextPosition);
        
        // 그 블록이 막혀있나?
        return _tilemap.HasTile(nextMapBlock);
    }
}