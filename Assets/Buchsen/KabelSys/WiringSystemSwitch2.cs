using UnityEngine;

public enum WiringMode
{
    TokenSystem,
    CableSystem
}

public class WiringSystemSwitch2 : MonoBehaviour
{
    [Header("Modus-Auswahl")]
    public WiringMode activeMode = WiringMode.TokenSystem;

    [Header("System-Referenzen")]
    [SerializeField] private MonoBehaviour tokenManager;
    [SerializeField] private CableManager2 cableManager;

    private void Start()
    {
        ApplyMode();
    }

    public void SetMode(WiringMode mode)
    {
        activeMode = mode;
        ApplyMode();
    }

    public void ToggleMode()
    {
        activeMode = activeMode == WiringMode.TokenSystem
            ? WiringMode.CableSystem
            : WiringMode.TokenSystem;
        ApplyMode();
    }

    private void ApplyMode()
    {
        if (tokenManager != null)
            tokenManager.enabled = (activeMode == WiringMode.TokenSystem);

        if (cableManager != null)
            cableManager.enabled = (activeMode == WiringMode.CableSystem);

        Debug.Log($"Wiring-Modus: {activeMode}");
    }
}
