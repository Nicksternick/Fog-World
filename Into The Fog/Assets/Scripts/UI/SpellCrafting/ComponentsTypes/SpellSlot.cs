using UnityEngine;
using UnityEngine.EventSystems;

public enum SpellComponentType
{
    None = -1,
    Element,
    Shape
}

public class SpellSlot : MonoBehaviour, IDropHandler
{
    // ===== | Variables | =====
    [SerializeField] protected SpellComponentType componentType;
    [SerializeField] protected SpellMenuItem item;

    // ===== | Properties | =====
    public bool HasChild { get { return transform.childCount != 0; } }

    public SpellMenuItem Item { get { return item; } }

    // ===== | Methods | =====
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        SpellMenuItem item = dropped.GetComponent<SpellMenuItem>();

        //Debug.Log("Drop");

        if (item.ComponentType == componentType)
        {
            if (0 == transform.childCount)
            {
                item.parentAfterDrag = transform;
            }
            else
            {
                SpellForum.Instance.SendComponentToInventory(this.item);
                ClearSlot();

                item.parentAfterDrag = transform;
            }
        }
    }

    public bool IsChild()
    {
        return HasChild;
    }

    public void SetSlot(SpellMenuItem item)
    {
        this.item = item;
    }

    public void ClearSlot()
    {
        item = null;
    }
}
