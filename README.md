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
2. Un GameObject empty para controlar los fondos y la velocidad de camara
![alt text](image-4.png) 


Resultado:
La camara mueve hacia derecha, y los fondos intercambian cuando cumple la condicion.
![alt text](Unity_bCrvCehYX0.gif)


## Desplazar el punto a partir del cual se aplica la textura. La parte sobrante se aplica en el espacio vacío.
### Tarea: Aplicar un fondo a tu escena aplicando la técnica del desplazamiento de textura
1. Configuracion de la textura
![alt text](image-6.png)

2. Configuracion de un objeto **Plane** como fondo 
![alt text](image-5.png)

3. Crear un script para aplicar el offset al material
```
void Update()
{
    offsetX += scrollSpeed * Time.deltaTime;

    // Apply the offset to the main texture
    rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
}
```

Resultado:
![alt text](Unity_w9X7hLsj46.gif)

## Tarea: Aplicar efecto parallax usando la técnica de scroll en la que se mueve continuamente la posición del fondo.

## Tarea: Aplicar efecto parallax actualizando el offset de la textura.
1. Igual que los apartados anteriores, hay que configurar las texturas para que **warp mode** sea **repeat**
2. Añadir varias capas y configurar las distancias de cada una respecto al juegador.
![alt text](image-7.png)
3. Crear un script llamado **ParallaxController.cs**
Lo que hace script es que respecto a la camara, calcula la distancia entre la camara y cada capa, las capas más lejanas, se desplazan con menos velocidad, y las más cerca, se desplazan más rápido.
utlizando lo siguiente codigo para que los objetos fondos mueven con la camara y el personaje.
```
transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);
```

Código completo
```
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
```

Resultado: 
![alt text](Unity_VGS2Tzyhwz.gif)

## Tarea: En tu escena 2D crea un prefab que sirva de base para generar un tipo de objetos sobre los que vas a hacer un pooling de objetos que se recolectarán continuamente en tu escena. Cuando un objeto es recolectado debe pasar al pool y dejar de visualizarse. Este objeto estará disponible en el pool. En la escena, siempre que sea posible debe haber una cantidad de objetos que fijes, hasta que el número de objetos que no se han eliminado sea menor que dicha cantidad. Recuerda que para generar los objetos puedes usar el método Instantiate. Los objetos ya creados pueden estar activos o no, para ello usar SetActive.


