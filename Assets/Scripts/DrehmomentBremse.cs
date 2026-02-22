using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DrehmomentBremse : MonoBehaviour
{
    public float schrittweite = 0.1f;
    public float MBremse = 0.0f;
    public static float AktuellesDrehmomentBremse;
    

    void Start()
    {

    }

    void Update()
    {
        // Überprüfe den Mausklick und führe entsprechende Aktionen aus

        int layerMask = LayerMask.GetMask("Trigger Layer");

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
            {
                //Debug.Log("Hit object: " + hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Trigger Plus"))
                {
                    //Debug.Log("Plus");
                    PLUS();
                }
                if (hit.collider.CompareTag("Trigger Minus"))
                {
                    //Debug.Log("Minus");
                    MINUS();
                }
            }
        }
    }

    public void PLUS()
    {

        MBremse += schrittweite;
        AktuellesDrehmomentBremse = MBremse;
       Debug.Log("MBremse " + AktuellesDrehmomentBremse);
    }

    public void MINUS()
    {
        MBremse -= schrittweite;
        AktuellesDrehmomentBremse = MBremse;
        Debug.Log("MBremse " + AktuellesDrehmomentBremse);
    }
}