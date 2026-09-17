using UnityEngine;

// 플레이어의 이동 방향 몇 칸 앞길을 미리가서 막는 적
public class PredictEnemy : MonoBehaviour, ITargetBrain
{
    [SerializeField] private float _predictDistance = 3f;
    [SerializeField] private float _killRadius = 3f;

    public Vector3 TargetPosition(Transform player, EnemyNavigator navigator)
    {
        if (player == null)
        {
            return navigator.transform.position;
        }
        // 플레이어와 거리를 재서 범위 내에 들어오면 돌진.
        float distanceToPlayer = Vector3.Distance(navigator.transform.position, player.position);
        if (distanceToPlayer <= _killRadius)
        {
            return player.position;
        }
        // 플레이어가 멀리 있다면 플레이어의 앞길을 예측 후 이동
        PlayerMove playerMove = player.GetComponent<PlayerMove>();

        if (playerMove != null)
        {
            Vector3 playerDirection = playerMove.GetCurrentDirection(); // 플레이어 현재 이동 방향

            return player.position + (playerDirection * _predictDistance);
        }

        return player.position;
    }
}
