using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;
    private float mouseX, mouseY;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private float xClamp = 85f;
    private float xRotation = 0f;

    void Start()
    {
        // Auto-find Kamera falls nicht zugewiesen
        if (playerCamera == null)
        {
            Camera cam = GetComponentInChildren<Camera>();
            if (cam != null) playerCamera = cam.transform;
            else playerCamera = Camera.main.transform;
        }

        sensitivityX = PlayerPrefs.GetFloat("SensitivityX", 1f);
        sensitivityY = PlayerPrefs.GetFloat("SensitivityY", 1f);
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
        if (Movement.inputGesperrt) return;

        mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

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
