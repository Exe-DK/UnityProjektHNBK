using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    [Header("Button Settings")]
    public float pressDistance = 0.1f;
    public float pressSpeed = 5f;
    public float returnSpeed = 3f;
    public Vector3 pressDirection = Vector3.back;

    [Header("Debug / Test")]
    [Tooltip("Erlaubt das Drücken per Taste (nur zum Testen!)")]
    public bool useKeyboardInput = false;
    public KeyCode activationKey = KeyCode.E;

    private Vector3 originalPosition;
    private bool isPressed = false;
    private bool isPressing = false;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // Nur aktiv wenn useKeyboardInput = true (zum Testen)
        if (useKeyboardInput)
        {
            if (Input.GetKeyDown(activationKey)) PressButton();
            if (Input.GetKeyUp(activationKey))   ReleaseButton();
        }

        // Knopf eindrücken
        if (isPressing && !isPressed)
        {
            Vector3 targetPosition = originalPosition + (pressDirection.normalized * pressDistance);
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPosition,
                pressSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                isPressed = true;
                Debug.Log($"{gameObject.name} wurde gedrückt!");
            }
        }
        // Knopf zurückfahren
        else if (!isPressing && isPressed)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalPosition,
                returnSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.localPosition, originalPosition) < 0.01f)
            {
                isPressed = false;
            }
        }
    }

    // Wird von PlayerInteraction aufgerufen
    public void PressButton()
    {
        isPressing = true;
    }

    public void ReleaseButton()
    {
        isPressing = false;
    }
}
