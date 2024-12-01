using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Nicholas 10/8/24
/// The core class for the player in the 3D levels
/// </summary>
public class FogPlayer : MonoBehaviour
{
    // ===== | Variables | =====

    [Header("Player Attributes")]
    // ----- | Player Attributes | -----
    [SerializeField] private float health = 100;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float dashSpeed = 1;
    [SerializeField] private float dashCooldown = 1;
    [SerializeField] private float dashTime = 1;

    [Header("Player Inputs")]
    // ----- | Inputs from PlayerInput | -----
    [SerializeField] private PlayerInput input;
    private InputAction move;
    private InputAction look;

    [Header("Camera")]
    // ----- | Camera SubObjects | -----
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera mapCamera;

    [Header("Collider And Model")]
    // ----- | Collider/Model SubObject | -----
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private Transform playerModel;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 dashDirection = Vector3.zero;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer;

    private Spell spell1;
    private Spell spell2;
    private Spell spell3;
    private Spell spell4;

    [SerializeField] private Animator animator;

    private float spell1Countdown = 3.0f;
    private float spell2Countdown = 3.0f;
    private float spell3Countdown = 3.0f;
    private float spell4Countdown = 3.0f;

    public static event Action<FogPlayer> playerDamageEvent;

    [SerializeField] private GameObject controllerAimArrow;

    // ===== | Properties | =====
    public float Health
    {
        get { return health; }
    }

    // ===== | Methods | =====
    private void Awake()
    {
        move = input.actions.FindAction("Move");
        look = input.actions.FindAction("Look");
        //dashTimer = gameObject.AddComponent<Timer>();
    }

