using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSlot : SpellSlot
{
    // ===== | Variables | =====

    // ===== | Properties| =====
    public Elements Element
    {
        get
        {
            if (item != null)
            {
                return (item as ElementItem).Element;
            }

            return Elements.None;
        }
    }

    // ===== | Methods | =====
    private void Start()
    {
        componentType = SpellComponentType.Element;
    }

    public override string ToString()
    {
        return Element.ToString();
    }
}