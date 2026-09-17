using UnityEngine;
using UnityEngine.Tilemaps;

// 플레이어 방구 스킬을 전담하는 클래스
public class PlayerGasSkill : MonoBehaviour
{
    [SerializeField] private float _gasCost = 5f;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _gasPrefab;

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
        // 등 뒤 방향 계산
        Vector3 backDirection = -_playerMove.GetCurrentDirection();

        // 바로 뒤가 벽인가?
        Vector3 firstBackPosition = transform.position + backDirection;
        Vector3Int firstBackTile = _tilemap.WorldToCell(firstBackPosition);

        if (_tilemap.HasTile(firstBackTile))
        {
            Debug.Log("등 뒤가 벽이라 방귀를 쏠 수 없다.");
        }

        // 연료 차감을 요청하고 성공했을 때
        if (GameManager.Instance.UseFuel(_gasCost))
        {
            Debug.Log($"뿡! 연료를 {_gasCost}만큼 소모하여 방귀를 뀌엇다.");

            // 반복문으로 뒤로 3칸까지 벽이 없는지 체크한다.
            for (int i = 1; i <= 3; i++)
            {
                Vector3 gasPosition = transform.position + (backDirection * i);
                Vector3Int tilePosition = _tilemap.WorldToCell(gasPosition);

                if (_tilemap.HasTile(tilePosition)) // 중간에 반복문 등장시 즉시 종료
                {
                    break;
                }

                // 벽이 아니면 프리팹 소환
                Instantiate(_gasPrefab, gasPosition, Quaternion.identity);
            }
        }
        // 연료 차감 요청 실패했을 때
        else
        {
            Debug.Log($"악! 연료가 부족해서 방귀를 뀔 수 없다.");
        }
    }
}