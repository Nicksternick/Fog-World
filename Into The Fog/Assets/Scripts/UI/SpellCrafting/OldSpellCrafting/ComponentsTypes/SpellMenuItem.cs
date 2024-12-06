using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellMenuItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;                 // The image from the item PREFAB
    public Transform parentAfterDrag;

    [SerializeField] protected SpellComponentType componentType;

    public SpellComponentType ComponentType { get { return componentType; } }

    // ===== | Methods | =====

    // Switches parent and hides image so it can snap to a new slot
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        AudioManager.Instance.PlaySound("On Component Click", true, 2.0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    // Assigns new parent and reenables the item's ability to be moved
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag End");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        SpellForum.Instance.ComponentDropped(this);
    }
}
