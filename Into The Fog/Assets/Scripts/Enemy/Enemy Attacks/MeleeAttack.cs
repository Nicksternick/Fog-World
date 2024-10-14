using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbstractAttack
{
    // ===== | Variables | =====
    [SerializeField] private float damage;
    [SerializeField] private float damageCooldown;
    [SerializeField] private bool canAttack;

    private const float MaxTimer = 2;

    // ===== | Methods | =====
    // Start is called before the first frame update
    void Start()
    {
        damageCooldown = MaxTimer;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack)
        {
            damageCooldown -= Time.deltaTime;

            if (damageCooldown < 0)
            {
                damageCooldown = MaxTimer;
                canAttack = true;
            }
        }
    }
}
