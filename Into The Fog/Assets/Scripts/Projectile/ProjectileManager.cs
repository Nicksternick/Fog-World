using System.Collections.Generic;
using UnityEngine;

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

[System.Serializable]
public class FormPrefabs<T>
{
    public List<T> elementPrefabs;  // Prefabs for each element (Fire, Ice, etc.) for a specific form
}

[System.Serializable]
public class ProjectilePrefabs
{
    public FormPrefabs<Ball> ballPrefabs;     // Ball form, with prefabs for each element
    public FormPrefabs<Laser> laserPrefabs;   // Laser form, with prefabs for each element
    public FormPrefabs<AoE> aoePrefabs;       // AoE form, with prefabs for each element
}

public class ProjectileManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static ProjectileManager Instance;

    private const int elementCount = 1;

    [SerializeField]
    private ProjectilePrefabs projectilePrefabs = new ProjectilePrefabs();

    private List<GenericPool<Ball>> ballPools;
    private List<GenericPool<Laser>> laserPools;
    private List<GenericPool<AoE>> aoePools;

    //[SerializeField]
    //private Ball ballPrefab;                                    // Prefab for the ball
    //[SerializeField]
    //private List<Shader> ballFxs = new List<Shader>();          // List for ball spell effects

    //[SerializeField]
    //private Laser laserPrefab;                                  // Prefab for the laser
    //[SerializeField]
    //private List<Shader> laserFxs = new List<Shader>();         // List for Laser spell effects
    //
    //[SerializeField]
    //private AoE aoePrefab;                                      // Prefab for the AoE
    //[SerializeField]
    //private List<Shader> aoeFxs = new List<Shader>();           // List for Aoe spell effects


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
    //private GenericPool<Ball> ballPool; 
    //private GenericPool<Laser> laserPool;
    //private GenericPool<AoE> aoePool;

    void Awake()
    {
        InitializePrefabList(ref projectilePrefabs.ballPrefabs);
        InitializePrefabList(ref projectilePrefabs.laserPrefabs);
        InitializePrefabList(ref projectilePrefabs.aoePrefabs);
    }

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
        InitializePools();
    }

    /// <summary>
    /// Jay 11/10/24
    /// Helper method to make sure that the pool list does not cause out of range exceptions 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="formPrefabs"></param>
    private void InitializePrefabList<T>(ref FormPrefabs<T> formPrefabs) where T : MonoBehaviour
    {
        int elementCount = System.Enum.GetValues(typeof(Elements)).Length;

        // Initialize the list if it hasn't been already
        if (formPrefabs.elementPrefabs == null)
        {
            formPrefabs.elementPrefabs = new List<T>(new T[elementCount]);
        }
        else
        {
            // Ensure the list has the correct number of elements
            for (int i = formPrefabs.elementPrefabs.Count+1; i < elementCount; i++)
            {
                formPrefabs.elementPrefabs.Add(null); // Add nulls to fill any missing slots
            }
        }
    }

    /// <summary>
    /// Jay 11/10/24
    /// Initializes the object pools and pool lists 
    /// </summary>
    private void InitializePools()
    {

        ballPools = new List<GenericPool<Ball>>(elementCount);
        laserPools = new List<GenericPool<Laser>>(elementCount);
        aoePools = new List<GenericPool<AoE>>(elementCount);

        for (int i = 0; i <= elementCount; i++)
        {
            // Initialize each pool only if the prefab exists
            ballPools.Add(projectilePrefabs.ballPrefabs.elementPrefabs[i] != null
                ? new GenericPool<Ball>(projectilePrefabs.ballPrefabs.elementPrefabs[i], 50, 200) : null);

            laserPools.Add(projectilePrefabs.laserPrefabs.elementPrefabs[i] != null
                ? new GenericPool<Laser>(projectilePrefabs.laserPrefabs.elementPrefabs[i], 50, 200) : null);

            aoePools.Add(projectilePrefabs.aoePrefabs.elementPrefabs[i] != null
                ? new GenericPool<AoE>(projectilePrefabs.aoePrefabs.elementPrefabs[i], 50, 200) : null);
        }
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
    
    ///// <summary>
    ///// Helper method to easily apply Spell effects
    ///// </summary>
    ///// <param name="projectile"> What the effect will be added to </param>
    ///// <param name="element"> The spell's element </param>
    //private void ApplyFX(Projectile projectile, Elements element)
    //{
    //    Shader shader = null;
    //    
    //    // Retrieve the corresponding shader from the correct list
    //    switch (projectile)
    //    {
    //        case Ball _ when ballFxs.Count > (int)element:
    //            shader = ballFxs[(int)element];
    //            break;
    //
    //        case Laser _ when laserFxs.Count > (int)element:
    //            shader = laserFxs[(int)element];
    //            break;
    //
    //        case AoE _ when aoeFxs.Count > (int)element:
    //            shader = aoeFxs[(int)element];
    //            break;
    //
    //        default:
    //            break;
    //    }
    //
    //    if (shader != null)
    //    {
    //        Renderer renderer = projectile.GetComponent<Renderer>();
    //        if (renderer != null)
    //        {
    //            renderer.material.shader = shader;
    //        }
    //    }
    //}

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
        var pool = ballPools[(int)element];
        Ball ball = pool.GetFromPool();
        ball.SetPool(pool);
        
        // Position the spell in front of the player
        ball.transform.position = caller.transform.position + caller.transform.forward * 2;
        ball.transform.rotation = caller.transform.rotation;
        ball.Caller = caller;


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
        var pool = laserPools[(int)element];
        Laser laser = pool.GetFromPool();
        laser.SetPool(pool);
        // Position the spell in front of the player
        laser.transform.position = caller.transform.position + caller.transform.forward * 6;
        laser.transform.position += new Vector3(0,.25f,0);
        laser.transform.rotation = caller.transform.rotation;
        laser.Caller = caller;

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

        var pool = aoePools[(int)element];
        AoE aoe = pool.GetFromPool();
        aoe.SetPool(pool);

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

        // Initialize with the data
        aoe.Initialize(AoeDuration, new ProjectileData(AoeDamage, 1.0f, 0.0f, false, element));

        return aoe;
    }
    #endregion
}
