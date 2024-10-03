using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //Variables
    [SerializeField] public Transform target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float health;
    [SerializeField] public HealthBar healthBar;

    /// <summary>
    /// AJ Wagner - 10/2/2024
    /// Added basic support for the health bar
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    /// <summary>
    /// Ruby 9/20/2024
    /// Makes enemy follow the player
    /// </summary>
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            //Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            //rb.MovePosition(pos);
            //transform.LookAt(target);
        }
    }

    /// <summary>
    /// Nicholas 10/3/2024
    /// Reduces the enemy health value
    /// </summary>
    /// <param name="amount"></param>
    public void takeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
