using UnityEngine;

public class ParallaxContollerWith2Bg : MonoBehaviour
{
    [Header("Backgrounds")]
    public Transform fondoActual;
    public Transform fondoReserva;
    
    [Header("Camera Settings")]
    public Transform player; // player object
    private Vector3 cameraOffset = new Vector3(0, 3f, -10f); // Camera offset to the player
    private Camera mainCamera;
    private float spriteWidth;
    private float cameraWidth;
    
    void Start()
    {
        mainCamera = Camera.main;

        if (player == null)
        {
            Debug.LogError("Player not assigned in CameraScrollBackground!");
            return;
        }

        // width of the background
        spriteWidth = fondoActual.GetComponent<SpriteRenderer>().bounds.size.x;
        
        // width of the camera
        cameraWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        
        // position init of the bg B
        fondoReserva.position = new Vector3(
            fondoActual.position.x + spriteWidth,
            fondoActual.position.y,
            fondoActual.position.z
        );
        
        // Camera position Start
        mainCamera.transform.position = player.position + cameraOffset;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // camera followe the player
        Vector3 targetPosition = new Vector3(
            player.position.x + cameraOffset.x,
            player.position.y + cameraOffset.y,
            player.position.z + cameraOffset.z
        );
        
        mainCamera.transform.position = targetPosition;

        // switch background
        ScrollBackgrounds();
    }

    void ScrollBackgrounds()
    {
        float cameraX = mainCamera.transform.position.x;
        float cameraRightEdge = cameraX + cameraWidth / 2f;
        float cameraLeftEdge = cameraX - cameraWidth / 2f;

        float fondoCenterX = fondoActual.position.x;

        // swap bg in right side
        if (cameraRightEdge > fondoCenterX + spriteWidth / 2f)
        {
            SwapBackgrounds();
        }

        // swap bg in left side
        if (cameraLeftEdge < fondoCenterX - spriteWidth / 2f)
        {
            SwapBackgroundsLeft();
        }
    }

    void SwapBackgrounds()
    {
        // move bg B to A - right siede
        Vector3 nuevaPosicion = new Vector3(
            fondoActual.position.x + spriteWidth,
            fondoActual.position.y,
            fondoActual.position.z
        );
        fondoReserva.position = nuevaPosicion;

        // swap
        Transform temp = fondoActual;
        fondoActual = fondoReserva;
        fondoReserva = temp;
    }

    void SwapBackgroundsLeft()
    {
        // move bg B to A - left siede
        Vector3 nuevaPosicion = new Vector3(
            fondoActual.position.x - spriteWidth,
            fondoActual.position.y,
            fondoActual.position.z
        );
        fondoReserva.position = nuevaPosicion;

        // swap
        Transform temp = fondoActual;
        fondoActual = fondoReserva;
        fondoReserva = temp;
    }
}