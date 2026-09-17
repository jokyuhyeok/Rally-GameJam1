using UnityEngine;

public class Gas : MonoBehaviour
{
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private float _stunTime = 3f;

    private void Start()
    {
        // 자기 자신을 일정 시간 이후 파괴한다. 
        Destroy(gameObject, _lifetime);
    }

    // 방구의 센서에 닿았을 때 발동됨.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyNavigator enemy = collision.GetComponent<EnemyNavigator>();

        if (enemy != null)
        {
            enemy.Stun(_stunTime);
        }
    }
}