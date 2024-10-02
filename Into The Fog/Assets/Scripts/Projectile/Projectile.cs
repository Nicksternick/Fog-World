using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Projectile : MonoBehaviour
{
    protected float timeUntilDestroy;
    protected ProjectileData data;
    protected Vector3 position;

    protected Projectile(GameObject caller, float timeUntilDestroy, ProjectileData data)
    {
        position = caller.transform.position;
        this.timeUntilDestroy = timeUntilDestroy;
        this.data = data;
    }

    public Vector3 Position
    {
        get { return position; }
    }

    public abstract void SpawnProjectile();
}
