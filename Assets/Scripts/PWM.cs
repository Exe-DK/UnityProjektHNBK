using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWM : MonoBehaviour
{
    public LineRenderer PWMControllerRenderer;
    public LineRenderer sinusRenderer;  // LineRenderer für den Sinus

    public ReglerDrehung potentiometer;

    public float Netzfrequenz;
    public float PWM_Frequenz = 1000f; // PWM-Frequenz (z. B. 1 kHz)

    float Periodendauer;
    float Pulsweite;
    float T_on;
    float T_off;

    // Maximale Anzeigedauer: 25 ms (0.025 Sekunden)
    float maxAnzeigeDauer = 0.025f;
    float graphWidth = 5.0f; // Feste Breite für das Diagramm

    void Start()
    {
        // Die Achse wird extern verwaltet, daher keine Notwendigkeit, sie hier zu zeichnen
    }

    void Update()
    {
        Netzfrequenz = Mathf.Max(ReglerDrehung.Frequenz, 1); // Mindestens 1 Hz

        DrawLine();
        DrawSinus();
    }

    void DrawLine()
    {
        float skalierungsFaktor = graphWidth / maxAnzeigeDauer; // Stellt sicher, dass 25ms immer gleich breit sind

        List<Vector3> punkte = new List<Vector3>();
        float zeit = 0f;
        float x = 0f;

        while (zeit < maxAnzeigeDauer)
        {
            float sinuswert = Mathf.Sin(2 * Mathf.PI * Netzfrequenz * zeit); // Sinuswert berechnen
            T_on = (sinuswert + 1) / 2 * (1.0f / PWM_Frequenz); // Skaliertes PWM-Signal
            T_off = (1.0f / PWM_Frequenz) - T_on; // Restzeit

            // PWM Rechteck erzeugen mit korrekt skalierter X-Achse
            punkte.Add(new Vector3(x, 0f, 0f));    // Start Low
            punkte.Add(new Vector3(x, 1f, 0f));    // Anstieg High
            x += T_on * skalierungsFaktor;
            punkte.Add(new Vector3(x, 1f, 0f));    // Bleibt High
            punkte.Add(new Vector3(x, 0f, 0f));    // Abfall Low
            x += T_off * skalierungsFaktor;

            zeit += (1.0f / PWM_Frequenz);
        }

        PWMControllerRenderer.positionCount = punkte.Count;
        PWMControllerRenderer.SetPositions(punkte.ToArray());
    }

    void DrawSinus()
    {
        int punkteProSinus = 100; // Anzahl der Punkte für eine glatte Sinuskurve
        float skalierungsFaktor = graphWidth / maxAnzeigeDauer; // Stellt sicher, dass 25ms immer gleich breit sind

        List<Vector3> punkte = new List<Vector3>();
        float zeit = 0f;
        float x = 0f;

        for (int i = 0; i < punkteProSinus; i++)
        {
            float sinuswert = Mathf.Sin(2 * Mathf.PI * Netzfrequenz * zeit); // Sinuswert berechnen
            x = zeit * skalierungsFaktor;
            punkte.Add(new Vector3(x, sinuswert, 0f));

            zeit += maxAnzeigeDauer / punkteProSinus;
        }

        sinusRenderer.positionCount = punkte.Count;
        sinusRenderer.SetPositions(punkte.ToArray());
    }
}










