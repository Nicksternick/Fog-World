using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables

    [SerializeField] private Rigidbody rb;
    //Speed player Moves
    [SerializeField] private float speed = 5;
    //Speed Player Rotates
    [SerializeField] private float turnSpeed = 720;
    private Vector3 input;

    void Update()
    {
        GatherInput();
        Look();
    }

    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Ruby 9/18/2024
    /// This gathers the user input
    /// </summary>
    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0,Input.GetAxisRaw("Vertical"));
    }
    
    /// <summary>
    /// Ruby 9/18/2024
    /// This manages the rotation of the player based on user input
    /// </summary>
    void Look()
    {

        if (input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

            var skewedInput = matrix.MultiplyPoint3x4(input);

            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }

    }

    /// <summary>
    /// Ruby 9/18/2024
    /// This Moves the player
    /// </summary>
    void Move()
    {
        //Makes the player stop when not moving
        //Also prevents speed increasing from diagonals (issue with multiplying transform.forward by input.magnitude)
        //If you have a better way to do this feel free to change
        float toMove;
        if (input.magnitude != 0)
        {
            toMove = 1;
        }
        else
        {
            toMove = 0;
        }

        rb.MovePosition(transform.position + (transform.forward * toMove) * speed * Time.deltaTime);
    }
}
