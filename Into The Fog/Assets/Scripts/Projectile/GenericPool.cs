using UnityEngine;
using UnityEngine.Pool;

public class GenericPool<T> where T : MonoBehaviour
{
    private ObjectPool<T> objectPool;
    private T prefab;  // Prefab to instantiate

    public GenericPool(T prefab, int defaultCapacity = 10, int maxSize = 100)
    {
        this.prefab = prefab;

        // Initialize the pool, no need to pass functions for creating, activating, or deactivating.
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

    // Create a new instance from the prefab
    private T CreateInstance()
    {
        return Object.Instantiate(prefab);
    }

    // When an object is taken from the pool
    private void OnTakeFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    // When an object is returned to the pool
    private void OnReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    // When the pool is destroying the object
    private void OnDestroyInstance(T obj)
    {
        Object.Destroy(obj.gameObject);
    }

    public T GetFromPool()
    {
        return objectPool.Get();
    }

    public void ReturnToPool(T obj)
    {
        objectPool.Release(obj);
    }
}


