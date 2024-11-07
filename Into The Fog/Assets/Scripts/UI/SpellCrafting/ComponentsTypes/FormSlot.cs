using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormSlot : SpellSlot
{
    // ===== | Variables | =====

    // ===== | Properties| =====
    public Forms Form
    {
        get
        {
            if (item != null)
            {
                return (item as FormItem).Form;
            }

            return Forms.None;
        }
    }

    // ===== | Methods | =====
    private void Start()
    {
        componentType = SpellComponentType.Shape;
    }

    public override string ToString()
    {
        return Form.ToString();
    }
}
