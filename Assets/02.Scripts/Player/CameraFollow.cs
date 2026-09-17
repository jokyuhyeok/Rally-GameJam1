using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (target == null) return;

        // 부드러움 없이 그냥 즉시 위치 고정 (레트로 픽셀 감성)
        transform.position = target.position + offset;
    }
}