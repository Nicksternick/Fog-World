using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellInventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        SpellMenuItem item = dropped.GetComponent<SpellMenuItem>();

        item.parentAfterDrag = transform;
    }
}
