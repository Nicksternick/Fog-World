using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class VirtualMouseController : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] private VirtualMouseInput mouseController;

    // Reference to the UI element representing the virtual mouse
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] private Vector2 baseMouseSize = new Vector2(50, 50);

    [SerializeField] private CanvasScaler canvasScaler;

    // ===== | Methods | =====
    private void Awake()
    {
        mouseController = GetComponent<VirtualMouseInput>();
    }

    private void LateUpdate()
    {
        Vector2 mousePosition = mouseController.virtualMouse.position.value;
        mousePosition.x = Mathf.Clamp(mousePosition.x, 0f, Screen.width);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0f, Screen.height);
        InputState.Change(mouseController.virtualMouse.position, mousePosition);

        // Update the virtual mouse position in the Input System
        InputState.Change(mouseController.virtualMouse.position, mousePosition);

        // Move the virtual mouse UI element to the correct position
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            mousePosition,
            null,
            out anchoredPosition);

        rectTransform.anchoredPosition = anchoredPosition;

        // Adjust the mouse size based on the Canvas's scale factor
        float scaleFactor = canvasScaler.scaleFactor;
        rectTransform.sizeDelta = baseMouseSize / scaleFactor;
    }
}
