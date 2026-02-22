using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Verwende TextMeshPro-Namespace

public class SinusNetz : MonoBehaviour
{
    // Start: LineRenderer for Netzfrequenz
    public LineRenderer SinusNetzfrequenzRenderer;


    // Variablen
    public float Netzfrequenz; // Netzfrequenz
    public float ws; // Synchrondrehfrequenz
    public float U; // Eingangsspannung; Vorsicht bei f > 50Hz; Extra Funktion notwendig
    public float Udach; //Scheitelspannung

    // Start: Sinus Netzfrequenz Kennlinie:
    void BerechneUndZeigeSinuskurve()
    {
        List<Vector3> sinuskurve = new List<Vector3>();

        // Iteriere durch verschiedene Winkelwerte und berechne den Sinus
        for (float angle = 0f; angle <= 720f; angle += 10f)
        {
            // Berechne den Sinuswert für den aktuellen Winkel
            float sinusValue = Mathf.Sin(Mathf.Deg2Rad * angle);

            // Füge den Punkt zur Sinuskurve hinzu (x: Winkel, y: Sinuswert, z: 0)
            sinuskurve.Add(new Vector3(angle, sinusValue, 0f));
        }

        // Zeige die Sinuskurve mit dem Line Renderer
        SinusNetzfrequenzRenderer.positionCount = sinuskurve.Count;
        for (int i = 0; i < sinuskurve.Count; i++)
        {
            // Setze die Position für den Line Renderer
            SinusNetzfrequenzRenderer.SetPosition(i, sinuskurve[i]);



        }


    }

    // ENDE: Sinus Netzfrequenz Kennlinie:




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BerechneUndZeigeSinuskurve();
    }
}