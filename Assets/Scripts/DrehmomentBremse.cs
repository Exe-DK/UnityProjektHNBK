using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrehmomentBremse : MonoBehaviour
{
    public float schrittweite = 0.1f;
    public float MBremse = 0.0f;
    public static float AktuellesDrehmomentBremse;

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
