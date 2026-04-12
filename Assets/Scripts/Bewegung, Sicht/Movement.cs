using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 1f;
    Vector2 horizontalInput;

    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    // === Etagen-System ===
    [Header("Etagen-Höhen (Y-Position)")]
    [SerializeField] float[] etagen = { 1.4f, 1.8f, 2.2f };
    [SerializeField] int startEtage = 1;
    [SerializeField] float uebergangsGeschwindigkeit = 5f;

    private int aktuelleEtage;
    private float zielHoehe;

    private void Start()
    {
        aktuelleEtage = startEtage;
        zielHoehe = etagen[aktuelleEtage];

        // Startposition per CharacterController setzen
        controller.enabled = false;
        Vector3 pos = transform.position;
        pos.y = zielHoehe;
        transform.position = pos;
        controller.enabled = true;
    }

    private void Update()
    {
        // === Etagen-Steuerung ===
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (aktuelleEtage < etagen.Length - 1)
            {
                aktuelleEtage++;
                zielHoehe = etagen[aktuelleEtage];
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (aktuelleEtage > 0)
            {
                aktuelleEtage--;
                zielHoehe = etagen[aktuelleEtage];
            }
        }

        // === Höhe sanft anpassen per controller.Move ===
        float aktuelleHoehe = transform.position.y;
        float differenz = zielHoehe - aktuelleHoehe;

        if (Mathf.Abs(differenz) > 0.01f)
        {
            float schritt = differenz * uebergangsGeschwindigkeit * Time.deltaTime;
            controller.Move(new Vector3(0f, schritt, 0f));
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
