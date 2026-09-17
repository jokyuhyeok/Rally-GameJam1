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

    // 깃발 획득 시 올라갈 점수 수치 조정
    [SerializeField] private int _scoreNormalFlag;
    [SerializeField] private int _score_S_Flag_Multiplier;
    [SerializeField] private int _score_L_Flag_Ratio;

    private int _score = 0;
    private int _flagCount = 0;
    private bool _activate_S_Flag = false;

    private void Awake()
    {
        // 씬 시작 시 자신을 Instance에 등록
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (_isGameOver)
        {
            return;
        }

        // 2. 연료 계산 및 체크 
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

    private void GameOver()
    {
        _isGameOver = true;
        Debug.LogError("연료 바닥! 게임 오버입니다!");
    }

    // 깃발 획득 시 깃발 종류에 따라 점수 계산 후 최종 점수 반환
    public void CollectFlag(string flagType)
    {
        _flagCount++;

        int gainedScore = CalculateBaseScore(); // 기본 점수 계산

        // 깃발이 S_깃발이면 깃발 획득 점수를 n배 늘린다.
        S_Flag_Activate(flagType);
        gainedScore = Score_S_Flag(gainedScore);

        // 깃발이 L_깃발이면 남은 연료량에 비례해 보너스 점수를 획득
        gainedScore = Score_L_Flag(flagType, gainedScore);

        // 최종 점수
        _score += gainedScore;
        Debug.Log($"{flagType} 획득. (+{gainedScore}점) / 현재 총점 : {_score}");
    }

    private int Score_L_Flag(string flagType, int gainedScore)
    {
        if (flagType == "L_Flag")
        {
            int fuelBonus = Mathf.FloorToInt(_currentFuel * _score_L_Flag_Ratio);
            gainedScore += fuelBonus;
        }

        return gainedScore;
    }

    private void S_Flag_Activate(string flagType)
    {
        if (flagType == "S_Flag")
        {
            _activate_S_Flag = true;
        }
    }

    private int Score_S_Flag(int gainedScore)
    {
        if (_activate_S_Flag)
        {
            gainedScore *= _score_S_Flag_Multiplier;
        }

        return gainedScore;
    }

    private int CalculateBaseScore()
    {
        return _flagCount * _scoreNormalFlag;
    }
}