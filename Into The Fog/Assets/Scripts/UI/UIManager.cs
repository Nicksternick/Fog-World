using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static UIManager Instance;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private HealthBar staminaBar;
    [SerializeField] private FogIconSpell spell1;
    [SerializeField] private FogIconSpell spell2;

    // ===== | Properties | =====
    public HealthBar HealthBar { get { return healthBar; } }
    public HealthBar StaminaBar { get { return staminaBar; } }
    public FogIconSpell Spell1 { get { return spell1; } }
    public FogIconSpell Spell2 { get { return spell2; } }

    // ===== | Methods | =====
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
