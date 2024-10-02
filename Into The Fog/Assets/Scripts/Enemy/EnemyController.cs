using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Variables
    [SerializeField] public Transform target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float health;

    // ===== | Properties | =====
    public float Health
    {
        get { return health; }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
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
}
