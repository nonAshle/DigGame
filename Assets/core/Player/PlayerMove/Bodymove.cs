using UnityEngine;

public class Bodymove : MonoBehaviour
{
    public Transform target;
    public float smoothness = 5f;
    private Vector3 offset;

    void Start()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothness * Time.deltaTime);
    }
}