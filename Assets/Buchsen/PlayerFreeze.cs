using UnityEngine;

public class PlayerFreeze : MonoBehaviour
{
    public static PlayerFreeze Instance;

    private Movement movement;
    private MouseLook mouseLook;
    private bool isFrozen = false;

    public bool IsFrozen => isFrozen;

    void Awake()
    {
        Instance = this;
        movement = GetComponent<Movement>();
        mouseLook = GetComponent<MouseLook>();
    }

    public void Freeze()
    {
        isFrozen = true;
        movement.enabled = false;
        mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Unfreeze()
    {
        isFrozen = false;
        movement.enabled = true;
        mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
