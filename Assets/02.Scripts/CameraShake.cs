using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 _originalPosition; // 흔들리기 전 원래 카메라 위치

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Shake(float duration, float magnitude)
    {
        // 흔들기 시작할 때 현재 카메라 위치를 기억함
        _originalPosition = transform.localPosition;

        // 코루틴 시작 전에 혹시 실행 중인 흔들림이 있으면 정지 (중복 방지)
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        // duration(시간) 동안 반복해서 카메라 위치를 랜덤하게 흔듦
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // 카메라 위치 덜덜덜 이동
            transform.localPosition =
                new Vector3(_originalPosition.x + x, _originalPosition.y + y, _originalPosition.z);

            // Time.timeScale이 0(역경직)이어도 작동하도록 현실 시간으로 계산
            elapsed += Time.unscaledDeltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 흔들림이 끝나면 카메라를 원래 위치로 딱 맞춰줌
        transform.localPosition = _originalPosition;
    }
}