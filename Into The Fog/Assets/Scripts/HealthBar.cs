using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// ADD REFERENCE TO PLAYER AND ENEMY WHEN DAMAGE IS IMPLEMENTED
// TIE INTO DAMAGE SCRIPT AS WELL

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Slider health;      // The displayed health

    // Sets the max health for the bar
    public void SetMaxHealth(int unitHealth)
    {
        health.maxValue = unitHealth;
        health.value = unitHealth;
    }

    // Takes health value and sets the bar appropriately
    public void SetHealth(int currentHealth)
    {
        health.value = currentHealth;
    }

    // Decrements the bar in response to damage
    public void TakeDamage(int damageValue)
    {
        // Prevent negative values for the bar, just in case
        if (damageValue < health.value)
        {
            health.value -= damageValue;
        }
        else
        {
            health.value = 0;
        }
    }
}
