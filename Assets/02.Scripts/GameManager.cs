using UnityEngine;

// 게임에서의 점수, 시간 제한(체력 / 연료), 깃발 획득 목표 관련 클래스
public class GameManager : MonoBehaviour
{
    // 싱글톤 : Player.cs에서 인스턴스로 바로 접근할 수 있도록 함.
    public static GameManager Instance { get; private set; }

    // 연료량과 초당 연료 소모량 (시간제한과 같음.)
    [SerializeField] private float _currentFuel = 100f;
    [SerializeField] private float _fuelUseForSec = 1f;
    private bool _isGameOver = false;

    public float CurrentFuel => _currentFuel;
    public bool IsGameOver => _isGameOver;

    private void Awake()
    {
        // 게임시작시 자신을 인스턴스에 등록
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (_isGameOver)
            return;

        // 연료 계산 및 체크
        UpdateFuel();
    }

    // 매 프레임 연료를 감소, 고갈 시 게임오버 처리.
    private void UpdateFuel()
    {
        _currentFuel -= _fuelUseForSec * Time.deltaTime;

        if (_currentFuel <= 0f)
        {
            _currentFuel = 0f;
            GameOver();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
        Debug.LogError("게임 오버입니다!");
        Time.timeScale = 0f; // 아까 이야기했던 정지 기능 포함
    }

    // 깃발 습득 요청이 오면 ScoreManager에게 현재 연료량과 함께 토스!
    public void CollectFlag(string flagType)
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CollectFlag(flagType, _currentFuel);
        }
    }

    // player 연료 스킬 사용시 연료 차감
    public bool UseFuel(float cost)
    {
        // 스킬 발동 성공
        if (_currentFuel >= cost)
        {
            _currentFuel -= cost;
            return true;
        }

        // 스킬 발동 실패
        return false;
    }
}