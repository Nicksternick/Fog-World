using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (0 == transform.childCount)
        {
            GameObject dropped = eventData.pointerDrag;
            SpellMenuItem item = dropped.GetComponent<SpellMenuItem>();
            item.parentAfterDrag = transform;
        }
    }

}
