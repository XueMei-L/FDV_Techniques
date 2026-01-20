using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in inspector
    public float smoothSpeed = 0.1f; // Camera smooth movement
    private Vector3 offset; // Initial offset between camera and player

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned!");
        }
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Target position with offset
        Vector3 targetPos = player.position + offset;
        targetPos.z = -10; // Keep camera at fixed Z

        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
    }
}
