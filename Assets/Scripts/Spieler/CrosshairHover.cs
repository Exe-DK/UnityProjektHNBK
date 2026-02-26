using UnityEngine;

public class CrosshairHover : MonoBehaviour
{
    [Header("Fadenkreuz Einstellungen")]
    
    [Tooltip("Wie weit der Raycast reicht (in Metern)")]
    [Range(0.1f, 50f)]
    public float erfassungsreichweite = 10f;

    [Header("Debug")]
    [Tooltip("Zeigt den Raycast in der Scene-Ansicht")]
    public bool debugRaycast = false;

    // Kein Start() nötig - hält es simpel
    private ButtonColorChange lastHoveredButton = null;

    private void Update()
    {
        // Unity 6: Camera.main ist weiterhin gültig
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );

        ButtonColorChange currentButton = null;

        if (Physics.Raycast(ray, out RaycastHit hit, erfassungsreichweite))
        {
            hit.collider.TryGetComponent(out currentButton);

            if (debugRaycast)
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {
            if (debugRaycast)
                Debug.DrawRay(ray.origin, ray.direction * erfassungsreichweite, Color.red);
        }

        if (lastHoveredButton != null && lastHoveredButton != currentButton)
            lastHoveredButton.OnHoverEnd();

        if (currentButton != null && currentButton != lastHoveredButton)
            currentButton.OnHoverStart();

        lastHoveredButton = currentButton;
    }
}
