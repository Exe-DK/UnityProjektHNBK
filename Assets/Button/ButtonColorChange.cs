using UnityEngine;
using UnityEngine.UI; // **Für UI-Image-Komponenten**

public class ButtonColorChange : MonoBehaviour
{
    [Header("Farbeinstellungen")]
    public Color normalColor = new Color(0.5f, 0.5f, 0.5f); // Grau
    public Color hoverColor = Color.yellow; // Gelb beim Hovern
    [Range(0.1f, 20f)] public float fadeSpeed = 5f; // Geschwindigkeit des Farbwechsels

    [Header("Bild-Referenz (optional)")]
    [Tooltip("Ziehe hier das Bild-Objekt (SpriteRenderer) rein, falls vorhanden.")]
    public SpriteRenderer buttonImage; // **Für 2D-Bilder auf 3D-Buttons**

    [Tooltip("Ziehe hier das UI-Image (Canvas) rein, falls vorhanden.")]
    public Image uiImage; // **Für UI-Buttons (Canvas)**

    private Material material;
    private bool isHovering = false;
    private Color currentColor;

    void Start()
    {
        // Material kopieren (wichtig, um das Original nicht zu ändern)
        material = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = material;
        currentColor = normalColor;
        material.color = currentColor;

        // **Falls kein Bild zugewiesen ist, suche automatisch nach einem SpriteRenderer**
        if (buttonImage == null)
        {
            buttonImage = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Update()
    {
        // Sanfter Farbwechsel
        if (isHovering && currentColor != hoverColor)
        {
            currentColor = Color.Lerp(currentColor, hoverColor, fadeSpeed * Time.deltaTime);
        }
        else if (!isHovering && currentColor != normalColor)
        {
            currentColor = Color.Lerp(currentColor, normalColor, fadeSpeed * Time.deltaTime);
        }

        // **Farbe des 3D-Buttons ändern**
        material.color = currentColor;

        // **Farbe des Bildes ändern (falls vorhanden)**
        if (buttonImage != null)
        {
            buttonImage.color = currentColor;
        }
        if (uiImage != null)
        {
            uiImage.color = currentColor;
        }
    }

    // Diese Methoden werden vom Fadenkreuz-Script aufgerufen
    public void OnHoverStart() => isHovering = true;
    public void OnHoverEnd() => isHovering = false;
}
