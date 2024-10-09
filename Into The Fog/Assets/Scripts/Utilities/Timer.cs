using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Nicholas 10/9/2024
/// A simple timer class that counts down from a 
/// desired float, then triggers an event when completed
/// </summary>
public class Timer : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] private float maxTime;
    [SerializeField] private UnityEvent onCountDownEnd;
    private float currentTime;
    private bool isCountingDown;

    // ===== | Properties | =====
    public bool IsCounting { get { return isCountingDown; } }
    public UnityEvent OnCountDownEnd { get { return onCountDownEnd; } }
    public float MaxTime { get { return maxTime; } }

    // ===== | Methods | =====
    private void Awake()
    {
        onCountDownEnd = new UnityEvent();
    }

    /// <summary>
    /// Nicholas 10/9/2024
    /// Starts the timer, only starts 
    /// if the timer isn't already counting
    /// </summary>
    public void StartTimer() 
    {
        // Only start the timer if it's not already counting
        if (!isCountingDown)
        {
            currentTime = maxTime;
            isCountingDown = true;
        }
        else
        {
            Debug.LogAssertion("Timers is already running");
        }
    }

    /// <summary>
    /// Nicholas 10/9/2024
    /// Stops the timer if the timer is currently counting
    /// </summary>
    public void CancelTimer() { isCountingDown = false; }

    /// <summary>
    /// Nicholas 10/9/2024
    /// Sets the max time variable, only sets it 
    /// if the timer is currently not counting
    /// </summary>
    /// <param name="newTime"></param>
    public void SetMaxTime(float newTime)
    {
        if (!isCountingDown)
        {
            maxTime = newTime;
        }
        else
        {
            Debug.LogAssertion("Timers max time was " +
                "attempted to be changed while it was running");
        }
    }

    private void Update()
    {
        // Decrement the time and see if it's below zero
        currentTime -= Time.deltaTime;
        if (currentTime <= 0 && isCountingDown)
        {
            Debug.Log("Timer Exit");
            // Stop the timer and trigger the unity event
            isCountingDown = false;
            onCountDownEnd.Invoke();
        }
    }
}
