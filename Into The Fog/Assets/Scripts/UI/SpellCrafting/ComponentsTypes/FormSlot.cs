using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormSlot : SpellSlot
{
    // ===== | Variables | =====
    private FormItem formItem;

    // ===== | Properties| =====
    public Forms Form
    {
        get
        {
            if (formItem != null)
            {
                return formItem.Form;
            }

            return Forms.None;
        }
    }

    public FormItem Item { get { return formItem; } }

    // ===== | Methods | =====
    private void Start()
    {
        componentType = SpellComponentType.Shape;
    }

    private void Update()
    {
        if (transform.childCount == 0 && item != null)
            item = null;

        if (item != null && formItem == null)
        {
            formItem = item as FormItem;

        }
        else if (item == null && formItem != null)
        {
            formItem = null;
        }
    }
}
