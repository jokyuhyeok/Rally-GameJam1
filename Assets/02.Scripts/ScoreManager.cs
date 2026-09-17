using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // 깃발 3종류 변수 선언
    [SerializeField] private int _scoreNormalFlag = 100;
    [SerializeField] private int _score_S_Flag_Multiplier = 2;
    [SerializeField] private int _score_L_Flag_Ratio = 5;

    private int _score = 0; // 점수
    private int _hiScore = 0; // 하이스코어
    private int _flagCount = 0; // 깃발의 개수
    private bool _activate_S_Flag = false;

    // 외부에서 읽어갈 수 있는 프로퍼티
    public int Score => _score;
    public int HiScore => _hiScore;

    private void Awake()
    {
        // 씬 시작시 자신을 Instance에 등록
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadHiScore(); // 게임 시작 시 저장된 최고 점수 불러오기
    }
    
    private void Update()
    {
        // 키보드 F12 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.F12)) 
        {
            PlayerPrefs.DeleteKey("HI_SCORE"); 
            
            // 현재 화면과 변수에도 0(또는 기본값)으로 즉시 반영
            _hiScore = 20000; // 처음 세팅했던 기본 하이스코어 값
            Debug.Log("하이스코어 데이터가 초기화되었습니다!");
        }
    }

    // PlayerPrefs로 하이스코어 불러오기
    private void LoadHiScore()
    {
        // "HI_SCORE"라는 키로 저장된 값을 불러옴. 기록이 없으면 기본값 0 지정
        _hiScore = PlayerPrefs.GetInt("HI_SCORE", 0);
    }

    // PlayerPrefs로 하이스코어 영구 저장하기
    private void SaveHiScore()
    {
        if (_score > _hiScore)
        {
            _hiScore = _score;
            PlayerPrefs.SetInt("HI_SCORE", _hiScore);
            PlayerPrefs.Save(); // 즉시 하드디스크에 기록
        }
    }

    // 깃발 획득 시 점수 계산 및 총점 갱신
    public void CollectFlag(string flagType, float currentFuel)
    {
        _flagCount++;
        int gainedScore = CalculateBaseScore(); // 기본 깃발 획득시 획득 점수

        // S 깃발 획득후 모든 깃발 획득점수 2배, L 깃발 획득시 남은 연료에 따라 보너스 점수 올라감.
        S_Flag_Activate(flagType);
        gainedScore = Score_S_Flag(gainedScore);
        gainedScore = Score_L_Flag(flagType, gainedScore, currentFuel);

        _score += gainedScore;
        SaveHiScore(); // 점수가 오를 때마다 최고 점수 갱신 체크

        Debug.Log($"{flagType} 획득. (+{gainedScore}점) / 현재 총점 : {_score} / 하이스코어 : {_hiScore}");
    }

    private int CalculateBaseScore() => _flagCount * _scoreNormalFlag;

    private void S_Flag_Activate(string flagType)
    {
        if (flagType == "S_Flag") _activate_S_Flag = true;
    }

    private int Score_S_Flag(int gainedScore)
    {
        if (_activate_S_Flag) gainedScore *= _score_S_Flag_Multiplier;
        return gainedScore;
    }

    private int Score_L_Flag(string flagType, int gainedScore, float currentFuel)
    {
        if (flagType == "L_Flag")
        {
            int fuelBonus = Mathf.FloorToInt(currentFuel * _score_L_Flag_Ratio);
            gainedScore += fuelBonus;
        }

        return gainedScore;
    }
}