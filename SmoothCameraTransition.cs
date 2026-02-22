using UnityEngine;

public class SmoothCameraTransition : MonoBehaviour
{
    public Vector3 targetPosition; // Zielposition der Kamera
    public Vector3 targetRotationEuler; // Zielrotation der Kamera in Euler-Winkeln
    public float transitionSpeed = 2f; // Geschwindigkeit des Übergangs

    public MonoBehaviour inputManager; // Referenz zum Input Manager Skript
    public MonoBehaviour movement; // Referenz zum Movement Skript
    public MonoBehaviour mouseLook; // Referenz zum Mouse Look Skript

    private Vector3 originalPosition; // Ursprüngliche Position der Kamera
    private Quaternion originalRotation; // Ursprüngliche Rotation der Kamera
    private bool isAtTarget = false; // Status, ob die Kamera an der Zielposition ist
    private bool isTransitioning = false; // Status, ob ein Übergang läuft

    private void Start()
    {
        // Speichere die ursprüngliche Position und Rotation der Kamera
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        // Überprüfen, ob der Spieler die E-Taste drückt
        if (Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            // Starte die Transition
            StartCoroutine(SwitchPosition());
        }
    }

    private System.Collections.IEnumerator SwitchPosition()
    {
        isTransitioning = true;

        // Deaktiviere die anderen Skripte
        inputManager.enabled = false;
        movement.enabled = false;
        mouseLook.enabled = false;

        // Definiere das Ziel (je nach aktuellem Status)
        Vector3 targetPos = isAtTarget ? originalPosition : targetPosition;
        Quaternion targetRot = isAtTarget ? originalRotation : Quaternion.Euler(targetRotationEuler);

        // Übergang durch Interpolation
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * transitionSpeed;
            transform.position = Vector3.Lerp(transform.position, targetPos, progress);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, progress);
            yield return null;
        }

        // Stelle sicher, dass die Kamera exakt an der Zielposition endet
        transform.position = targetPos;
        transform.rotation = targetRot;

        // Umschalten des Status
        isAtTarget = !isAtTarget;
        isTransitioning = false;

        // Aktiviere die anderen Skripte wieder, wenn die Kamera zur ursprünglichen Position zurückkehrt
        if (!isAtTarget)
        {
            inputManager.enabled = true;
            movement.enabled = true;
            mouseLook.enabled = true;
        }
    }
}

