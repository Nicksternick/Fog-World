using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbstractAttack
{
    // ===== | Variables | =====
    [SerializeField] private float damage;
    [SerializeField] private float damageModifier = 2;

    private float damageCooldown;
    private bool canAttack;

    [SerializeField] private float attackCooldown = 2;
    [SerializeField] private float knockBackForce;

    // ===== | Methods | =====
    // Start is called before the first frame update
    void Start()
    {
        damage += damageModifier * GameManager.Instance.CurrentLevel;
        damageCooldown = attackCooldown;
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
                damageCooldown = attackCooldown;
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
            rb.AddForce((player.transform.position - transform.parent.position).normalized * knockBackForce, ForceMode.Impulse);
            //CalculatePushForce(knockBackForce, player.transform.position - transform.parent.position)
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
