using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

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
    private GameObject ballPrefab;  // Prefab for the ball
    private ObjectPool<Ball> ballPool;  // Pool for Ball objects

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

        // Initialize the object pool
        ballPool = new ObjectPool<Ball>(
            CreateBall,
            OnTakeBallFromPool,
            OnReturnBallToPool,
            OnDestroyBall,
            collectionCheck: true,
            defaultCapacity: 200,
            maxSize: 1000
        );
    }

    // Create new Ball object from the prefab
    private Ball CreateBall()
    {
        // Instantiate the ball from a prefab
        GameObject ballObject = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        Ball ball = ballObject.GetComponent<Ball>();

        // Set up object pool reference in the ball
        ball.SetPool(ballPool);

        return ball;
    }

    private void OnTakeBallFromPool(Ball ball)
    {
        ball.gameObject.SetActive(true);
    }

    private void OnReturnBallToPool(Ball ball)
    {
        ball.gameObject.SetActive(false);
    }

    private void OnDestroyBall(Ball ball)
    {
        Destroy(ball.gameObject);
    }

    public Ball GetBallFromPool(Vector3 position, Quaternion rotation, GameObject caller, Elements element)
    {
        Ball ball = ballPool.Get();
        ball.transform.position = position;
        ball.transform.rotation = rotation;
        ball.Caller = caller;

        switch (element)
        {
            case Elements.Fire:
                ball.ChangeColor(Color.red);
                break;
            case Elements.Ice:
                ball.ChangeColor(Color.cyan);
                break;
        }

        ball.Initialize(5.0f, new ProjectileData(1.0f, 1.0f, 5.0f, 3.0f, false));

        return ball;
    }

    public void ReturnBallToPool(Ball ball)
    {
        ballPool.Release(ball);
    }
}
