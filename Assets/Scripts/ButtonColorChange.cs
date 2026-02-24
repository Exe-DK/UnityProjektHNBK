using UnityEngine;

public class ButtonColorChange : MonoBehaviour
{
    [Header("Farbeinstellungen")]
    public Color normalColor = new Color(0.5f, 0.5f, 0.5f); // Grau
    public Color hoverColor = Color.yellow; // Gelb beim Hovern
    [Range(0.1f, 20f)] public float fadeSpeed = 5f; // Geschwindigkeit des Farbwechsels

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

        material.color = currentColor;
    }

    // Diese Methoden werden vom Fadenkreuz-Script aufgerufen
    public void OnHoverStart() => isHovering = true;
    public void OnHoverEnd() => isHovering = false;
}
