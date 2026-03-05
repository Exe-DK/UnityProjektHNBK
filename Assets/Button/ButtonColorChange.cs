using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // ✅ NEU

public class ButtonColorChange : MonoBehaviour
{
    [Header("Farbeinstellungen")]
    public Color normalColor = new Color(0.5f, 0.5f, 0.5f);
    public Color hoverColor = Color.yellow;
    [Range(0.1f, 20f)] public float fadeSpeed = 5f;

    [Header("Klick-Event")]
    public UnityEvent onClick; // ✅ NEU – erscheint im Inspector!

    [Header("Bild-Referenz (optional)")]
    public SpriteRenderer buttonImage;
    public Image uiImage;

    private Renderer objRenderer;
    private Material material;
    private bool isHovering = false;
    private Color currentColor;
<<<<<<< Updated upstream
=======
    private Color normalColor;
>>>>>>> Stashed changes

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        if (objRenderer != null)
        {
            // Material kopieren
            material = new Material(objRenderer.material);
            objRenderer.material = material;

<<<<<<< Updated upstream
            // ✅ Wichtig: Farbe auf dem Material setzen, 
            // auch wenn eine Textur drauf liegt!
            material.color = normalColor;

            // ✅ Falls das Shader "_BaseColor" nutzt (URP/HDRP)
=======
>>>>>>> Stashed changes
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", normalColor);
            }
        }

        currentColor = normalColor;

        if (buttonImage == null)
            buttonImage = GetComponentInChildren<SpriteRenderer>();
<<<<<<< Updated upstream
=======

        if (material == null)
        {
            if (buttonImage != null)
                normalColor = buttonImage.color;
            else if (uiImage != null)
                normalColor = uiImage.color;
        }
>>>>>>> Stashed changes
    }

    void Update()
    {
<<<<<<< Updated upstream
        // Sanfter Farbwechsel
=======
        // ✅ NEU – Klick erkennen wenn Hover aktiv
        if (isHovering && Input.GetMouseButtonDown(0))
        {
            onClick.Invoke();
        }

>>>>>>> Stashed changes
        Color targetColor = isHovering ? hoverColor : normalColor;
        currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);

        // ✅ 3D-Material Farbe ändern (funktioniert auch MIT Textur)
        if (material != null)
        {
            material.color = currentColor;

            // ✅ Für URP/HDRP Shader
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", currentColor);
            }
        }

        // ✅ SpriteRenderer Farbe
        if (buttonImage != null)
            buttonImage.color = currentColor;

        // ✅ UI Image Farbe
        if (uiImage != null)
            uiImage.color = currentColor;
    }

    public void OnHoverStart() => isHovering = true;
    public void OnHoverEnd() => isHovering = false;
}
