// using UnityEngine;

// public class CameraFollower : MonoBehaviour
// {
//     public Transform target;
//     public float smoothSpeed = 5f;

//     // actualiza la camara en ultimo paso
//     void LateUpdate()
//     {
//         Vector3 pos = transform.position;
//         pos.x = Mathf.Lerp(pos.x, target.position.x, smoothSpeed * Time.deltaTime);
//         transform.position = pos;
//     }
// }

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
