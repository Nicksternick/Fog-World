using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static UIManager Instance;

    private const int KeyboardGlyphStart = 4;
    private const int XboxGlyphStart = 13;
    private const int Ps4GlyphStart = 21;

    [SerializeField] private UIState state;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private HealthBar staminaBar;
    [SerializeField] private FogIconSpell spell1;
    [SerializeField] private FogIconSpell spell2;
    [SerializeField] private FogIconSpell spell3;
    [SerializeField] private FogIconSpell spell4;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hiveCount;

    // ===== | Properties | =====
    public HealthBar HealthBar { get { return healthBar; } }
    public HealthBar StaminaBar { get { return staminaBar; } }
    public TextMeshProUGUI HiveCount { get { return hiveCount; } }
    public FogIconSpell Spell1 { get { return spell1; } }
    public FogIconSpell Spell2 { get { return spell2; } }
    public FogIconSpell Spell3 { get { return spell3; } }
    public FogIconSpell Spell4 { get { return spell4; } }

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
        levelText.text = $"Level: {GameManager.Instance.CurrentLevel + 1}";
    }

    public void DeviceChange(PlayerInput input)
    {
        Cursor.visible = input.currentControlScheme != "Controller";

        if (input.currentControlScheme == "Controller")
        {
            state = UIState.Controller;
            UpdateSpellUI();
        }
        else
        {
            state = UIState.KeyboardMouse;
            UpdateSpellUI();
        }
    }

    private void UpdateSpellUI()
    {
        int index = state == UIState.Controller ? XboxGlyphStart : KeyboardGlyphStart;

        if (spell1.gameObject.activeSelf)
        {
            spell1.SetControlGlyph(index);
        }

        if (spell2.gameObject.activeSelf)
        {
            spell2.SetControlGlyph(index + 1);
        }

        if (spell3.gameObject.activeSelf)
        {
            spell3.SetControlGlyph(index + 2);
        }

        if (spell4.gameObject.activeSelf)
        {
            spell4.SetControlGlyph(index + 3);
        }
    }

    public void SetupUIInputEvents(PlayerInput input)
    {
        Debug.Log(input);
        input.onControlsChanged += DeviceChange;
        DeviceChange(input);
    }
}
