using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AJ Wagner - 10/2/2024
/// Created the basics for the health bar and a
/// couple of base helper methods.
/// 
/// Make sure to call the TakeDamage method whenever
/// something takes damage so the screen can update
/// </summary>
public class HealthBar : MonoBehaviour
{
    // The slider beneath the sprite that's in charge
    // of the health's representation
    [SerializeField] public Slider health;

    // Sets the max health for the bar
    public void SetMaxHealth(float unitHealth)
    {
        health.maxValue = unitHealth;
        health.value = unitHealth;
    }

    // Takes health value and sets the bar appropriately
    public void SetHealth(float currentHealth)
    {
        health.value = currentHealth;
    }

    // This method is redundant, commenting it out for now

    // Decrements the bar in response to damage
    //public void TakeDamage(float damageValue)
    //{
    //    // Prevent negative values for the bar, just in case
    //    if (damageValue < health.value)
    //    {
    //        health.value -= damageValue;
    //    }
    //    else
    //    {
    //        health.value = 0;
    //    }
    //}
}
