using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;  
using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour
{
    protected GameObject caller;                    // The object that fired the projectile
    protected float timeUntilDestroy;               // Duration before the projectile is destroyed
    protected ProjectileData data;                  // Projectile Base Stats
    protected Vector3 position;                     // Current position of the projectile
    protected Vector3 velocity;                     // Current velocity of the projectile
    protected quaternion rotation;                  // Current rotation of the projectile
    protected Rigidbody rb;                         // Rigidbody
    protected Coroutine despawnAfterTimeCoroutine;  // Coroutine that auto despawns projectile


    public abstract void Initialize(float timeUntilDestroy, ProjectileData data);
    protected abstract void DespawnProjectile();


    public Vector3 Position => position;   
    public Quaternion Rotation => (Quaternion)rotation;   
    public ProjectileData Data => data;

    /// <summary>
    /// The game object that spawned this
    /// </summary>
    public GameObject Caller
    {
         get{ return caller; }
         set{ caller = value; } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Jay 11/5/24
    /// Stop the coroutine if this is disabled
    /// </summary>
    protected void OnDisable()
    {
        // Stop the coroutine if it's running
        if (despawnAfterTimeCoroutine != null)
        {
            StopCoroutine(despawnAfterTimeCoroutine);
            despawnAfterTimeCoroutine = null;
        }
    }

    /// <summary>
    /// Jay 11/5/2024
    /// Apply a vector in the direction that the caller is facing with
    /// the projectile's speed stat
    /// </summary>
    public virtual void SetVelocity()
    {
        if (caller != null)
        {
            Vector3 direction = caller.transform.forward; 
            rb.velocity = direction * data.Speed;        
        }
    }

    /// <summary>
    /// Jay 11/5/2024
    /// *** Note due to how pooling is set up most projectile forms
    /// will have an override of this is similar code because the 
    /// related object pool can only be referenced in each individual
    /// child class
    /// 
    /// Used when a solid projectile hits anything
    /// </summary>
    /// <param name="collision"> The collsion object created </param>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(data.Element != Elements.Fire)
        {
            // Get the enemy's debuff script
            Debuff enemyDebuff = collision.gameObject.GetComponent<Debuff>();

            if (enemyDebuff != null)
            {
                // Apply the debuff with the specified element type
                enemyDebuff.TriggerDebuff(data.Element);
            }
        }
    }

    /// <summary>
    /// Jay 11/5/2024
    /// *** Note due to how pooling is set up most projectile forms
    /// will have an override of this is similar code because the 
    /// related object pool can only be referenced in each individual
    /// child class
    /// 
    /// Used when a trigger projectile hits anything
    /// </summary>
    /// <param name="collision"> The collsion object created </param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (data.Element != Elements.Fire)
        {
            // Get the enemey's debuff script
            Debuff enemyDebuff = other.gameObject.GetComponent<Debuff>();

            if (enemyDebuff != null)
            {
                // Apply the debuff with the specified element type
                enemyDebuff.TriggerDebuff(data.Element);
            }
        }

    }

    /// <summary>
    /// Despawns the projectile after a set time
    /// </summary>
    /// <returns> The result for the coroutine </returns>
    protected virtual IEnumerator DespawnAfterTime()
    {
        float elapsedTime = 0f;

        // Wait until the timeUntilDestroy period is completed
        while (elapsedTime < timeUntilDestroy)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait till next frame
        }
        
        DespawnProjectile();
    }

    /// <summary>
    /// Update For moving projectiles
    /// </summary>
    private void Update()
    {
        transform.position += rb.velocity * Time.deltaTime;

        position = transform.position;
        rotation = quaternion.Euler(transform.rotation.eulerAngles);
    }
}
