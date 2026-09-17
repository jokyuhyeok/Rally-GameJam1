using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어가 깃발에 닿았을 때 일어날 이벤트 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flag"))
        {
            Destroy(collision.gameObject);

            Debug.Log("깃발 획득!");
        }
    }
}