using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileData
{
    private float damage;
    private float size;
    private float cooldown;
    private bool isSelfDamage;

    public ProjectileData(float damage, float size, float cooldown, bool isSelfDamage)
    {
        this.damage = damage;
        this.size = size;
        this.cooldown = cooldown;
        this.isSelfDamage = isSelfDamage;
    }
}
