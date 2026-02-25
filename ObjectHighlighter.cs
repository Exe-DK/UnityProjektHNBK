using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    public Camera playerCamera;  // Die Kamera des Spielers
    public float maxDistance = 5f;  // Maximale Distanz für das Raycasting

    private OutlineRenderer currentOutline;  // Der aktuelle OutlineRenderer

    void Update()
    {
        // Raycast von der Kamera in die Blickrichtung des Spielers
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Überprüfen, ob das getroffene Objekt einen OutlineRenderer hat
            OutlineRenderer outline = hit.collider.GetComponent<OutlineRenderer>();

            if (outline != null)
            {
                // Wenn es einen neuen OutlineRenderer gibt, den aktuellen deaktivieren
                if (currentOutline != null && currentOutline != outline)
                {
                    currentOutline.HideOutline();
                }

                // Den neuen OutlineRenderer aktivieren
                outline.ShowOutline();
                currentOutline = outline;
            }
            else if (currentOutline != null)
            {
                // Wenn kein OutlineRenderer getroffen wurde, den aktuellen deaktivieren
                currentOutline.HideOutline();
                currentOutline = null;
            }
        }
        else if (currentOutline != null)
        {
            // Wenn nichts getroffen wurde, den aktuellen OutlineRenderer deaktivieren
            currentOutline.HideOutline();
            currentOutline = null;
        }
    }
}
