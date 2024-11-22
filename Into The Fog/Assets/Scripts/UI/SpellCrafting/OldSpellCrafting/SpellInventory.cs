using UnityEngine;
using UnityEngine.EventSystems;

public class SpellInventory : MonoBehaviour, IDropHandler
{
    // ===== | Variables | =====
    [SerializeField] SpellComponentType componentType;

    // ===== | Methods | =====
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        SpellMenuItem item = dropped.GetComponent<SpellMenuItem>();
        
        if (item.ComponentType == componentType )
            item.parentAfterDrag = transform;
    }
}
