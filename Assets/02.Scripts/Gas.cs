using UnityEngine;

public class Gas : MonoBehaviour
{
    [SerializeField] private float _lifetime = 3f;
    private void Start()
    {
        // 자기 자신을 일정 시간 이후 파괴한다. 
        Destroy(gameObject, _lifetime);
    }
}
