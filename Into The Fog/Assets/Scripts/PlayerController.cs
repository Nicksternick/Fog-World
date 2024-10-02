using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables

    private float hp;

    [SerializeField] private Rigidbody rb;
    //Speed player Moves
    [SerializeField] private float speed = 5;
    //Speed Player Rotates
    [SerializeField] private float turnSpeed = 720;
    //Cooldown value of dash
    [SerializeField] private float dashCooldown = 5;
    //Countdown until dash is usable again
    [SerializeField] private float dashCountdown = 0;
    //Countdown until dash speed stop
    [SerializeField] private float dashUptime = 0;
    //Speed during dash
    [SerializeField] private float dashSpeed = 10;
    private Vector3 input;

    [SerializeField] private ProjectileManager projectileManager;

    public float PlayerHp 
    { 
        get { return hp; } 
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
            projectileManager.GetBallFromPool
                (transform.position + transform.forward * 2,
                transform.rotation, this.gameObject,
                ProjectileManager.Elements.Fire);
        }
        if(Input.GetMouseButtonDown(1))
        {
            projectileManager.GetBallFromPool
                (transform.position + transform.forward * 2,
                transform.rotation, this.gameObject,
                ProjectileManager.Elements.Ice);
        }
    }

    /// <summary>
    /// Jay 10/2/2024
    /// This makes the player take damage
    /// </summary>
    /// <param name="amount"> Amount of damage </param>
    void takeDamage(float amount)
    {
        hp -= amount;
    }
}
