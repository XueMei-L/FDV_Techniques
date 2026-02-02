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
![alt text](imgs/image-3.png)

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
![alt text](imgs/Unity_U7xMrMExc7.gif)

Resultado2:
![alt text](imgs/Unity_EfQqwV6WDz.gif)


## La cámara se desplaza a la derecha y el fondo está estático. 
### Tarea: Aplicar un fondo con scroll a tu escena utilizando la técnica descrita en b.
1. En este caso solo se intercambia cuando el borde derecho de la camara es mayor que la posicion central del fondo
```
// utilizando esta condicion
pos_camera.x + cameraWidth > pos_fondoA.x + spriteWidth/2
```
1. Un GameObject empty para controlar los fondos y la velocidad de camara
![alt text](imgs/image-4.png) 


Resultado:
La camara mueve hacia derecha, y los fondos intercambian cuando cumple la condicion.
![alt text](imgs/Unity_bCrvCehYX0.gif)


## Desplazar el punto a partir del cual se aplica la textura. La parte sobrante se aplica en el espacio vacío.
### Tarea: Aplicar un fondo a tu escena aplicando la técnica del desplazamiento de textura
1. Configuracion de la textura
![alt text](imgs/image-6.png)

1. Configuracion de un objeto **Plane** como fondo 
![alt text](imgs/image-5.png)

1. Crear un script para aplicar el offset al material
```
void Update()
{
    offsetX += scrollSpeed * Time.deltaTime;

    // Apply the offset to the main texture
    rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
}
```

Resultado:
![alt text](imgs/Unity_w9X7hLsj46.gif)

## Tarea: Aplicar efecto parallax usando la técnica de scroll en la que se mueve continuamente la posición del fondo.
1. Reutilizar el codigo de **ScrollBackground.cs**, añadiendo el intercambio cuando el usuario hacia izquierda
2. La condicion que activa el intercambio de fondos es: cuando el borde de la camara sea mayor que el borde izq o el borde der del fondo,
3. Aplicar el parallax a cada layer, cuando sea más cerca mueve más rápido y cuando sea más lejos mueve más lento.
![alt text](imgs/image-8.png)

Resultado:
Intercambia según izquierda o derecha, consigue scroll ambos lados
![alt text](imgs/Unity_duVFXjwsCU.gif)

Resultado:
Problema, puesto que las capas lejas están más lejos no se actualizan como la primera capa
![alt text](imgs/Unity_hz6i5DZWhf.gif)


## Tarea: Aplicar efecto parallax actualizando el offset de la textura.
1. Igual que los apartados anteriores, hay que configurar las texturas para que **warp mode** sea **repeat**
2. Añadir varias capas y configurar las distancias de cada una respecto al juegador.
![alt text](imgs/image-7.png)
1. Crear un script llamado **ParallaxController.cs**
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
![alt text](imgs/Unity_VGS2Tzyhwz.gif)

## Tarea: En tu escena 2D crea un prefab que sirva de base para generar un tipo de objetos sobre los que vas a hacer un pooling de objetos que se recolectarán continuamente en tu escena. Cuando un objeto es recolectado debe pasar al pool y dejar de visualizarse. Este objeto estará disponible en el pool. En la escena, siempre que sea posible debe haber una cantidad de objetos que fijes, hasta que el número de objetos que no se han eliminado sea menor que dicha cantidad. Recuerda que para generar los objetos puedes usar el método Instantiate. Los objetos ya creados pueden estar activos o no, para ello usar SetActive.

1. Crar un objeto vacio para controlar pooling objetos llamado **GameManager**
2. Crear prefab para los objetos collectibles.
![alt text](imgs/image-9.png)
3. Crear un script para manejo de pool, la lógica seria, inicialmente en el pool tiene maxCoin * 2 de cantidad de estrellas, inicialmente todos están oculto. Activa una cierta cantidad de estrellas en la escena, cuando la cantidad de estrella es menor que la cantidad pretefinida, genera nueva estrella con la posición random en la escena. y cuando el jugador coleciona una estrealla, dicha estrella será oculta y vuelva al pooling.
3. Codigos:

*ObjectPoolManager.cs* para el control de estrellas y pooling
```
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("Prefabs & Pool Settings")]
    public GameObject starCoinPrefab;   // Star prefab
    public int maxCoins = 5;           // Number of active coins at a time
    public Transform coinsParent;       // Parent object for organization

    private List<GameObject> pool = new List<GameObject>();        // pooling objects
    private List<GameObject> activeCoins = new List<GameObject>(); // Active coins in scene

    void Start()
    {
        // Initialize the pool with extra coins
        for (int i = 0; i < maxCoins * 2; i++)
        {
            GameObject coin = Instantiate(starCoinPrefab, coinsParent);
            coin.GetComponent<StarCoinPerfab>().Init(this);
            coin.SetActive(false);
            pool.Add(coin);
        }

        // Spawn initial active coins
        for (int i = 0; i < maxCoins; i++)
        {
            SpawnCoin();
        }
    }

    void Update()
    {
        // Keep maxCoins in the scene
        while (activeCoins.Count < maxCoins)
        {
            SpawnCoin();
        }
    }

    // get the new star coin then add to activeCoins list
    public void SpawnCoin()
    {
        GameObject coin;

        if (pool.Count > 0)
        {
            coin = pool[0];
            pool.RemoveAt(0);
        }
        else
        {
            // If pool is empty, instantiate a new coin
            coin = Instantiate(starCoinPrefab, coinsParent);
            coin.GetComponent<StarCoinPerfab>().Init(this);
        }

        // Activate and place coin
        coin.SetActive(true);
        coin.transform.position = GetRandomPosition();
        activeCoins.Add(coin);
    }

    // when the player get the starcoin, dont destroy it, use the SetActive to hide it and add to the pool
    public void ReturnToPool(GameObject coin)
    {
        if (activeCoins.Contains(coin))
            activeCoins.Remove(coin);

        coin.SetActive(false);

        if (!pool.Contains(coin))
            pool.Add(coin);
    }

    // set the new position of the starcoin
    Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(-20f, 50f),        // position x
            Random.Range(-0.5f, 1f)         // position y
        );
    }
}
```

*StarCoinPerfab.cs*
```
using UnityEngine;

public class StarCoinPerfab : MonoBehaviour
{
    private ObjectPoolManager pool;

    public void Init(ObjectPoolManager poolManager)
    {
        pool = poolManager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Return coin to pool instead of destroying
            pool.ReturnToPool(gameObject);
        }
    }
}
```

Resultado: 
![alt text](Code_01Ftad1iQ4.gif)

Tarea: Revisa tu código de la entrega anterior e indica las mejoras que podrías hacer de cara al rendimiento.
Opinión personal: personalmente considero usar efecto Parallax, puesto que 