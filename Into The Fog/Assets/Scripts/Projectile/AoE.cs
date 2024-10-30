using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE : Projectile
{
    private GenericPool<AoE> pool;

    public override void Initialize(float timeUntilDestroy, ProjectileData data)
    {
        this.timeUntilDestroy = timeUntilDestroy;
        this.data = data;

        despawnAfterTimeCoroutine = StartCoroutine(DespawnAfterTime());
    }

    private void OnDisable()
    {
        // Stop the coroutine if it's running
        if (despawnAfterTimeCoroutine != null)
        {
            StopCoroutine(despawnAfterTimeCoroutine);
            despawnAfterTimeCoroutine = null;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // Check if the other object is tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy!");
            Enemy enemyController = other.GetComponent<Enemy>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(25);
            }
        }

        // Check if the trigger hit an object on the wall layer (layer 6)
        if (other.gameObject.layer == 6)
        {
            if (pool != null)
            {
                pool.ReturnToPool(this);
            }
        }
    }


    /// <summary>
    /// Jay 10/27/2024
    /// Abstract method to despawn this projectile
    /// </summary>
    protected override void DespawnProjectile()
    {
        // Return to pool when despawn time is reached
        if (pool != null)
        {
            pool.ReturnToPool(this);
        }
    }

    /// <summary>
    /// Jay 10/1/2024
    /// Set object pool for the aoe
    /// </summary>
    /// <returns></returns>
    public void SetPool(GenericPool<AoE> pool)
    {
        this.pool = pool;
    }
}
