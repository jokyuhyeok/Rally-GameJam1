using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 씬 이동(다시하기, 메인메뉴)을 위해 필수

public class UIManager : MonoBehaviour
{
    // GameManager 등 다른 곳에서 UIManager를 쉽게 부를 수 있도록 싱글톤 추가
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText; // 현재 점수
    [SerializeField] private TextMeshProUGUI _hiScoreText; // 최고 점수
    [SerializeField] private Slider _fuelSlider; // 연료 게이지

    [SerializeField] private GameObject _gameOverPanel; // 에디터에서 연결할 게임 오버 패널

    // 클리어 패널 연결할 빈칸
    [SerializeField] private GameObject _gameClearPanel;

    private void Awake()
    {
        // 시작할 때 자신을 Instance에 등록
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // GameManager의 초기 연료량과 맞춰주는 역할
        if (_fuelSlider != null && GameManager.Instance != null)
        {
            _fuelSlider.maxValue = GameManager.Instance.MaxFuel;

            // 슬라이더 바도 꽉 차게 초기값 세팅
            _fuelSlider.value = GameManager.Instance.MaxFuel;
        }

        // 게임을 켤 때 혹시라도 게임 오버 화면이 켜져있으면 강제로 숨김
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(false);
        }

        // 게임 시작할 때 클리어 화면도 확실히 숨겨둠
        if (_gameClearPanel != null)
        {
            _gameClearPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // 1. 점수 화면 갱신 (ScoreManager 담당)
        if (ScoreManager.Instance != null)
        {
            if (_scoreText != null)
                _scoreText.text = ScoreManager.Instance.Score.ToString("D6"); // 000300으로 표시해줌. 

            if (_hiScoreText != null)
                _hiScoreText.text = ScoreManager.Instance.HiScore.ToString("D6");
        }

        // 2. 연료 화면 갱신 (GameManager 담당)
        if (GameManager.Instance != null)
        {
            if (_fuelSlider != null)
                _fuelSlider.value = GameManager.Instance.CurrentFuel;
        }
    }

    // 게임 오버 시 GameManager가 호출하여 패널을 띄움
    public void ShowGameOver()
    {
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }
    }

    // 게임 클리어 시 호출하여 패널을 띄움
    public void ShowGameClear()
    {
        if (_gameClearPanel != null)
        {
            _gameClearPanel.SetActive(true);
        }
    }

    // [다시 하기] 버튼 클릭 시 실행
    public void RestartGame()
    {
        // 현재 플레이 중인 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // [메인 메뉴] 버튼 클릭 시 실행
    public void GoToMainMenu()
    {
        // Build Settings에 0번으로 등록된 MainMenu 씬 로드
        SceneManager.LoadScene(0);
    }
}