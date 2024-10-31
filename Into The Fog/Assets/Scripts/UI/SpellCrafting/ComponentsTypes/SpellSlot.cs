using UnityEngine;
using UnityEngine.EventSystems;

public enum SpellComponentType
{
    Element,
    Shape
}

public class SpellSlot : MonoBehaviour, IDropHandler
{
    // ===== | Variables | =====
    [SerializeField] protected SpellComponentType componentType;
    [SerializeField] protected SpellMenuItem item;

    // ===== | Methods | =====
    public void OnDrop(PointerEventData eventData)
    {
        if (0 == transform.childCount)
        {
            GameObject dropped = eventData.pointerDrag;
            SpellMenuItem item = dropped.GetComponent<SpellMenuItem>();

            if (item.ComponentType == componentType)
            {
                this.item = item;
                item.parentAfterDrag = transform;
            }
        }
    }

}
