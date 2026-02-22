using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Verwende TextMeshPro-Namespace

public class Kennlinie : MonoBehaviour
{
    public ReglerDrehung potentiometerInstanz;
    public LineRenderer drehmomentenKurvenRenderer;
    public LineRenderer StromKurvenRenderer;
    public float Netzfrequenz; // Netzfrequenz
    public float n; // Netzdrehzahl (FU ändert diese Variable)
    public float nr; // Rotordrehzahl
    private float ws; // Synchrondrehfrequenz
    public float U; // Eingangsspannung; Vorsicht bei f > 50Hz; Extra Funktion notwendig
    private float R2 = 40.9f; // Rotorwirkwiderstand
    private float Xsigma = 217.65f; // Hauptblindwiderstand
    private float Lsigmas = 0.369f; // Statorstreuinduktivität
    private float Lsigmar = 0.324f; // Rotorstreuinduktivität
    public float sn = 0.067f; // Bemessungsschlupf
    public float sk; // Kippschlupf
    public float Mk; // Kippmoment
    public float M; // Drehmoment
    public float Drehmoment; // DRehmomenttemp
    public float drehmomentBremse; // Drehmoment der Bremse
    private float schlupf; // Schlupf-temp

 void BerechneUndZeigeDrehmomentenKurve()
    {
        // Liste zum Speichern der Drehmomenten-Kurve (x: Umdrehung, y: Drehmoment, z: 0)
        List<Vector3> drehmomentenKurve = new List<Vector3>();
        // Iteriere durch verschiedene Schlupfwerte und berechne das Drehmoment
        for (float schlupf = 0f; schlupf <= 1.0f; schlupf += 0.05f)
        {
            float M; // Variable für das Drehmoment
            float Umdrehung; // Variable für die Umdrehungszahl des Rotors
      
            if (schlupf != 0) // Wenn der Schlupf nicht null ist, wird das Drehmoment wie folgt berechnet.
            {
                M = Mk * (2 / ((schlupf / sk) + (sk / schlupf)));          
            }
            else
            {
                M = 0f;
            }
            Umdrehung = Netzfrequenz * 60f * (1 - schlupf); // Umdrehung basierend auf Schlupf
            drehmomentenKurve.Add(new Vector3(Umdrehung, M, 0f));
        }
        drehmomentenKurvenRenderer.positionCount = drehmomentenKurve.Count;
        for (int i = 0; i < drehmomentenKurve.Count; i++)
        {
            // Setze die Position für den Line Renderer
            drehmomentenKurvenRenderer.SetPosition(i, drehmomentenKurve[i]);
        }
    }

    void Start()
    {
        Netzfrequenz = 0f;
        ws = 2 * Mathf.PI * Netzfrequenz; // Drehfrequenz als f(w)
        n = Netzfrequenz * 60; // Netzdrehzahl
        sn = 0.067f; // Bemessungsschlupf
        nr = n * sn; // Rotordrehzahl als Funktion des Schlupfes
    }

    void Update()
    {

        //Debug.Log("Update-Methode wird aufgerufen.");
        Netzfrequenz = ReglerDrehung.Frequenz;
        //Debug.Log("Spannungsanpassung wird berechnet.");
        if (Netzfrequenz <= 50)
        {
            U = 400 * Netzfrequenz / 50; // Spannung ist proportional zu der Netzfrequenz, kein Boost
        }
        else
        {
            U = 400;                    // Über 50Hz bleibt die SPannung maximal auf 400V
        }

        //Debug.Log("verschiedene Variablen werden berechnet.");

        n = Netzfrequenz * 60; // Netzdrehzahl
        ws = 2 * Mathf.PI * Netzfrequenz; // synchronDrehfrequenz
        nr = n * (1 - sn); // schlupf
        sk = R2 / Xsigma; // Kippschlupf
        Mk = 3 * Mathf.Pow(U, 2) / (ws * 2 * ws * (Lsigmas + Lsigmar));  //Kippmoment

        //Debug.Log("Funktionen werden aufgerufen.");

        BerechneUndZeigeDrehmomentenKurve();
    }

}