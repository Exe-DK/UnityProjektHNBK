using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OutlineRenderer : MonoBehaviour
{
    [Header("Outline Settings")]
    public Color outlineColor = Color.yellow;  // Farbe des Umrisses
    public float outlineWidth = 0.05f;        // Breite des Umrisses

    private GameObject outlineObject;        // Duplikat für den Umriss
    private MeshRenderer outlineRenderer;    // Renderer für das Duplikat

    private void Start()
    {
        // Duplikat-Objekt erstellen
        outlineObject = new GameObject("Outline");
        outlineObject.transform.parent = transform;
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = Vector3.one * 1.05f; // Skalierung für den Umriss

        // Mesh-Filter vom Original kopieren
        MeshFilter meshFilter = outlineObject.AddComponent<MeshFilter>();
        meshFilter.mesh = GetComponent<MeshFilter>().mesh;

        // Mesh-Renderer hinzufügen und konfigurieren
        outlineRenderer = outlineObject.AddComponent<MeshRenderer>();

        // Dynamisches Material erstellen und konfigurieren
        Material outlineMaterial = new Material(Shader.Find("Custom/OutlineShader"));
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);

        outlineRenderer.material = outlineMaterial;

        // Standardmäßig den Umriss ausblenden
        outlineRenderer.enabled = false;
    }

    // Methode, um den Umriss zu aktivieren
    public void ShowOutline()
    {
        if (outlineRenderer != null)
            outlineRenderer.enabled = true;
    }

    // Methode, um den Umriss zu deaktivieren
    public void HideOutline()
    {
        if (outlineRenderer != null)
            outlineRenderer.enabled = false;
    }
}
