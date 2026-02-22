using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius für die Objekterkennung
    public LayerMask interactableLayer = ~0; // Standardmäßig auf "Everything" gesetzt
    public Transform player; // Referenz zum Spieler-Objekt (Cylinder)

    void Update()
    {
      

        // **Prüfen, ob die Höhe des Cylinders größer als 6.5 ist**
        if (player != null && player.position.y > 6.5f)
        {
            
            HideAllOutlines();
            return; // **Beende die Methode, damit keine Outlines aktiviert werden**
        }

        // Finde alle Objekte im Umkreis des Spielers
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, interactableLayer);
       

        // Deaktiviere alle aktuellen Umrisse
        HideAllOutlines();

        // Aktiviere Umrisse für alle gefundenen Objekte
        foreach (Collider col in colliders)
        {
            OutlineRenderer outline = col.GetComponent<OutlineRenderer>();
            if (outline != null)
            {
                outline.ShowOutline();
               
            }
        }
    }

    // **Hilfsmethode, um alle Outlines zu deaktivieren**
    private void HideAllOutlines()
    {
        foreach (OutlineRenderer outline in FindObjectsOfType<OutlineRenderer>())
        {
            outline.HideOutline();
        }
    }
}
