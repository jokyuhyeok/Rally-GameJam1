using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 반드시 추가해야 하는 네임스페이스

public class MainMenuController : MonoBehaviour
{
    // 게임 시작 버튼을 누를 때 실행될 메서드
    public void GameStart()
    {
        // Build Settings에 등록된 인덱스 1번 씬(게임 플레이 씬)을 불러옵니다.
        // 또는 씬 이름인 "Stage1"을 문자열로 넣어도 됩니다.
        SceneManager.LoadScene(1); 
    }

    // 게임 종료 버튼을 누를 때 실행될 메서드
    public void GameQuit()
    {
        // 유니티 에디터에서는 작동하는 것처럼 안 보이지만, 실제 빌드된 게임에서는 꺼집니다.
        Debug.Log("게임을 종료합니다.");
        Application.Quit();
    }
}