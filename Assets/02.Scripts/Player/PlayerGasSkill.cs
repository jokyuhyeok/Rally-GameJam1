using UnityEngine;

// 플레이어 방구 스킬을 전담하는 클래스
public class PlayerGasSkill : MonoBehaviour
{
    [SerializeField] private float _gasCost = 5f;

    private PlayerMove _playerMove;

    private void Start()
    {
        // 플레이어에 붙어있는 PlayerMove 컴포넌트를 찾아 가져옴.
        _playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseGasSkill();
        }
    }

    private void UseGasSkill()
    {
        // 연료 차감을 요청하고 성공했을 때
        if (GameManager.Instance.UseFuel(_gasCost))
        {
            Debug.Log($"뿡! 연료를 {_gasCost}만큼 소모하여 방귀를 뀌엇다.");
            Vector3 backDirection = -_playerMove.GetCurrentDirection();
        }
        // 연료 차감 요청 실패했을 때
        else
        {
            Debug.Log($"악! 연료가 부족해서 방귀를 뀔 수 없다.");
        }
    }
}