using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Header("Camera Settings")]
    private Transform cam;             // camara
    private Vector3 camStartPos;       // Camera start position

    [Header("Background Settings")]
    private GameObject[] backgrounds;  // Array of background layers
    private Material[] mats;           // Materials for each layer
    private float[] backSpeed;         // Parallax speed for each layer - fast for the 1 and slow for the last
    private float farthestBack;        // Z distance of farthest background

    [Range(0.01f, 0.1f)]
    public float parallaxSpeed = 0.05f; // Base parallax speed

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;  // Number of background layers
        backgrounds = new GameObject[backCount];
        mats = new Material[backCount];
        backSpeed = new float[backCount];

        // Initialize arrays
        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mats[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        // calculate parallaxspeed using distance of layer and camera
        CalculateBackSpeed(backCount);
    }

    // Calculate relative speed for each background layer
    void CalculateBackSpeed(int backCount)
    {
        farthestBack = 0f;

        // Find farthest background from camera
        for (int i = 0; i < backCount; i++)
        {
            float zDist = Mathf.Abs(backgrounds[i].transform.position.z - cam.position.z);
            if (zDist > farthestBack)
                farthestBack = zDist;
        }

        // Set speed factor for each layer
        // Closer to the camera - Higher value - Faster scrolling
        // Farther from the camera - Lower value - Slower scrolling - Creates a sense of depth
        for (int i = 0; i < backCount; i++)
        {
            float zDist = Mathf.Abs(backgrounds[i].transform.position.z - cam.position.z);
            backSpeed[i] = 1 - (zDist / farthestBack); // Nearer layers move faster
        }
    }

    void LateUpdate()
    {
        float distance = cam.position.x - camStartPos.x;
        // move with camera
        transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            Vector2 offset = new Vector2(distance * speed, 0);
            mats[i].SetTextureOffset("_MainTex", offset);
        }
    }
}
