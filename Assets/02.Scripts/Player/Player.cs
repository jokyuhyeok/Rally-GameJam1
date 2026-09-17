using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어가 깃발에 닿았을 때 일어날 이벤트 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flag"))
        {
            // GameManager에 깃발 먹었다고 신고
            GameManager.Instance.CollectFlag("Flag");
            Destroy(collision.gameObject);
            Debug.Log("깃발 획득!");
        }
        else if (collision.CompareTag("S_Flag"))
        {
            GameManager.Instance.CollectFlag("S_Flag");
            Destroy(collision.gameObject);
            Debug.Log("S 깃발 획득! 지금부터 점수 2배");
        }
        else if (collision.CompareTag("L_Flag"))
        {
            GameManager.Instance.CollectFlag("L_Flag");
            Destroy(collision.gameObject);
            Debug.Log("L 깃발 획득! 남은 연료만큼 보너스 점수!");
        }
    }
}