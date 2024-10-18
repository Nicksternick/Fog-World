using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbstractAttack
{
    // ===== | Variables | =====
    private float damage;
    private float damageCooldown;
    private bool canAttack;

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

    public override void Attack()
    {
        if(!playerInAttackRange)
            return;

        if (canAttack)
        {
            canAttack = false;
            FogPlayer player = playerCollider.gameObject.GetComponent<FogPlayer>();
            Rigidbody rb = player.GetComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.AddForce(CalculatePushForce(30, player.transform.position - transform.position), ForceMode.Impulse);

            float damage = Random.Range(8, 13);
            player.TakeDamage(damage);
        }
    }

    /// <summary>
    /// Nicholas 9/30/2024
    /// Calculates the force to apply to the player
    /// </summary>
    /// <param name="pushForce"></param>
    /// <returns></returns>
    private Vector3 CalculatePushForce(float pushForce, Vector3 direction)
    {
        direction = direction.normalized;
        return new Vector3(direction.x, direction.y) * pushForce;
    }
}
