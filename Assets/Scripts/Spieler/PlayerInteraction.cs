using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    public LayerMask interactableLayer; // Layer für Knöpfe (z. B. "Interactable")

    void Update()
    {
        // Raycast vom Kamera-Mittelpunkt
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            // Prüfen, ob das getroffene Objekt ein Button ist
            ButtonPress button = hit.collider.GetComponent<ButtonPress>();
            if (button != null)
            {
                if (Input.GetMouseButtonDown(0)) // Linke Maustaste
                {
                    button.PressButton();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    button.ReleaseButton();
                }
            }
        }
    }
}