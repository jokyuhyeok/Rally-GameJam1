using UnityEngine;

// Enemy들의 각자 다른 타겟 설정을 분리하기 위한 인터페이스 
public interface ITargetBrain
{
    // 도달해야할 목표 좌표를 반환한다.
    Vector3 TargetPosition(Transform player, EnemyNavigator navigator);
}
