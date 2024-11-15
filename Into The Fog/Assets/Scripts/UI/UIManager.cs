using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static UIManager Instance;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private HealthBar staminaBar;
    [SerializeField] private FogIconSpell spell1;
    [SerializeField] private FogIconSpell spell2;
    [SerializeField] private FogIconSpell spell3;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hiveCount;

    // ===== | Properties | =====
    public HealthBar HealthBar { get { return healthBar; } }
    public HealthBar StaminaBar { get { return staminaBar; } }
    public TextMeshProUGUI HiveCount { get { return hiveCount; } }
    public FogIconSpell Spell1 { get { return spell1; } }
    public FogIconSpell Spell2 { get { return spell2; } }
    public FogIconSpell Spell3 { get { return spell3; } }

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

    private void Start()
    {
        levelText.text = $"Level: {GameManager.Instance.CurrentLevel}";
    }
}
