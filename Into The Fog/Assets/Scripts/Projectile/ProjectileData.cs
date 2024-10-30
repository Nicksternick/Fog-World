using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileData
{
    private float damage;
    private float size;
    private float speed;
    private bool isSelfDamage;
    private Elements element;

    public ProjectileData(float damage, float size, float speed, bool isSelfDamage, Elements element)
    {
        this.damage = damage;
        this.size = size;
        this.speed = speed;
        this.isSelfDamage = isSelfDamage;
        this.element = element;
    }

    public float Damage => damage;
    public float Size => size;
    public float Speed => speed;
    public bool IsSelfDamage => isSelfDamage;
    public Elements Element => element;


}
