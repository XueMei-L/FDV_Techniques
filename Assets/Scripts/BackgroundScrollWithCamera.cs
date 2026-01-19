using UnityEngine;

public class CameraScrollBackground : MonoBehaviour
{
    [Header("Backgrounds")]
    public Transform fondoActual;
    public Transform fondoReserva;
    
    [Header("Camera Settings")]
    public float cameraMoveSpeed = 5f;
    
    private float spriteWidth;
    private float cameraWidth;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        // Ancho del fondo 
        spriteWidth = fondoActual.GetComponent<SpriteRenderer>().bounds.size.x;
        
        // Ancho de la camara, seria la posicion del punto central * 2 * ancho del encuadre
        cameraWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        
        // Posicion del fondoAuxiliar
        fondoReserva.position = new Vector3(
            fondoActual.position.x + spriteWidth,
            fondoActual.position.y,
            fondoActual.position.z
        );
    }
    void Update()
    {
        // Mover la camara hacia derecha
        mainCamera.transform.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime);
        
        // Obtener la posicion de la camara y el borde derecho de la camara
        float cameraX = mainCamera.transform.position.x;
        float cameraRightEdge = cameraX + cameraWidth;
        
        // Obtener la posicion del centro del fondo
        float fondoCenterX = fondoActual.position.x;
        
        // cuando el borde derecho de la camara es mayor que la posicion del centro del fondo realiza intercambia. 
        // pos_camera.x + cameraWidth > pos_fondoA.x + spriteWidth/2
        if (cameraRightEdge > fondoCenterX + spriteWidth / 2f)
        {
            SwapBackgrounds();
        }
    }
    
    void SwapBackgrounds()
    {
        // calcular nueva posicion de fondoAuxiliar
        Vector3 nuevaPosicion = new Vector3(
            fondoActual.position.x + spriteWidth,
            fondoActual.position.y,
            fondoActual.position.z
        );
        
        // 2. mover el fondo a la nueva posicion
        fondoReserva.position = nuevaPosicion;
        
        // 3. intercambia
        Transform temp = fondoActual;
        fondoActual = fondoReserva;
        fondoReserva = temp;
    }

}