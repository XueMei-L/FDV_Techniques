using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Backgrounds")]
    [SerializeField] private Transform fondoActual;    // Currently visible background
    [SerializeField] private Transform fondoAuxiliar;  // Auxiliary background (to the right)
    
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 2f;
    
    private float spriteWidth;
    private float posxini;

    void Start()
    {
        SpriteRenderer spriteRenderer = fondoActual.GetComponent<SpriteRenderer>();
        // obtener ancho del fondoA       * ambos son iguales
        spriteWidth = spriteRenderer.bounds.size.x;
        // fondoAuxiliar.position.x = fondoActual.position.x + spriteWidth

        posxini = fondoActual.position.x;
    }

    void Update()
    {
        // en cada frame mueve hacia izquierda ambos fondos
        float moveDistance = scrollSpeed * Time.deltaTime;

        fondoActual.Translate(Vector3.left * moveDistance);
        fondoAuxiliar.Translate(Vector3.left * moveDistance);
        
        // modificar un poco, quitar / 2f, puesto que se queda la mitad y intercambia
        if (fondoActual.position.x < posxini - spriteWidth)
        {
            SwapBackgrounds();
        }
    }

    void SwapBackgrounds()
    {
        // intercambio de fondos
        Transform temp = fondoActual;
        fondoActual = fondoAuxiliar;
        fondoAuxiliar = temp;

        Vector3 newPosition = new Vector3(
            fondoActual.position.x + spriteWidth,
            fondoActual.position.y,
            fondoActual.position.z
        );
        
        fondoAuxiliar.position = newPosition;
    }
}
