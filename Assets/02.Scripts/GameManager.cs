using UnityEngine;

// 게임에서의 점수, 시간 제한(체력 / 연료), 깃발 획득 목표 관련 클래스
public class GameManager : MonoBehaviour
{
    // 다른 스크립트에서 GameInstance로 부를 수 있게 해줌
    public static GameManager Instance { get; private set; }
    
    // 연료량과 초당 연료 소모량 (시간제한과 같음.)
    [SerializeField] private float _maxFuel = 100f;
    [SerializeField] private float _currentFuel = 100f;
    [SerializeField] private float _fuelUseForSec = 1f;

    private bool _isGameOver = false;
    
    public float MaxFuel => _maxFuel;
    public float CurrentFuel => _currentFuel;
    public bool IsGameOver => _isGameOver;
    
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
        // 게임 오버면 아무것도 하지 않음
        if (_isGameOver)
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
            GameOver();;
        }
    }
    
    public void GameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true; 
        Debug.LogError("게임 오버입니다!");
    }
    
    public bool UseFuel(float cost)
    {
        if (_currentFuel >= cost)
        {
            _currentFuel -= cost;
            return true;
        }
        return false;
    }
}
