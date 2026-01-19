using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    // actualiza la camara en ultimo paso
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, smoothSpeed * Time.deltaTime);
        transform.position = pos;
    }
}
