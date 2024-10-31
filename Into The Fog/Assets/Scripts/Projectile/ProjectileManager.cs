using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;

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
    private Ball ballPrefab;  // Prefab for the ball

    [SerializeField]
    private Laser laserPrefab;  // Prefab for the laser

    [SerializeField]
    private AoE aoePrefab;  // Prefab for the AoE

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

    #region Ball Spawning
    // Public method to get a Ball from the pool
    public Ball GetBallFromPool(GameObject caller, Elements element)
    {
        Ball ball = ballPool.GetFromPool();
        ball.SetPool(ballPool);
        ball.transform.position = caller.transform.position + caller.transform.forward * 2;
        ball.transform.rotation = caller.transform.rotation;
        ball.Caller = caller;

        // Customize the ball based on its element
        switch (element)
        {
            case Elements.Fire:
                ball.ChangeColor(Color.red);
                break;
            case Elements.Ice:
                ball.ChangeColor(Color.cyan);
                break;
        }

        // Initialize with the data
        ball.Initialize(5.0f, new ProjectileData(1.0f, 1.0f, 5.0f, false, element));

        return ball;
    }

    #endregion

    #region Laser Spawning
    // Public method to get a Laser from the pool
    public Laser GetLaserFromPool(GameObject caller, Elements element)
    {
        Laser laser = laserPool.GetFromPool();
        laser.SetPool(laserPool);
        laser.transform.position = caller.transform.position + caller.transform.forward * 6;
        laser.transform.position += new Vector3(0,.25f,0);
        laser.transform.rotation = caller.transform.rotation;
        laser.Caller = caller;

        // Customize the ball based on its element
        switch (element)
        {
            case Elements.Fire:
                laser.ChangeColor(Color.red);
                break;
            case Elements.Ice:
                laser.ChangeColor(Color.cyan);
                break;
        }

        // Initialize with the data
        laser.Initialize(3.0f, new ProjectileData(1.0f, 1.0f, 0.0f, false, element));

        return laser;
    }
    #endregion

    #region AoE Spawning
    // Public method to get a Laser from the pool
    public AoE GetAoEFromPool(GameObject caller, Elements element)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        AoE aoe = aoePool.GetFromPool();
        aoe.SetPool(aoePool);

        // Ternary statement that spawns AoE under a enemy or the player if they are in the way of the raycast
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

        aoe.transform.position = spawnPosition;
        aoe.transform.rotation = caller.transform.rotation;
        aoe.Caller = caller;


        // Customize the ball based on its element
        switch (element)
        {
            case Elements.Fire:
                aoe.ChangeColor(Color.red);
                break;
            case Elements.Ice:
                aoe.ChangeColor(Color.cyan);
                break;
        }

        // Initialize with the data
        aoe.Initialize(3.0f, new ProjectileData(1.0f, 1.0f, 0.0f, false, element));

        return aoe;
    }
    #endregion
}
