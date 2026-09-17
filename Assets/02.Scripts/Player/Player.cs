using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어가 깃발에 닿았을 때 일어날 이벤트 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 현재 연료량을 GameManager로부터 안전하게 가져온다.
        float currentFuel = GameManager.Instance != null ? GameManager.Instance.CurrentFuel : 0f;

        if (collision.CompareTag("Flag"))
        {
            ScoreManager.Instance.CollectFlag("Flag", currentFuel);
            Destroy(collision.gameObject);
            Debug.Log("깃발 획득!");
        }
        else if (collision.CompareTag("S_Flag"))
        {
            ScoreManager.Instance.CollectFlag("S_Flag", currentFuel);
            Destroy(collision.gameObject);
            Debug.Log("S 깃발 획득! 지금부터 점수 2배");
        }
        else if (collision.CompareTag("L_Flag"))
        {
            ScoreManager.Instance.CollectFlag("L_Flag", currentFuel);
            Destroy(collision.gameObject);
            Debug.Log("L 깃발 획득! 남은 연료만큼 보너스 점수!");
        }
    }
}