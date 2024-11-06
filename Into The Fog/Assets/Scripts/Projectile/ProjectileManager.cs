using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Rendering.FilterWindow;

/// <summary>
/// Enum to represent various elemental types.
/// </summary>
public enum Elements
{
    None = -1,
    Fire,
    Ice,
}

/// <summary>
/// Enum to represent various spell forms.
/// </summary>
public enum Forms
{
    None = -1,
    Ball,
    Laser,
    AoE
}

public class ProjectileManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static ProjectileManager Instance;

    [SerializeField]
    private Ball ballPrefab;                                    // Prefab for the ball
    [SerializeField]
    private List<Shader> ballFxs = new List<Shader>();          // List for ball spell effects

    [SerializeField]
    private Laser laserPrefab;                                  // Prefab for the laser
    [SerializeField]
    private List<Shader> laserFxs = new List<Shader>();         // List for Laser spell effects

    [SerializeField]
    private AoE aoePrefab;                                      // Prefab for the AoE
    [SerializeField]
    private List<Shader> aoeFxs = new List<Shader>();           // List for Aoe spell effects


    #region Projectile Stats
    // Ball
    [SerializeField]
    private float ballDamage = 25.0f;
    [SerializeField]
    private float ballDuration = 5.0f; 

    // Laser
    [SerializeField]
    private float laserDamage = 25.0f;
    [SerializeField]
    private float laserDuration = 0.4f;

    // Aoe
    [SerializeField]
    private float AoeDamage = 25.0f;
    [SerializeField]
    private float AoeDuration = 3.0f;
    #endregion

    // Object Pools
    private GenericPool<Ball> ballPool; 
    private GenericPool<Laser> laserPool;
    private GenericPool<AoE> aoePool;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        // Initialize the generic object pool for Balls
        ballPool = new GenericPool<Ball>(
            ballPrefab,   // Pass the prefab directly
            defaultCapacity: 100,
            maxSize: 500
       );

        // Initialize the generic object pool for Lasers
        laserPool = new GenericPool<Laser>(
            laserPrefab,   // Pass the prefab directly
            defaultCapacity: 100,
            maxSize: 500
       );

        // Initialize the generic object pool for AoEs
        aoePool = new GenericPool<AoE>(
            aoePrefab,   // Pass the prefab directly
            defaultCapacity: 100,
            maxSize: 500
       );
    }

    /// <summary>
    /// Create the respective projectile for spells
    /// </summary>
    /// <param name="caller"> Gameobject Caller </param>
    /// <param name="element"> Spell element </param>
    /// <param name="form"> Spell Form </param>
    public void CreateProjectile(GameObject caller, Elements element, Forms form)
    {
        switch (form) 
        {
            case Forms.Ball:
                GetBallFromPool(caller, element);
                break;
            case Forms.Laser:
                GetLaserFromPool(caller, element);
                break;
            case Forms.AoE:
                GetAoEFromPool(caller, element);
                break;
        }
    }

    /// <summary>
    /// Helper method to easily apply Spell effects
    /// </summary>
    /// <param name="projectile"> What the effect will be added to </param>
    /// <param name="element"> The spell's element </param>
    private void ApplyFX(Projectile projectile, Elements element)
    {
        Shader shader = null;
        
        // Retrieve the corresponding shader from the correct list
        switch (projectile)
        {
            case Ball _ when ballFxs.Count > (int)element:
                shader = ballFxs[(int)element];
                break;

            case Laser _ when laserFxs.Count > (int)element:
                shader = laserFxs[(int)element];
                break;

            case AoE _ when aoeFxs.Count > (int)element:
                shader = aoeFxs[(int)element];
                break;

            default:
                break;
        }

        if (shader != null)
        {
            Renderer renderer = projectile.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.shader = shader;
            }
        }
    }

    #region Ball Spawning

    /// <summary>
    /// Jay 11/5/24
    /// Public method to get a Ball from the pool
    /// </summary>
    /// <param name="caller"> The game object spawning the spell </param>
    /// <param name="element"> The spell's element </param>
    /// <returns> A ball spell </returns>
    public Ball GetBallFromPool(GameObject caller, Elements element)
    {
        Ball ball = ballPool.GetFromPool();
        ball.SetPool(ballPool);
        
        // Position the spell in front of the player
        ball.transform.position = caller.transform.position + caller.transform.forward * 2;
        ball.transform.rotation = caller.transform.rotation;
        ball.Caller = caller;

        ApplyFX(ball, element);

        // Initialize with the data
        ball.Initialize(ballDuration, new ProjectileData(ballDamage, 1.0f, 5.0f, false, element));

        return ball;
    }

    #endregion

    #region Laser Spawning
    /// <summary>
    /// Jay 11/5/24
    /// Public method to get a laser from the pool
    /// </summary>
    /// <param name="caller"> The game object spawning the spell </param>
    /// <param name="element"> The spell's element </param>
    /// <returns> A laser spell </returns>
    public Laser GetLaserFromPool(GameObject caller, Elements element)
    {
        Laser laser = laserPool.GetFromPool();
        laser.SetPool(laserPool);
        // Position the spell in front of the player
        laser.transform.position = caller.transform.position + caller.transform.forward * 6;
        laser.transform.position += new Vector3(0,.25f,0);
        laser.transform.rotation = caller.transform.rotation;
        laser.Caller = caller;

        ApplyFX(laser, element);

        // Initialize with the data
        laser.Initialize(laserDuration, new ProjectileData(laserDamage, 1.0f, 0.0f, false, element));

        return laser;
    }
    #endregion

    #region AoE Spawning
    /// <summary>
    /// Jay 11/5/24
    /// Public method to get a aoe spell from the pool
    /// </summary>
    /// <param name="caller"> The game object spawning the spell </param>
    /// <param name="element"> The spell's element </param>
    /// <returns> A aoe spell </returns>
    public AoE GetAoEFromPool(GameObject caller, Elements element)
    {
        // Check the to see if there is a point that the mouse hit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        AoE aoe = aoePool.GetFromPool();
        aoe.SetPool(aoePool);

        // ===== | Nicholas: Update Mouse Code To Make Placement More Consistent | =====

        // Setup the plane and distance for later in the method
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Vector3 mousePosition = Vector3.zero;

        if (plane.Raycast(ray, out distance))
        {
            // Get that point
            mousePosition = ray.GetPoint(distance);
        }

        Vector3 spawnPosition = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check to see what the player clicked on
            if (hit.collider.gameObject.layer == 6)
            {
                return null;
            }
            else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy"))
            {
                spawnPosition = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
            }
            else { spawnPosition = hit.point; }
        }

        spawnPosition.y = 0.0f;

        aoe.transform.position = mousePosition;
        aoe.transform.rotation = caller.transform.rotation;
        aoe.Caller = caller;

        ApplyFX(aoe, element);

        // Initialize with the data
        aoe.Initialize(AoeDuration, new ProjectileData(AoeDamage, 1.0f, 0.0f, false, element));

        return aoe;
    }
    #endregion
}
