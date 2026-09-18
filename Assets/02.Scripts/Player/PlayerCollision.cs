using UnityEngine;

// 플레이어의 깃발, 적과의 충돌을 관리하는 클래스
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private AudioClip _explosionSound; // 인스펙터에서 폭발음 넣을 칸

    [Header("Effects")]
    [SerializeField] private GameObject _flagParticlePrefab; // 파티클 넣을 칸

    [SerializeField] private GameObject _floatingTextPrefab; // 둥실둥실 텍스트 프리팹 넣을 칸

    private AudioSource _audioSource;
    private Animator _animator;

    private void Start()
    {
        // 게임 시작 시 Player에 붙어있는 Animator 컴포넌트 가져오기
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 게임 오버 상태면 중복 충돌 방지
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        // 적 Tag를 가진 오브젝트와 만나면 게임오버.
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            // 부딪히는 순간 폭발 사운드 재생
            if (_explosionSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_explosionSound);
            }

            // 1. 사망 애니메이션 스위치 켜기
            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }

            // 부딪힌 상대가 '적(Enemy)'일 때만 적도 함께 터뜨리기
            if (collision.CompareTag("Enemy"))
            {
                Animator enemyAnimator = collision.GetComponent<Animator>();
                if (enemyAnimator != null)
                {
                    enemyAnimator.SetTrigger("Die");
                }

                Destroy(collision.gameObject, 0.5f);
            }

            GameManager.Instance.GameOver();
        }
        // 깃발 태그를 가진 오브젝트를 만나면 점수 올라감.
        else if (collision.CompareTag("Flag") || collision.CompareTag("S_Flag") || collision.CompareTag("L_Flag"))
        {
            // 현재 연료량은 GameManager에서 가져와 토스
            float currentFuel = GameManager.Instance != null ? GameManager.Instance.CurrentFuel : 0f;

            // ScoreManager가 콤보와 보너스를 계산한 최종 점수를 받아옴
            int gainedScore = ScoreManager.Instance.CollectFlag(collision.tag, currentFuel);

            // 남은 전체 깃발 개수 감소 및 클리어 체크
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DecreaseFlag();
            }

            // 깃발 위치에 파티클 폭죽 터뜨리기
            if (_flagParticlePrefab != null)
            {
                Instantiate(_flagParticlePrefab, collision.transform.position, Quaternion.identity);
            }

            // 플로팅 텍스트 생성하고 실제 획득 점수 띄우기
            if (_floatingTextPrefab != null)
            {
                GameObject textObj =
                    Instantiate(_floatingTextPrefab, collision.transform.position, Quaternion.identity);
                FloatingText fText = textObj.GetComponent<FloatingText>();
                if (fText != null)
                {
                    fText.SetText("+" + gainedScore); // 계산된 점수 반영
                }
            }

            Destroy(collision.gameObject);
        }
    }
}