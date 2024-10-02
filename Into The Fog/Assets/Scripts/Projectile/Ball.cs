using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Ball : Projectile
{
    private ObjectPool<Ball> pool;

    public Ball(GameObject caller, float timeUntilDestroy, ProjectileData data) 
        : base(caller, timeUntilDestroy, data)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    public override void SpawnProjectile()
    {

    }

    public virtual void SetPool(ObjectPool<Ball> pool)
    {
        this.pool = pool;
    }
}
