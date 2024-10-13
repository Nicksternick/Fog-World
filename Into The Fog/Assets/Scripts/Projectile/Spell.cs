using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    private Elements element;
    private Forms form;
    private float cooldown;

    public float Cooldown {  get { return cooldown; } set {  cooldown = value; } }

    public void Initialize(Elements element, Forms form, float cooldown) 
    {
        this.element = element;
        this.form = form;
        this.cooldown = cooldown;
    }

    public void CastSpell(GameObject caller)
    {
        ProjectileManager.Instance.CreateProjectile(caller, element, form);
    }

}
