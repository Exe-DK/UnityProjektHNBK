using UnityEngine;

public class CrosshairHover : MonoBehaviour
{
    [Header("Fadenkreuz Einstellungen")]
    [Tooltip("Wie weit der Raycast reicht (in Metern)")]
    [Range(0.1f, 50f)]
    public float erfassungsreichweite = 10f;

    [Header("Layer Filter")]
    [Tooltip("Wähle hier den Layer 'Buttons' aus")]
    public LayerMask interactableLayer = ~0; // Standard: Alles

    [Header("Debug")]
    [Tooltip("Zeigt den Raycast in der Scene-Ansicht")]
    public bool debugRaycast = false;

    private ButtonColorChange lastHoveredButton = null;

    private void Update()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );

        ButtonColorChange currentButton = null;

        // RaycastAll - geht durch ALLE Objekte durch
        RaycastHit[] hits = Physics.RaycastAll(ray, erfassungsreichweite, interactableLayer);

        // Nach Entfernung sortieren (nächstes zuerst)
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out ButtonColorChange button))
            {
                currentButton = button;
                break; // Nächsten Button gefunden, fertig
            }
        }

        if (debugRaycast)
        {
            if (currentButton != null)
                Debug.DrawRay(ray.origin, ray.direction * erfassungsreichweite, Color.green);
            else
                Debug.DrawRay(ray.origin, ray.direction * erfassungsreichweite, Color.red);
        }

        if (lastHoveredButton != null && lastHoveredButton != currentButton)
            lastHoveredButton.OnHoverEnd();

        if (currentButton != null && currentButton != lastHoveredButton)
            currentButton.OnHoverStart();

        lastHoveredButton = currentButton;
    }
}
