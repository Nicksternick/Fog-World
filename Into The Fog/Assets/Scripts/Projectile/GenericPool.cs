using UnityEngine;
using UnityEngine.Pool;

public class GenericPool<T> where T : MonoBehaviour
{
    private ObjectPool<T> objectPool;
    private T prefab;  // Prefab to instantiate

    public GenericPool(T prefab, int defaultCapacity = 10, int maxSize = 100)
    {
        this.prefab = prefab;

        // Initialize the pool
        objectPool = new ObjectPool<T>(
            CreateInstance,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyInstance,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    /// <summary>
    /// Jay 10/8/2024
    /// Create a new instance from the prefab
    /// </summary>
    private T CreateInstance()
    {
        return Object.Instantiate(prefab);
    }

    /// <summary>
    /// Jay 10/8/2024
    /// When an object is taken from the pool
    /// </summary>
    private void OnTakeFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    /// <summary>
    /// Jay 10/8/2024
    /// When an object is returned to the pool
    /// </summary>
    private void OnReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    /// <summary>
    /// Jay 10/8/2024
    /// When the pool is destroying the object
    /// </summary>
    private void OnDestroyInstance(T obj)
    {
        Object.Destroy(obj.gameObject);
    }
    
    /// <summary>
    /// Gets object from pool
    /// </summary>
    /// <returns> Pooled object </returns>
    public T GetFromPool()
    {
        return objectPool.Get();
    }

    /// <summary>
    /// Jay 10/8/2024
    /// Returns object to pool
    /// </summary>
    public void ReturnToPool(T obj)
    {
        objectPool.Release(obj);
    }
}


