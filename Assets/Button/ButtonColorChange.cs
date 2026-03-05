using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour
{
    [Header("Farbeinstellungen")]
    public Color hoverColor = Color.yellow;
    [Range(0.1f, 20f)] public float fadeSpeed = 5f;

    [Header("Bild-Referenz (optional)")]
    public SpriteRenderer buttonImage;
    public Image uiImage;

    private Renderer objRenderer;
    private Material material;
    private bool isHovering = false;
    private Color currentColor;
    private Color normalColor; // ✅ Wird automatisch vom Material gelesen

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        if (objRenderer != null)
        {
            material = new Material(objRenderer.material);
            objRenderer.material = material;

            // ✅ Ursprungsfarbe des Materials speichern
            if (material.HasProperty("_BaseColor"))
                normalColor = material.GetColor("_BaseColor");
            else
                normalColor = material.color;
        }

        currentColor = normalColor;

        if (buttonImage == null)
            buttonImage = GetComponentInChildren<SpriteRenderer>();

        // ✅ Ursprungsfarbe von Sprite/UI speichern falls kein 3D-Renderer
        if (material == null)
        {
            if (buttonImage != null)
                normalColor = buttonImage.color;
            else if (uiImage != null)
                normalColor = uiImage.color;
        }
    }

    void Update()
    {
        Color targetColor = isHovering ? hoverColor : normalColor;
        currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);

        if (material != null)
        {
            material.color = currentColor;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", currentColor);
        }

        if (buttonImage != null)
            buttonImage.color = currentColor;

        if (uiImage != null)
            uiImage.color = currentColor;
    }

    public void OnHoverStart() => isHovering = true;
    public void OnHoverEnd() => isHovering = false;
}
