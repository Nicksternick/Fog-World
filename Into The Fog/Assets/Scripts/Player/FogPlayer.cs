using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Player Inputs")]
    // ----- | Inputs from PlayerInput | -----
    [SerializeField] private PlayerInput input;
    private InputAction move;

    [Header("Camera")]
    // ----- | Camera SubObjects | -----
    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private Camera playerCamera;

    [Header("Collider And Model")]
    // ----- | Collider/Model SubObject | -----
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private MeshRenderer playerModel;

    private Vector3 moveVector;

    // ===== | Methods | =====
    private void Awake()
    {
        move = input.actions.FindAction("Move");
    }

    private void Update()
    {
        LookAtMouse();
    }

    private void FixedUpdate()
    {
        if (move != null)
        {
            Vector2 readValue = move.ReadValue<Vector2>().normalized;

            // Get the right and forward vectors from the camera's orientation
            Vector3 cameraRight = playerCamera.transform.right;
            Vector3 cameraForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;

            // Combine the input with camera orientation to create movement direction
            moveVector = (cameraRight * readValue.x + cameraForward * readValue.y).normalized;
        }

        playerRigidBody.MovePosition(transform.position + (moveVector * moveSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Nicholas 10/8/24
    /// Does a raycast to determine where the mouse is,
    /// then makes the player face the position of the mouse
    /// </summary>
    private void LookAtMouse()
    {
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if(plane.Raycast(ray, out distance))
        {
            Vector3 mousePosition = ray.GetPoint(distance);
            playerModel.transform.LookAt(mousePosition, Vector3.up);
            Quaternion rotation = playerModel.transform.rotation;
            rotation.x = 0;
            rotation.z = 0;
            playerModel.transform.rotation = rotation;
        } 
    }
}
