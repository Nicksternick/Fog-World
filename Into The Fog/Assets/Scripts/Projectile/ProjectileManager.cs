using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;

    public static ObjectPool<Ball> BallPool; 

    // Start is called before the first frame update
    void Start()
    {
        BallPool = new ObjectPool<Ball>(CreateBall, OnTakeBallFromPool, OnReturnBallToPool, OnDestroyBall, true, 500, 1000);
    }

    private Ball CreateBall()
    {
        ProjectileData ballData = new ProjectileData(1, 1, 3, false);
        Ball ball = new Ball(this.gameObject, 5, ballData);

        // Spawn Ball instince
        ball = Instantiate(ball, this.gameObject.transform.position, this.gameObject.transform.rotation);

        // Set up object pool
        ball.SetPool(BallPool);

        return ball;
    }

    private void OnTakeBallFromPool(Ball ball)
    {
        // Set transform and rotation
        ball.transform.position = this.gameObject.transform.position;
        ball.transform.rotation = this.gameObject.transform.rotation;

        ball.gameObject.SetActive(true);
    }

    private void OnReturnBallToPool(Ball Ball)
    {
        Ball.gameObject.SetActive(false);
    }

    private void OnDestroyBall(Ball ball)
    {
        Destroy(ball.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
