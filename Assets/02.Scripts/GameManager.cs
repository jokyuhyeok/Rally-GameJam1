using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// 게임에서의 점수, 시간 제한(체력 / 연료), 깃발 획득 목표 관련 클래스
public class GameManager : MonoBehaviour
{
    // 다른 스크립트에서 GameInstance로 부를 수 있게 해줌
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _countdownText; // 💡 카운트다운 글자

    // 연료량과 초당 연료 소모량 (시간제한과 같음.)
    [SerializeField] private float _maxFuel = 100f;
    [SerializeField] private float _currentFuel = 100f;
    [SerializeField] private float _fuelUseForSec = 1f;

    // 3초 대기 타이머
    private float _startTimer = 3.99f;

    // 타이머가 0 이하가 되면 true 반환
    public bool IsGameStarted => _startTimer <= 0f;

    private bool _isGameOver = false;
    private bool _isFuelEmpty = false;

    public float MaxFuel => _maxFuel;
    public float CurrentFuel => _currentFuel;
    public bool IsGameOver => _isGameOver;
    public bool IsFuelEmpty => _isFuelEmpty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // 게임이 시작될 때 현재 연료를 최대치로 세팅
        _currentFuel = _maxFuel;
    }

    private void Update()
    {
        // 타이머 계산 (3초 대기)
        if (_startTimer > 0f)
        {
            _startTimer -= Time.deltaTime; // 매 프레임 시간 깎기

            if (_countdownText != null)
            {
                // 소수점 올림으로 3, 2, 1 표시
                _countdownText.text = Mathf.FloorToInt(_startTimer).ToString();
            }

            return; // 타이머가 안 끝났으면 밑으로 안 내려감 (연료 소모 정지)
        }
        // 타이머가 0이 되고, 텍스트가 켜져있다면 GO!로 바꾸고 잠시 후 끄기 등 처리 가능
        else if (_countdownText != null && _countdownText.gameObject.activeSelf)
        {
            _countdownText.gameObject.SetActive(false); // 글자 숨김
        }

        // 게임 오버면 아무것도 하지 않음
        if (_isGameOver || _isFuelEmpty)
        {
            return;
        }

        // 2. 연료 계산 및 체크 
        FuelRemainCheck();
    }

    private void FuelRemainCheck()
    {
        // 초당 연료 소모시키고 Debug Log 확인..
        _currentFuel -= _fuelUseForSec * Time.deltaTime;

        if (_currentFuel <= 0f)
        {
            _currentFuel = 0f;
            _isFuelEmpty = true;
            Debug.Log("연료가 바닥나서 차가 멈췄습니다!");
        }
    }

    public void GameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;
        Debug.LogError("게임 오버입니다!");

        // 💡 [추가] UIManager의 ShowGameOver 함수를 실행해 화면 띄우기
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }
    }

    public bool UseFuel(float cost)
    {
        // 연료가 고갈되었거나 부족하면 사용 불가
        if (_isFuelEmpty || _currentFuel < cost)
        {
            return false;
        }

        _currentFuel -= cost;
        return true;
    }
}