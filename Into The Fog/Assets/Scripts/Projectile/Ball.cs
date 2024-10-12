using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Ball : Projectile
{
    private ObjectPool<Ball> ballPool;
    protected Coroutine despawnAfterTimeCoroutine;

    public override void Initialize(float timeUntilDestroy, ProjectileData data)
    {
        this.timeUntilDestroy = timeUntilDestroy;
        this.data = data;

        despawnAfterTimeCoroutine = StartCoroutine(DespawnBallAfterTime());
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy!");
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.TakeDamage(25);

            // Return to object pool
            if (ballPool != null)
            {
                ballPool.Release(this);
            }
        }

        
    }

    /// <summary>
    /// Jay 10/1/2024
    /// Despawn ball after set amount of time
    /// </summary>
    /// <returns></returns>
    private IEnumerator DespawnBallAfterTime()
    {
        float elapsedTime = 0f;

        // Wait until the timeUntilDestroy period is completed
        while (elapsedTime < timeUntilDestroy)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait till next frame
        }

        // Return to object pool
        if (ballPool != null)
        {
            ballPool.Release(this);
        }
    }

    /// <summary>
    /// Jay 10/1/2024
    /// Set object pool for the ball
    /// </summary>
    /// <returns></returns>
    public virtual void SetPool(ObjectPool<Ball> pool)
    {
        ballPool = pool;
    }
}
