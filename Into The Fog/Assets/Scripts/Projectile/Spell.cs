using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] private Elements element;
    [SerializeField] private Forms form;
    [SerializeField] private float cooldown;

    /// <summary>
    /// Jay 10/13/2024
    /// Spell's Cooldown
    /// </summary>
    public float Cooldown {  get { return cooldown; } set {  cooldown = value; } }

    public Elements Element { get { return element; } }
    public Forms Form { get { return form; } }

    /// <summary>
    /// Jay 10/1/2024
    /// </summary>
    /// <param name="element"> Spell element </param>
    /// <param name="form"> Spell form </param>
    /// <param name="cooldown"> Spell cooldown </param>
    public void Initialize(Elements element, Forms form, float cooldown) 
    {
        this.element = element;
        this.form = form;
        this.cooldown = cooldown;
    }

    /// <summary>
    /// Sets off projectile creation process
    /// </summary>
    /// <param name="caller"> The gameobject that invoked this method </param>
    public void CastSpell(GameObject caller)
    {
        ProjectileManager.Instance.CreateProjectile(caller, element, form);
    }

}
