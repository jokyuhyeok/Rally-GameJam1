using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// 게임에서의 점수, 시간 제한(체력 / 연료), 깃발 획득 목표 관련 클래스
public class GameManager : MonoBehaviour
{
    // 다른 스크립트에서 GameInstance로 부를 수 있게 해줌
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _countdownText; // 카운트다운 글자

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

    // 클리어 상태와 남은 깃발 개수를 관리할 변수
    public bool IsStageClear { get; private set; } = false;
    private int _remainFlags = 0;

    [SerializeField] private float _shakeDuration = 0.2f; // 흔들리는 시간
    [SerializeField] private float _shakeMagnitude = 0.3f; // 흔들리는 강도
    [SerializeField] private float _hitStopTime = 0.15f; // 화면이 멈춰있는 시간

    // 역경직(시간 멈춤) 타이머 변수
    private float _hitStopTimer = 0f;
    private bool _isHitStopping = false;

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

    // 게임 시작 시 맵에 배치된 깃발의 총 개수를 파악합니다.
    private void Start()
    {
        _remainFlags = GameObject.FindGameObjectsWithTag("Flag").Length
                       + GameObject.FindGameObjectsWithTag("S_Flag").Length
                       + GameObject.FindGameObjectsWithTag("L_Flag").Length;
    }

    private void Update()
    {
        // [역경직 타이머] 시간이 멈춰있는지 제일 먼저 체크
        if (_isHitStopping)
        {
            _hitStopTimer -= Time.unscaledDeltaTime; // 멈춘 상태에서도 흐르는 현실 시간 사용
            if (_hitStopTimer <= 0f)
            {
                _isHitStopping = false;
                Time.timeScale = 1f; // 정지 해제

                // 역경직이 끝나면 화면에 게임 오버 UI 띄우기
                if (UIManager.Instance != null) UIManager.Instance.ShowGameOver();
            }

            return; // 역경직 중에는 밑으로 안 내려감 (연료 소모 정지)
        }

        // 타이머 계산 (3초 대기)
        if (_startTimer > 0f)
        {
            _startTimer -= Time.deltaTime; // 매 프레임 시간 깎기

            if (_countdownText != null)
            {
                // 소수점 버림으로 3, 2, 1 표시 (기존 코드 유지)
                _countdownText.text = Mathf.FloorToInt(_startTimer).ToString();
            }

            return; // 타이머가 안 끝났으면 밑으로 안 내려감 (연료 소모 정지)
        }
        // 타이머가 0이 되고, 텍스트가 켜져있다면 GO!로 바꾸고 잠시 후 끄기 등 처리 가능
        else if (_countdownText != null && _countdownText.gameObject.activeSelf)
        {
            _countdownText.gameObject.SetActive(false); // 글자 숨김
        }

        // 게임 오버, 연료 고갈, "또는 클리어 상태"면 아무것도 하지 않음 (연료 소모 멈춤)
        if (_isGameOver || _isFuelEmpty || IsStageClear)
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

    // 적과 부딪혔을 때 호출
    public void GameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;
        Debug.LogError("게임 오버입니다!");

        // 1. 번쩍임 UI 실행
        if (UIManager.Instance != null) UIManager.Instance.ShowFlash();

        // 2. 카메라 흔들림 실행
        if (CameraShake.Instance != null) CameraShake.Instance.Shake(_shakeDuration, _shakeMagnitude);

        // 3. 역경직(시간 정지) 스위치 켜기
        Time.timeScale = 0f; // 게임 시간 정지
        _hitStopTimer = _hitStopTime; // 멈춰있을 시간 세팅
        _isHitStopping = true; // 타이머 시작
    }

    // 깃발을 먹었을 때 호출할 함수
    public void DecreaseFlag()
    {
        _remainFlags--; // 남은 깃발 개수 1 감소

        // 깃발을 다 먹었다면? 게임 클리어!
        if (_remainFlags <= 0 && !IsStageClear)
        {
            IsStageClear = true;
            Debug.Log("게임 클리어!");

            // UIManager를 통해 클리어 패널 띄우기
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowGameClear();
            }
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