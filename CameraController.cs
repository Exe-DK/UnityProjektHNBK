using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;  // Hauptkamera, die wir steuern wollen
    public Camera displayCamera;  // Kamera, die zum Display wechselt
    public float smoothSpeed = 0.125f;  // Geschwindigkeit der Kamerabewegung

    private bool transitioning = false;  // Flag, das den Übergang überwacht
    private Vector3 targetPosition;  // Zielposition für den Übergang

    private void Start()
    {
        if (Camera == null)
            Camera = Camera.main; // Wenn keine Kamera zugewiesen wurde, verwenden wir die Hauptkamera

        if (displayCamera != null)
            displayCamera.gameObject.SetActive(false); // Deaktiviert die displayCamera zu Beginn
    }

    private void Update()
    {
        // Wenn der Spieler auf das Display klickt (hier: Beispiel mit 'E'-Taste)
        if (Input.GetKeyDown(KeyCode.E) && !transitioning) // Du kannst hier jede beliebige Bedingung setzen
        {
            StartTransition();
        }

        // Falls der Übergang aktiv ist, die Position der Kamera mit sanfter Bewegung ändern
        if (transitioning)
        {
            MoveCameraToDisplay();
        }
    }

    // Startet die Kameratransition
    void StartTransition()
    {
        if (displayCamera == null)
        {
            Debug.LogError("DisplayCamera ist nicht zugewiesen!");
            return;
        }

        transitioning = true;  // Übergang aktivieren
        targetPosition = displayCamera.transform.position; // Direkt die Position der DisplayCamera als Ziel verwenden
        Camera.gameObject.SetActive(false); // Deaktiviert die Camera
        displayCamera.gameObject.SetActive(true); // Aktiviert die DisplayCamera
    }

    // Bewegt die Kamera sanft zur Display-Position
    void MoveCameraToDisplay()
    {
        // Sanfte Bewegung der Kamera zur Zielposition
        Vector3 smoothedPosition = Vector3.Lerp(displayCamera.transform.position, targetPosition, smoothSpeed);
        displayCamera.transform.position = smoothedPosition;

        // Optional: Kamera auf das Display ausrichten
        displayCamera.transform.LookAt(displayCamera.transform.position); // Wenn du willst, dass sie immer auf das Display schaut

        // Wenn die Kamera fast am Ziel angekommen ist, beenden wir den Übergang
        if (Vector3.Distance(displayCamera.transform.position, targetPosition) < 0.1f)
        {
            transitioning = false; // Übergang beenden
        }
    }
}

