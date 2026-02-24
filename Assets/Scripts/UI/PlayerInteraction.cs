using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    public LayerMask interactableLayer;

    private GlowEffect currentGlow;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            // Prüfen, ob das getroffene Objekt ein Button ist
            ButtonPress button = hit.collider.GetComponent<ButtonPress>();
            if (button != null)
            {
                // Glow-Effekt aktivieren (nur wenn Fadenkreuz über dem Knopf ist)
                GlowEffect glow = hit.collider.GetComponent<GlowEffect>();
                if (glow != null && currentGlow != glow)
                {
                    if (currentGlow != null)
                        currentGlow.DisableGlow();

                    glow.EnableGlow();
                    currentGlow = glow;
                }

                // Button-Interaktion (optional)
                if (Input.GetMouseButtonDown(0))
                {
                    button.PressButton();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    button.ReleaseButton();
                }
            }
        }
        else
        {
            // Glow deaktivieren, wenn kein Objekt getroffen wird
            if (currentGlow != null)
            {
                currentGlow.DisableGlow();
                currentGlow = null;
            }
        }
    }
}
