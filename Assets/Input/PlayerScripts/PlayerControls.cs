using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        // Bewegung in horizontaler Richtung
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f).normalized;

        // Bewegung ausführen
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
