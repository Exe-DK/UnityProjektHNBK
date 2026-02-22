using UnityEngine;
using System.Collections;

public class CameraTeleport : MonoBehaviour
{
    public Vector3 firstPosition; // Die feste Zielposition für die erste Position
    public Quaternion firstRotation; // Die feste Zielrotation für die erste Position
    public GameObject playerObject; // Das Spieler-Objekt (Zylinder)
    public float transitionDuration = 1.0f; // Dauer des sanften Schwenkens

    private Vector3 originalPosition; // Ursprüngliche Position des Spielers
    private Quaternion originalRotation; // Ursprüngliche Rotation des Spielers
    private bool isAtFirstPosition = true; // Flag, ob der Spieler an der ersten Position ist
    private bool isTeleporting = false; // Verhindert wiederholte Teleportation während des Timers

    void Start()
    {
        // Speichern der ursprünglichen Position und Rotation des Spielers
        originalPosition = playerObject.transform.position;
        originalRotation = playerObject.transform.rotation;
    }

    void Update()
    {
        // Wenn beim Anvisieren des Frequenzumrichters die F-Taste oder LMB gedrückt wird und der Spieler nicht teleportiert wird
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0)) && !isTeleporting && IsFrequenzumrichterClicked())
        {
            if (isAtFirstPosition)
            {
                // Setze den Spieler auf die erste feste Position
                StartCoroutine(SmoothTransition(firstPosition, firstRotation));
            }
            else
            {
                // Aktiviere die Steuerung und setze die Position auf den Y-Wert 6
                StartCoroutine(EnableMovementAndMoveToY6());
            }
        }
    }

    bool IsFrequenzumrichterClicked()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // Überprüfe, ob der getroffene Collider den Tag "Frequenzumrichter" hat
            if (hit.transform.CompareTag("Frequenzumrichter"))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator SmoothTransition(Vector3 targetPosition, Quaternion targetRotation)
    {
        isTeleporting = true;

        Vector3 startPosition = playerObject.transform.position;
        Quaternion startRotation = playerObject.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            playerObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);
            playerObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Setze die Position und Rotation auf die Zielwerte
        playerObject.transform.position = targetPosition;
        playerObject.transform.rotation = targetRotation;

        // Deaktiviere die Bewegung
        DisableMovement();

        // Setze das Flag, dass der Spieler jetzt an der ersten Position ist
        isAtFirstPosition = false;

        isTeleporting = false;
    }

    IEnumerator EnableMovementAndMoveToY6()
    {
        isTeleporting = true;

        if (playerObject != null)
        {
            // Deaktiviere die Bewegung
            DisableMovement();

            // Setze die Position auf den Y-Wert 6
            Vector3 newPosition = playerObject.transform.position;
            newPosition.y = 6;
            yield return StartCoroutine(SmoothTransition(newPosition, playerObject.transform.rotation));

            // Reaktiviere die Bewegung
            EnableMovement();

            // Setze das Flag, dass der Spieler nicht mehr an der ersten Position ist
            isAtFirstPosition = true;
        }

        isTeleporting = false;
    }

    void DisableMovement()
    {
        var movementScript = playerObject.GetComponent<Movement>();
        var mouseLookScript = playerObject.GetComponent<MouseLook>();

        if (movementScript != null)
            movementScript.enabled = false;

        if (mouseLookScript != null)
            mouseLookScript.enabled = false;
    }

    void EnableMovement()
    {
        var movementScript = playerObject.GetComponent<Movement>();
        var mouseLookScript = playerObject.GetComponent<MouseLook>();

        if (movementScript != null)
            movementScript.enabled = true;

        if (mouseLookScript != null)
            mouseLookScript.enabled = true;
    }
}


