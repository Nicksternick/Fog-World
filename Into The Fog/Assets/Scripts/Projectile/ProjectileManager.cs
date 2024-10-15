using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using static Projectile;
using static UnityEditor.Rendering.FilterWindow;

/// <summary>
/// Enum to represent various spell forms.
/// </summary>
public enum Forms
{
    Ball,
}

/// <summary>
/// Enum to represent various elemental types.
/// </summary>
public enum Elements
{
    Fire,
    Ice,
}

public class ProjectileManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static ProjectileManager Instance;

    [SerializeField]
    private Ball ballPrefab;  // Prefab for the ball
    private GenericPool<Ball> ballPool; // Pool for Ball objects

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
            defaultCapacity: 200,
            maxSize: 1000
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
        }
    }

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
        ball.Initialize(5.0f, new ProjectileData(1.0f, 1.0f, 5.0f, false));

        return ball;
    }

    // Public method to return the Ball to the pool
    public void ReturnBallToPool(Ball ball)
    {
        ballPool.ReturnToPool(ball);
    }
}
