using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Ball : Projectile
{
    private GenericPool<Ball> pool;

    public override void Initialize(float timeUntilDestroy, ProjectileData data)
    {
        this.timeUntilDestroy = timeUntilDestroy;
        this.data = data;

        despawnAfterTimeCoroutine = StartCoroutine(DespawnAfterTime());
        SetVelocity();
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

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy!");
            Enemy enemyController = collision.gameObject.GetComponent<Enemy>();
            enemyController.TakeDamage(25);

            // Return to object pool
            if (pool != null)
            {
                pool.ReturnToPool(this);
            }
        }

        // If the ball hits the wall
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
    /// Set object pool for the ball
    /// </summary>
    /// <returns></returns>
    public void SetPool(GenericPool<Ball> pool)
    {
        this.pool = pool;
    }
}