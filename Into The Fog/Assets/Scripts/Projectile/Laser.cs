using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile
{
    private GenericPool<Laser> pool;
    private float originalLength;

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

        // Make sure the laser is facing the right direction
        Quaternion rotation = Quaternion.LookRotation(caller.transform.forward);
        transform.rotation = rotation * Quaternion.Euler(90, 0, 0);

        despawnAfterTimeCoroutine = StartCoroutine(DespawnAfterTime());

        // Keep track of the laser's orginal length
        originalLength = transform.localScale.y;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy!");
            Enemy enemyController = other.gameObject.GetComponent<Enemy>();
            enemyController.TakeDamage(data.Damage);
        }

        // Resize the laser if it hits a wall
        if (other.gameObject.layer == 6)
        {
            float offset = 0.5f;
            Vector3 rayStartPosition = caller.transform.position + caller.transform.forward * offset;

            // Perform the raycast from in front of the player
            RaycastHit[] hits = Physics.RaycastAll(rayStartPosition, caller.transform.forward, 50);

            float newLength = originalLength;

            foreach (RaycastHit hit in hits)
            {
                // Make sure that the raycast of the laser hitbox itself is not being used 
                if (hit.collider.gameObject != gameObject)
                {
                    newLength = hit.distance;
                    break;
                }
            }

            // Scale the laser to match the new length
            transform.localScale = new Vector3(transform.localScale.x, newLength / 2, transform.localScale.z);

            // Position the laser to fill the space between the caller and the wall
            transform.position = caller.transform.position + caller.transform.forward * (newLength / 2 + offset);
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
            transform.localScale = new Vector3(transform.localScale.x, originalLength, transform.localScale.z);
            pool.ReturnToPool(this);
        }
    }

    /// <summary>
    /// Jay 10/1/2024
    /// Set object pool for the laser
    /// </summary>
    /// <returns></returns>
    public void SetPool(GenericPool<Laser> pool)
    {
        this.pool = pool;
    }
}
