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
