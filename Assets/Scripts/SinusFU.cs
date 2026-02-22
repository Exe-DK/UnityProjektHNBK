using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SinusFU : MonoBehaviour
{
    public LineRenderer Sinus_FU_sekURenderer;
    public ReglerDrehung potentiometer;
    public float Netzfrequenz;

    void BerechneUndZeigeFU_Sinus()
    {
        float f = Netzfrequenz / 50;

        List<Vector3> Sinus_FU_sekU = new List<Vector3>();

        // Iteriere durch verschiedene Winkelwerte und berechne den Sinus
        for (float angle = 0f; angle <= 1000f; angle += 10f)
        {
            // Berechne den Sinuswert für den aktuellen Winkel
            float sinusValue = Mathf.Sin(Mathf.Deg2Rad * angle * f);

            // Füge den Punkt zur Sinuskurve hinzu (x: Winkel, y: Sinuswert, z: 0)
            Sinus_FU_sekU.Add(new Vector3(angle, sinusValue, 0f));
        }

        // Zeige die Sinuskurve mit dem Line Renderer
        Sinus_FU_sekURenderer.positionCount = Sinus_FU_sekU.Count;
        for (int i = 0; i < Sinus_FU_sekU.Count; i++)
        {
            // Setze die Position für den Line Renderer
            Sinus_FU_sekURenderer.SetPosition(i, Sinus_FU_sekU[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fügen Sie hier ggf. Initialisierungscodes hinzu
    }

    // Update is called once per frame
    void Update()
    {
        Netzfrequenz = ReglerDrehung.Frequenz;
        BerechneUndZeigeFU_Sinus();
    }
}