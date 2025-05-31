using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Посилання на гравця
    public Vector3 offset;         // Зсув відносно гравця
    public float smoothSpeed = 0.125f;  // Швидкість згладжування руху

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}