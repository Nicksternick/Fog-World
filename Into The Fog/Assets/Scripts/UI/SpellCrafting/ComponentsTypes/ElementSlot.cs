using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSlot : SpellSlot
{
    // ===== | Variables | =====
    private ElementItem elementItem;

    // ===== | Properties| =====
    public Elements Element
    {
        get
        {
            if (elementItem != null)
            {
                return elementItem.Element;
            }

            return Elements.None;
        }
    }

    public ElementItem Item { get { return elementItem; } }

    // ===== | Methods | =====
    private void Start()
    {
        componentType = SpellComponentType.Element;
    }

    private void Update()
    {
        if (transform.childCount == 0 && item != null)
            item = null;

        if (item != null && elementItem == null)
        {
            elementItem = item as ElementItem;
            
        }
        
        if (item == null && elementItem != null)
        {
            elementItem = null;
        }
    }
}