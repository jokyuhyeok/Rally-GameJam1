using UnityEngine;

// 플레이어의 뒤를 노려 도망을 막는 적
public class BackEnemy : MonoBehaviour, ITargetBrain
{
    // 몇 칸 뒤를 노릴지 조정가능 변수
    [SerializeField] private float _backDistance = 3f;
    [SerializeField] private float _killRadius = 3f;

    public Vector3 TargetPosition(Transform player, EnemyNavigator navigator)
    {
        if (player == null)
        {
            return navigator.transform.position;
        }
        // 가까워지면 플레이어에게 돌진
        float distanceToPlayer = Vector3.Distance(navigator.transform.position, player.position);

        if (distanceToPlayer <= _killRadius)
        {
            return player.position;
        }

        // 일정거리 이상 거리가 있다면?
        PlayerMove playerMove = player.GetComponent<PlayerMove>();

        if (playerMove != null)
        {
            Vector3 playerDirection = playerMove.GetCurrentDirection();
            // 등 뒤의 좌표를 목표로 잡는다. 
            return player.position + (-playerDirection * _backDistance);
        }

        return player.position;
    }
}
