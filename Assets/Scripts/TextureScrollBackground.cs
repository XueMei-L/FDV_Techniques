using Unity.Mathematics;
using UnityEngine;

public class TextureScrollBackground : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float scrollSpeed = 0.2f; // Texture scrolling speed

    private float offsetX = 0f;
    private Renderer rend;



    void Start()
    {
        // Get the Renderer component
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offsetX += scrollSpeed * Time.deltaTime;

        // Apply the offset to the main texture
        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
    }
}
