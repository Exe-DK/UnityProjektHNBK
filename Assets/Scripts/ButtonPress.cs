using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    [Header("Button Settings")]
    public float pressDistance = 0.1f;      // Wie weit der Knopf eingedrückt wird
    public float pressSpeed = 5f;           // Geschwindigkeit des Eindrückens
    public float returnSpeed = 3f;          // Geschwindigkeit des Zurückkehrens
    public Vector3 pressDirection = Vector3.back; // Richtung des Eindrückens (z. B. back, down, left)
    public KeyCode activationKey = KeyCode.E; // Taste zum Drücken (optional)

    private Vector3 originalPosition;
    private bool isPressed = false;
    private bool isPressing = false;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // Optional: Tastendruck (z. B. "E") zum Testen
        if (Input.GetKeyDown(activationKey))
        {
            PressButton();
        }
        if (Input.GetKeyUp(activationKey))
        {
            ReleaseButton();
        }

        // Bewegung des Knopfs
        if (isPressing && !isPressed)
        {
            // Knopf in die definierte Richtung eindrücken
            Vector3 targetPosition = originalPosition + (pressDirection.normalized * pressDistance);
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPosition,
                pressSpeed * Time.deltaTime
            );

            // Prüfen, ob der Knopf vollständig eingedrückt ist
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                isPressed = true;
                Debug.Log("Button pressed!");
                // Hier kannst du eine Aktion auslösen (z. B. Tür öffnen)
            }
        }
        else if (!isPressing && isPressed)
        {
            // Knopf zurückfahren
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalPosition,
                returnSpeed * Time.deltaTime
            );

            // Prüfen, ob der Knopf zurück ist
            if (Vector3.Distance(transform.localPosition, originalPosition) < 0.01f)
            {
                isPressed = false;
            }
        }
    }

    // Wird von Raycast aufgerufen (siehe PlayerInteraction-Script)
    public void PressButton()
    {
        isPressing = true;
    }

    public void ReleaseButton()
    {
        isPressing = false;
    }
}
