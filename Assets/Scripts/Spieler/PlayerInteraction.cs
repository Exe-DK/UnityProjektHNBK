using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Movement.inputGesperrt) return;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            ButtonPress button = hit.collider.GetComponent<ButtonPress>();
            if (button != null)
            {
                if (Input.GetMouseButtonDown(0))
                    button.PressButton();
                if (Input.GetMouseButtonUp(0))
                    button.ReleaseButton();
            }
        }
    }
}
