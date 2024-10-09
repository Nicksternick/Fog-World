using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // ===== | Variables | =====

    [SerializeField] private InputActionMap playerActions;

    /// <summary>The health of the player</summary>
    [SerializeField] public float health = 100;
    /// <summary>The rigid body of the player</summary>
    [SerializeField] private Rigidbody rb;
    /// <summary>Speed player Moves</summary>
    [SerializeField] private float speed = 5;
    /// <summary>Speed Player Rotates</summary>
    [SerializeField] private float turnSpeed = 720;
    /// <summary>Cooldown value of dash</summary>
    [SerializeField] private float dashCooldown = 5;
    /// <summary>The speed of the dash</summary>
    [SerializeField] private float dashSpeed = 10;
    

    public static event Action<PlayerController> playerDamageEvent;

    /// <summary>Countdown until dash is usable again</summary>
    private float dashCountdown = 0;
    /// <summary>Countdown until dash speed stop</summary>
    private float dashUptime = 0;
    /// <summary>Speed during dash</summary>
    private Vector3 input;

    /// <summary>
    /// AJ Wagner - 10/2/2024
    /// Added basic support for the health bar
    /// </summary>
    void Start()
    {
       
    }

    void Update()
    {
        GatherInput();
        Look();
        Dash();
        CastSpell();
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
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
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
    /// Manages all things dash related
    /// </summary>
    void Dash()
    {
        if (dashCountdown > 0)
        {
            dashCountdown -= Time.deltaTime;
        }
        if (dashUptime > 0)
        {
            dashUptime -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && dashCountdown<=0)
        {
            dashCountdown = dashCooldown;
            dashUptime = 0.25f;
        }
    }

    /// <summary>
    /// Ruby 9/18/2024
    /// This Moves the player
    /// </summary>
    void Move()
    {
        
        //Speed is dependant on dash
        if (dashUptime > 0)
        {
            rb.MovePosition(transform.position + transform.forward * dashSpeed * Time.deltaTime);
        } 
        else
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

    /// <summary>
    /// Jay 10/1/2024
    /// This makes the player cast a spell
    /// </summary>
    void CastSpell()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ProjectileManager.Instance.GetBallFromPool
                (transform.position + transform.forward * 2,
                transform.rotation, this.gameObject,
                Elements.Fire);
        }
        if(Input.GetMouseButtonDown(1))
        {
            ProjectileManager.Instance.GetBallFromPool
                (transform.position + transform.forward * 2,
                transform.rotation, this.gameObject,
                Elements.Ice);
        }
    }

    /// <summary>
    /// Jay 10/2/2024
    /// This makes the player take damage
    /// </summary>
    /// <param name="amount"> Amount of damage </param>
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (playerDamageEvent!=null)
        {
            playerDamageEvent(this);
        }
        /*
        if (health <= 0)
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("GameOver");
        }
        */
    }
}
