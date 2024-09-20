using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Variables
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 4f;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Ruby 9/20/2024
    /// Makes enemy follow the player
    /// </summary>
    void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(pos);
        transform.LookAt(target);
    }
}
