using UnityEngine;
using UnityEngine.Tilemaps;

// 플레이어 방구 스킬을 전담하는 클래스
public class PlayerGasSkill : MonoBehaviour
{
    [SerializeField] private float _gasCost = 5f;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _gasPrefab;

    // 방귀를 뀔 때 팡! 터지는 임팩트 파티클 넣을 빈칸
    [SerializeField] private GameObject _gasImpactParticle;

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
        Vector3 backPosition = transform.position + backDirection;
        Vector3Int backTile = _tilemap.WorldToCell(backPosition);

        if (_tilemap.HasTile(backTile))
        {
            Debug.Log("등 뒤가 벽이라 방귀를 쏠 수 없다.");
            return;
        }

        // 연료 차감을 요청하고 성공했을 때
        if (GameManager.Instance.UseFuel(_gasCost))
        {
            Debug.Log($"뿡! 연료를 {_gasCost}만큼 소모하여 방귀를 뀌엇다.");

            // 벽이 아니면 프리팹 소환
            Instantiate(_gasPrefab, backPosition, Quaternion.identity);

            // 방귀가 나오는 위치에 타격감 200% 임팩트 파티클 터뜨리기!
            if (_gasImpactParticle != null)
            {
                // backDirection(뒤쪽 방향)에 0.5f를 곱해서 절반만 뒤로 가게 만듭니다. (수치를 0.3f로 하면 더 바짝 붙습니다)
                Vector3 impactPosition = transform.position + (backDirection * 0.2f);
                Instantiate(_gasImpactParticle, impactPosition, Quaternion.identity);
            }
        }
        // 연료 차감 요청 실패했을 때
        else
        {
            Debug.Log($"악! 연료가 부족해서 방귀를 뀔 수 없다.");
        }
    }
}