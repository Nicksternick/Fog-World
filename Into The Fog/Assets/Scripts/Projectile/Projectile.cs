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

    public GameObject Caller
    {
         get{ return caller; }
         set{ caller = value; } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void SetVelocity()
    {
        if (caller != null)
        {
            Vector3 direction = caller.transform.forward; 
            rb.velocity = direction * data.Speed;        
        }
    }

    public void ChangeColor(UnityEngine.Color color)
    {
        Renderer renderer = this.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debuff enemyDebuff = collision.gameObject.GetComponent<Debuff>();
        if (enemyDebuff != null)
        {
            // Apply the debuff with the specified element type
            enemyDebuff.TriggerDebuff(data.Element);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debuff enemyDebuff = other.gameObject.GetComponent<Debuff>();
        if (enemyDebuff != null)
        {
            // Apply the debuff with the specified element type
            enemyDebuff.TriggerDebuff(data.Element);
        }
    }

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

    private void Update()
    {
        transform.position += rb.velocity * Time.deltaTime;

        position = transform.position;
        rotation = quaternion.Euler(transform.rotation.eulerAngles);
    }
}
