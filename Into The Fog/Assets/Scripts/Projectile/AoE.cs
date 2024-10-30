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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy!");
            Enemy enemyController = collision.gameObject.GetComponent<Enemy>();
            enemyController.TakeDamage(25);

        }

        // If the aoe hits the wall
        if (collision.gameObject.layer == 6)
        {
            // Return to object pool
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
