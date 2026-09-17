using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;     // 현재 점수
    [SerializeField] private TextMeshProUGUI _hiScoreText;   // 최고 점수
    [SerializeField] private Slider _fuelSlider;             // 연료 게이지

    private void Start()
    {
        // GameManager의 초기 연료량과 맞춰주는 역할
        if (_fuelSlider != null && GameManager.Instance != null)
        {
            _fuelSlider.maxValue = GameManager.Instance.MaxFuel;
            
            // 슬라이더 바도 꽉 차게 초기값 세팅
            _fuelSlider.value = GameManager.Instance.MaxFuel; 
        }
    }

    private void Update()
    {
        // 1. 점수 화면 갱신 (ScoreManager 담당)
        if (ScoreManager.Instance != null)
        {
            if (_scoreText != null)
                _scoreText.text = ScoreManager.Instance.Score.ToString("D6");   // 000300으로 표시해줌. 

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
}