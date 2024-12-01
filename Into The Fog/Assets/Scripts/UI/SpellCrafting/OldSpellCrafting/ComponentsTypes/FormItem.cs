using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormItem : SpellMenuItem
{
    [SerializeField] private Forms spellShape;

    public Forms Form
    {
        get { return spellShape; }
        set { spellShape = value; }
    }
}
