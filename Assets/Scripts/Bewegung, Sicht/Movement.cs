using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 5f;
    Vector2 horizontalInput;

    // === Etagen-System ===
    [Header("Etagen (Player Y-Position in Welt)")]
    [SerializeField] float[] etagen = { 0f, 4f, 8f };
    [SerializeField] int startEtage = 0;
    [SerializeField] float uebergangsGeschwindigkeit = 10f;

    private int aktuelleEtage;
    private float zielHoehe;
    private bool wechseltEtage = false;

    // === Freeze-System ===
    public static bool inputGesperrt = false;

    private void Start()
    {
        aktuelleEtage = startEtage;
        zielHoehe = etagen[aktuelleEtage];

        // Player auf Starthöhe setzen
        controller.enabled = false;
        Vector3 pos = transform.position;
        pos.y = zielHoehe;
        transform.position = pos;
        controller.enabled = true;
    }

    private void Update()
    {
        if (inputGesperrt) return;

        // === Etagen-Steuerung ===
        if (Input.GetKeyDown(KeyCode.LeftShift) && !wechseltEtage)
        {
            if (aktuelleEtage < etagen.Length - 1)
            {
                aktuelleEtage++;
                zielHoehe = etagen[aktuelleEtage];
                wechseltEtage = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !wechseltEtage)
        {
            if (aktuelleEtage > 0)
            {
                aktuelleEtage--;
                zielHoehe = etagen[aktuelleEtage];
                wechseltEtage = true;
            }
        }

     // === Etagenwechsel (Player teleportieren) ===
if (wechseltEtage)
{
    controller.enabled = false;
    Vector3 pos = transform.position;
    pos.y = zielHoehe;
    transform.position = pos;
    controller.enabled = true;
    wechseltEtage = false;
    return;
}


        // === Horizontale Bewegung ===
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }
}
