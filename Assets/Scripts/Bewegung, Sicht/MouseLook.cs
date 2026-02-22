using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;
    private float mouseX, mouseY;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private float xClamp = 85F;
    private float xRotation = 0;

    /*void Rotation()
    {
        if (Time.deltaTime == 0)
        {
            return;
        }
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        // Lade die gespeicherten Sensitivity-Werte aus den PlayerPrefs
        sensitivityX = PlayerPrefs.GetFloat("SensitivityX", 1f);
        sensitivityY = PlayerPrefs.GetFloat("SensitivityY", 1f);

        //Initialisiere die SensitivityX und SensitivityY Eigenschaften
        SensitivityX = sensitivityX;
        SensitivityY = sensitivityY;
    }

    public float SensitivityX
    {
        get => sensitivityX;
        set => sensitivityX = value;
    }

    public float SensitivityY
    {
        get => sensitivityY;
        set => sensitivityY = value;
    }

    private void Update()
    {
        
        // Empfange die Mauseingaben
        mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Rotiere den Spieler um die Y-Achse
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

        // Rotiere die Kamera um die X-Achse
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }

    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;

    }
    
}
