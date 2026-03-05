using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonColorChange : MonoBehaviour
{
    [Header("Farbeinstellungen")]
    public Color normalColor = new Color(0.5f, 0.5f, 0.5f);
    public Color hoverColor = Color.yellow;
    [Range(0.1f, 20f)] public float fadeSpeed = 5f;

    [Header("Klick-Event")]
    public UnityEvent onClick;

    [Header("Bild-Referenz (optional)")]
    public SpriteRenderer buttonImage;
    public Image uiImage;

    private Renderer objRenderer;
    private Material material;
    private bool isHovering = false;
    private Color currentColor;

       void Start()
    {
        objRenderer = GetComponent<Renderer>();

        if (objRenderer != null)
        {
            material = new Material(objRenderer.material);
            objRenderer.material = material;

            if (material.HasProperty("_BaseColor"))
                normalColor = material.GetColor("_BaseColor");
            else
                normalColor = material.color;
        }

        currentColor = normalColor;

        if (buttonImage == null)
            buttonImage = GetComponentInChildren<SpriteRenderer>();

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
        if (isHovering && Input.GetMouseButtonDown(0))
            onClick.Invoke();

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
