using UnityEngine;

// 게임에서의 점수, 시간 제한(체력 / 연료), 깃발 획득 목표 관련 클래스
public class GameManager : MonoBehaviour
{
    // 연료량과 초당 연료 소모량 (시간제한과 같음.)
    [SerializeField] private float _currentFuel = 100f;
    [SerializeField] private float _fuelUseForSec = 1f;

    private bool _isGameOver = false;
    
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
        }
        
        Debug.Log("현재 남은 연료: " + _currentFuel);

        if (_currentFuel == 0f)
        {
            GameOver();
        }
        
    }
    
    private void GameOver()
    {
        _isGameOver = true; 
        Debug.LogError("연료 바닥! 게임 오버입니다!");
    }
}
