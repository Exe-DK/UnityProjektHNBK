using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorDrehung : MonoBehaviour
{

    // Start wird nur zum Start abgerufen
    void Start()
    {

    }

    // Update wird als Schleife immer wieder aufgerufen
    void Update()
    {
        float Drehzahl = Berechnung.nAP;
        //Debug.Log(Drehzahl);
        transform.Rotate(Vector3.up * Time.deltaTime * Drehzahl);
    }
}
