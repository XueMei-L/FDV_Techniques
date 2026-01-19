# FDV_2D_Mechanics

```
>> PRACTICA:   Unity Project - FDV_2D_Mechanics
>> COMPONENTE: XueMei Lin
>> GITHUB:     https://github.com/XueMei-L/FDV_2D_Mechanics.git
>> Versión:    1.0.0
```

# Objetivo
## En este proyecto, reutilizo el proyecto de Mecanicas para trabajar. El objetivo de la sesión es familiarizarse con técnicas empleadas para el scrolling de los fondos, así como el uso de diferentes cámaras en el juego.

## La cámara está fija, el fondo se va desplazando en cada frame. Se usan dos fondos. Uno de ellos lo va viendo la cámara en todo momento, el otro está preparado para el momento en que se ha avanzado hasta el punto en el que
la vista de la cámara ya no abarcaría el fondo inicial. 

### Tarea: Aplicar un fondo con scroll a tu escena utilizando la técnica descrita en a.
1. Eligir una imagen como un fondo con scroll, configurar la imagen
![alt text](image-3.png)

1. Poner los fondos en paralelas
2. Crear un **GameObject Empty** para manejar los fondos
3. Crear un script en **GameObject Empty** para manejar los dos fondos
```
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

```

Resultado: 
![alt text](Unity_U7xMrMExc7.gif)

Resultado2:
![alt text](Unity_EfQqwV6WDz.gif)


## La cámara se desplaza a la derecha y el fondo está estático. 
### Tarea: Aplicar un fondo con scroll a tu escena utilizando la técnica descrita en b.
1. En este caso solo se intercambia cuando el borde derecho de la camara es mayor que la posicion central del fondo
```
// utilizando esta condicion
pos_camera.x + cameraWidth > pos_fondoA.x + spriteWidth/2
```

Resultado:
![alt text](Unity_bCrvCehYX0.gif)

<!-- Resultado mejorado: -->
