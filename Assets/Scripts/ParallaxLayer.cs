using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f; // Smaller = farther

    private Transform cam;
    private Vector3 startPos;

    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position;
    }

    void LateUpdate()
    {
        float deltaX = cam.position.x;
        transform.position = new Vector3(
            startPos.x + deltaX * parallaxFactor,
            transform.position.y,
            transform.position.z
        );
    }
}
