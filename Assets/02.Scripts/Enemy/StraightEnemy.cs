using UnityEngine;

// 플레이어를 최단거리로 쫓아가는 Enemy
public class StraightEnemy : MonoBehaviour, ITargetBrain
{
    public Vector3 TargetPosition(Transform player, EnemyNavigator navigator)
    {
        // 플레이어가 없을 시 제자리 정지
        if (player == null)
        {
            return navigator.transform.position;
        }
        // 목표는 플레이어의 위치
        return player.position;
    }
}
