using UnityEngine;

// 플레이어의 깃발, 적과의 충돌을 관리하는 클래스
public class PlayerCollision : MonoBehaviour
{
    private Animator _animator;
    
    private void Start()
    {
        // 게임 시작 시 Player에 붙어있는 Animator 컴포넌트 가져오기
        _animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 게임 오버 상태면 중복 충돌 방지
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        
        // 적 Tag를 가진 오브젝트와 만나면 게임오버.
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            // 1. 사망 애니메이션 스위치 켜기
            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }
            GameManager.Instance.GameOver();
        }
        // 깃발 태그를 가진 오브젝트를 만나면 점수 올라감.
        else if (collision.CompareTag("Flag") || collision.CompareTag("S_Flag") || collision.CompareTag("L_Flag"))
        {
            // 현재 연료량은 GameManager에서 가져와 토스
            float currentFuel = GameManager.Instance != null ? GameManager.Instance.CurrentFuel : 0f;
            ScoreManager.Instance.CollectFlag(collision.tag, currentFuel);

            Destroy(collision.gameObject);
        }
    }
}