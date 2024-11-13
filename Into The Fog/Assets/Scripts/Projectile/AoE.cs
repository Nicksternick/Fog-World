using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE : Projectile
{
    private GenericPool<AoE> pool;

    /// <summary>
    /// Jay 11/5/24
    /// Initialization of the projectile 
    /// </summary>
    /// <param name="timeUntilDestroy"> When to despawn </param>
    /// <param name="data"> Projectile Stats </param>
    public override void Initialize(float timeUntilDestroy, ProjectileData data)
    {
        this.timeUntilDestroy = timeUntilDestroy;
        this.data = data;

        despawnAfterTimeCoroutine = StartCoroutine(DespawnAfterTime());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // Check if the other object is tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Hit Enemy!");
            Enemy enemyController = other.GetComponent<Enemy>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(data.Damage);
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
