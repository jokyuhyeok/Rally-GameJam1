using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.5f; // 위로 올라가는 속도
    [SerializeField] private float _destroyTime = 1f; // 몇 초 뒤에 사라질지

    private TextMeshPro _textMesh;
    private float _alpha = 1f;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();

        // 생성되고 destroyTime(1초) 뒤에 자기 자신을 파괴함
        Destroy(gameObject, _destroyTime);
    }

    private void Update()
    {
        // 1. 위로 둥실둥실 올라가기
        transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);

        // 2. 투명도 깎아서 스르륵 사라지게 만들기
        if (_textMesh != null)
        {
            _alpha -= Time.deltaTime / _destroyTime; // 시간에 맞춰 0까지 깎임
            Color color = _textMesh.color;
            color.a = Mathf.Max(_alpha, 0f);
            _textMesh.color = color;
        }
    }

    // 밖에서 점수를 넣어주는 함수 (예: +10, +50 등)
    public void SetText(string text)
    {
        if (_textMesh == null) _textMesh = GetComponent<TextMeshPro>();
        _textMesh.text = text;
    }
}