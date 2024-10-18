using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FogIconSpell : MonoBehaviour
{
    // ===== | Variables | =====
    [SerializeField] private TextMeshProUGUI spellStatus;
    [SerializeField] private HealthBar statusBar;

    // ===== | Variables | =====
    public void SetMaxCooldown(float maxCooldown)
    {
        statusBar.SetMaxHealth(maxCooldown);
    }

    public void SetCooldown(float cooldown)
    {
        statusBar.SetHealth(cooldown);
        if (statusBar.Value == statusBar.MaxValue)
        {
            spellStatus.text = "Ready!";
        }
        else
        {
            spellStatus.text = "Charging...";
        }
    }
    
}
