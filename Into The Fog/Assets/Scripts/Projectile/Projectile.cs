using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;  
using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour
{
    protected GameObject caller;             // The object that fired the projectile
    protected float timeUntilDestroy;        // Duration before the projectile is destroyed
    protected ProjectileData data;           // Projectile Base Stats
    protected Vector3 position;              // Current position of the projectile
    protected Vector3 velocity;              // Current velocity of the projectile
    protected quaternion rotation;           // Current rotation of the projectile
    protected Rigidbody rb;                  // Rigidbody

    public abstract void Initialize(float timeUntilDestroy, ProjectileData data);


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

    private void Update()
    {
        transform.position += rb.velocity * Time.deltaTime;

        position = transform.position;
        rotation = quaternion.Euler(transform.rotation.eulerAngles);
    }
}