    void Start()
    {
        UIManager.Instance.StaminaBar.SetMaxHealth(dashCooldown);
        UIManager.Instance.StaminaBar.SetHealth(dashCooldown);

        spell1 = GameManager.Instance.GetPlayerSpell(0);
        spell2 = GameManager.Instance.GetPlayerSpell(1);
        spell3 = GameManager.Instance.GetPlayerSpell(2);
        spell4 = GameManager.Instance.GetPlayerSpell(3);

        if (spell1 == null)
            UIManager.Instance.Spell1.gameObject.SetActive(false);
        else
        {
            UIManager.Instance.Spell1.SetMaxCooldown(spell1.Cooldown);
            spell1Countdown = spell1.Cooldown;
        }


        if (spell2 == null)
            UIManager.Instance.Spell2.gameObject.SetActive(false);
        else
        {
            UIManager.Instance.Spell2.SetMaxCooldown(spell2.Cooldown);
            spell2Countdown = spell2.Cooldown;
        }

        if (spell3 == null)
            UIManager.Instance.Spell3.gameObject.SetActive(false);
        else
        {
            UIManager.Instance.Spell3.SetMaxCooldown(spell3.Cooldown);
            spell3Countdown = spell3.Cooldown;
        }

        if (spell4 == null)
            UIManager.Instance.Spell4.gameObject.SetActive(false);
        else
        {
            UIManager.Instance.Spell4.SetMaxCooldown(spell4.Cooldown);
            spell4Countdown = spell4.Cooldown;
        }

        
        
        
        
        UIManager.Instance.HealthBar.SetMaxHealth(health);

        input.onControlsChanged += (PlayerInput input) =>
        {
            controllerAimArrow.gameObject.SetActive(input.currentControlScheme.Equals("Gamepad"));
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mapCamera.gameObject.SetActive(!mapCamera.gameObject.activeSelf);
        }

        LookAtMouse();

        if (spell1 != null)
        {
            if (spell1Countdown < spell1.Cooldown)
            {
                spell1Countdown += Time.deltaTime;
                UIManager.Instance.Spell1.SetCooldown(spell1Countdown);
            }
        }

        if (spell2 != null)
        {
            if (spell2Countdown < spell2.Cooldown)
            {
                spell2Countdown += Time.deltaTime;
                UIManager.Instance.Spell2.SetCooldown(spell2Countdown);
            }
        }

        if (spell3 != null)
        {
            if (spell3Countdown < spell3.Cooldown)
            {
                spell3Countdown += Time.deltaTime;
                UIManager.Instance.Spell3.SetCooldown(spell3Countdown);
            }
        }

        if (spell4 != null)
        {
            if (spell4Countdown < spell4.Cooldown)
            {
                spell4Countdown += Time.deltaTime;
                UIManager.Instance.Spell4.SetCooldown(spell4Countdown);
            }
        }

        if (!canDash && !isDashing)
        {
            dashTimer += Time.deltaTime;
            UIManager.Instance.StaminaBar.SetHealth(dashTimer);
            if (dashTimer > dashCooldown)
            {
                EndDashCooldown();
            }
        }

        if (Health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        else if (Health < UIManager.Instance.HealthBar.MaxValue)
        {
            health += 1 * Time.deltaTime;
            UIManager.Instance.HealthBar.SetHealth(health);
        }
    }

    private void FixedUpdate()
    {
        if (!mapCamera.gameObject.activeSelf)
        {
            if (!isDashing)
            {
                NormalMove();
            }
            else
            {
                DashMove();
            }
        }
    }

    // ----- | Movement Functions | -----

    /// <summary>
    /// Nicholas 10/9/2024
    /// Finds the axis to move on, then moves 
    /// along that axis based on the given inputs
    /// </summary>
    private void NormalMove()
    {
        // Only look for the axis if
        // the input action is not null
        if (move != null)
        {
            // Get the input for move and cast it to a vector 2
            Vector2 readValue = move.ReadValue<Vector2>().normalized;

            // Get the right and forward vectors from the camera's orientation
            Vector3 cameraRight = playerCamera.transform.right;
            Vector3 cameraForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;

            // Combine the input with camera orientation to create movement direction
            moveDirection = (cameraRight * readValue.x + cameraForward * readValue.y).normalized;

            if (readValue == Vector2.zero)
            {
                animator.SetBool("IsMoving", false);
            }
            else
            {
                animator.SetBool("IsMoving", true);
            }
        }

        // Move the player based on the movement speed
        playerRigidBody.MovePosition(transform.position + (moveDirection * moveSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Nicholas 10/9/2024
    /// Moves the player using the dash functionality
    /// </summary>
    private void DashMove()
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y = transform.localScale.y / 2;

        // Get a ray based on the mouses current position
        Ray ray = new Ray(rayOrigin, dashDirection);

        float distance = dashSpeed * Time.deltaTime;
        RaycastHit hit;

        // Check the to see if there is a point that the mouse hit
        if (Physics.Raycast(ray, out hit, distance))
        {
            Vector3 position = hit.point;
            position.x -= (transform.localScale.x / 2) * dashDirection.x;
            position.z -= (transform.localScale.z / 2) * dashDirection.z;
            position.y = 0;
            playerRigidBody.transform.position = position;
            EndDash();
        }

        dashTimer += Time.deltaTime;
        if (dashTimer > dashTime)
            EndDash();

        playerRigidBody.MovePosition(transform.position + (dashDirection * dashSpeed * Time.deltaTime));
    }

    // ----- | Dashing Functions | -----

    /// <summary>
    /// Nicholas 10/9/2024
    /// Reads an input and then switches the player movement to dashing
    /// </summary>
    public void Dash(InputAction.CallbackContext callback)
    {
        // Check to see if the input is pressed
        // when the player is able to dash
        if (callback.started && canDash)
        {
            // Get the current direction of the player
            dashDirection = moveDirection;

            // Setup the variables so the player
            // can enter the dashing state
            canDash = false;
            isDashing = true;

            dashTimer = 0;

            animator.SetBool("IsSprinting", true);

            // Setup the timer to stop dashing when the time is over
            //dashTimer.SetMaxTime(dashTime);
            //dashTimer.OnCountDownEnd.RemoveAllListeners();
            //dashTimer.OnCountDownEnd.AddListener(EndDash);
            //dashTimer.StartTimer();
        }
    }

    /// <summary>
    /// Nicholas 10/9/2024
    /// Ends the cooldown so the player can dash again
    /// </summary>
    private void EndDashCooldown() { canDash = true; }

    /// <summary>
    /// Nicholas 10/9/2024
    /// Ends the dashing state and returns 
    /// the player to normal movement
    /// </summary>
    private void EndDash()
    {
        // Setup the variables again
        isDashing = false;
        canDash = false;

        dashTimer = 0;

        animator.SetBool("IsSprinting", false);

        // Set up the timer to call EndDashCooldown
        //dashTimer.SetMaxTime(dashCooldown);
        //dashTimer.OnCountDownEnd.RemoveAllListeners();
        //dashTimer.OnCountDownEnd.AddListener(EndDashCooldown);
        //dashTimer.StartTimer();
    }

    // ----- | Other Functions | -----

    public void CastSpell1(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            if (spell1 != null)
            {
                if (spell1Countdown >= spell1.Cooldown)
                {
                    spell1Countdown = 0.0f;
                    spell1.CastSpell(playerModel.gameObject);

                    AudioManager.Instance.PlaySound("Cast Spell", 0.6f);

                    animator.SetTrigger("SpellCasted");
                }
                else
                {
                    AudioManager.Instance.PlaySound("Fail Spell", 0.6f);
                }
            }
        }
    }

    public void CastSpell2(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            if (spell2 != null)
            {
                if (spell2Countdown >= spell2.Cooldown)
                {
                    spell2Countdown = 0.0f;
                    spell2.CastSpell(playerModel.gameObject);

                    AudioManager.Instance.PlaySound("CastSpell", 0.6f);

                    animator.SetTrigger("SpellCasted");
                }
                else
                {
                    AudioManager.Instance.PlaySound("Fail Spell", 0.6f);
                }
            }
        }
    }

    public void CastSpell3(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            if (spell3 != null)
            {
                if (spell3Countdown >= spell3.Cooldown)
                {
                    spell3Countdown = 0.0f;
                    spell3.CastSpell(playerModel.gameObject);

                    AudioManager.Instance.PlaySound("Cast Spell", 0.6f);

                    animator.SetTrigger("SpellCasted");
                }
                else
                {
                    AudioManager.Instance.PlaySound("Fail Spell", 0.6f);
                }
            }
        }
    }

