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
