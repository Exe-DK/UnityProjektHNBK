using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    [Range(0, 5)] public float glowIntensity = 1.5f;
    [Range(0.1f, 20f)] public float fadeSpeed = 5f;

    private Material material;
    private bool isGlowing = false;
    private float currentIntensity = 0f;

    void Start()
    {
        // Material kopieren (wichtig für Unity 6.3!)
        material = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = material;

        // Emission manuell aktivieren (ersetzt DynamicGI)
        material.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Sanftes Ein-/Ausblenden
        if (isGlowing && currentIntensity < glowIntensity)
        {
            currentIntensity = Mathf.Lerp(currentIntensity, glowIntensity, fadeSpeed * Time.deltaTime);
        }
        else if (!isGlowing && currentIntensity > 0)
        {
            currentIntensity = Mathf.Lerp(currentIntensity, 0, fadeSpeed * Time.deltaTime);
        }

        // Emission aktualisieren
        material.SetColor("_EmissionColor", glowColor * currentIntensity);

        // Wichtig für Unity 6.3: Material-Update erzwingen
        GetComponent<Renderer>().UpdateGIMaterials();
    }

    public void EnableGlow() => isGlowing = true;
    public void DisableGlow() => isGlowing = false;
}