    public void CastSpell4(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            if (spell4 != null)
            {
                if (spell4Countdown >= spell4.Cooldown)
                {
                    spell4Countdown = 0.0f;
                    spell4.CastSpell(playerModel.gameObject);

                    AudioManager.Instance.PlaySound("Cast Spell", 0.6f);

                    animator.SetTrigger("SpellCasted");
                }
                else
                {
                    AudioManager.Instance.PlaySound("Fail Spell", 0.6f);
                }
            }
        }
    }

    /// <summary>
    /// Nicholas 10/8/24
    /// Does a raycast to determine where the mouse is,
    /// then makes the player face the position of the mouse
    /// </summary>
    private void LookAtMouse()
    {
        // Setup the plane and distance for later in the method
        Plane plane = new Plane(Vector3.up, 0);
        float distance;

        // Get a ray based on the mouses current position
        // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (input.currentControlScheme == "Keyboard Mouse")
        {
            Ray ray = playerCamera.ScreenPointToRay(look.ReadValue<Vector2>());

            // Check the to see if there is a point that the mouse hit
            if (plane.Raycast(ray, out distance))
            {
                // Get that point
                Vector3 mousePosition = ray.GetPoint(distance);

                // Make the player look at it, then reset it's x and z rotation
                playerModel.transform.LookAt(mousePosition, Vector3.up);
                Quaternion rotation = playerModel.transform.rotation;
                rotation.x = 0;
                rotation.z = 0;
                playerModel.transform.rotation = rotation;
            }
        }
        else
        {
            Vector3 rotation = look.ReadValue<Vector2>().normalized;

            if (rotation == Vector3.zero)
            {
                controllerAimArrow.gameObject.SetActive(false);
                return;
            }

            controllerAimArrow.gameObject.SetActive(true);

            Vector3 cameraRight = playerCamera.transform.right;
            Vector3 cameraForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;

            Vector3 playerRotation = cameraRight * rotation.x + cameraForward * rotation.y;
            Quaternion newQuat = Quaternion.LookRotation(playerRotation, Vector3.up);

            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, newQuat, 30);
        }

        
    }

    public void TakeDamage(float amount)
    {
        animator.SetTrigger("DamageTaken");
        health -= amount;
        UIManager.Instance.HealthBar.SetHealth(health);
        playerDamageEvent(this);
    }
}