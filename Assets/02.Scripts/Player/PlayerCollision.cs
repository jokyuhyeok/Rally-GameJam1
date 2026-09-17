using UnityEngine;

// 플레이어의 깃발, 적과의 충돌을 관리하는 클래스
public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적 Tag를 가진 오브젝트와 만나면 게임오버.
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
        // 깃발 태그를 가진 오브젝트를 만나면 점수 올라감.
        else if (collision.CompareTag("Flag") || collision.CompareTag("S_Flag") || collision.CompareTag("L_Flag"))
        {
            GameManager.Instance.CollectFlag(collision.tag);

            Destroy(collision.gameObject);
        }
    }
}