using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Verwende TextMeshPro-Namespace

public class KennlinieI : MonoBehaviour
{



    // Referenz auf eine Instanz von Potentiometer
    // Ohne diese Referenz würde Werte aus Potentiometer.cs statisch wirken. 
    // Tatsächlich ist der Wert verändlich. potentiometerInstanz wird in Update immer wieder neu geladen
    public ReglerDrehung potentiometer;





    // Anfang UI Referenz
    // Anzeige der Variabel in UI - Worldspace - Panel
    //
    // In Unity, in einem Canvas, muss unter UI > Text Meshpro erstellen, Text löschen, Farbe ändern und mit dem Skript "Berechnungen" verknüpfen

    // public TextMeshProUGUI netzfrequenzText; // Hier wird die Textkomponente für Netzfrequenz referenziert
    //public TextMeshProUGUI drehmomentText;   // Hier wird die Textkomponente für Drehmoment referenziert
    //public TextMeshProUGUI drehzahlText;   // Hier wird die Textkomponente für Drehzahl referenziert
    //public TextMeshProUGUI stromText;   // Hier wird die Textkomponente für Strom referenziert
    // Ende UI Referenz




    //Start: LineRenderer for M-n Kennlinie
    public LineRenderer drehmomentenKurvenRenderer;
    //Ende: LineRenderer for M-n Kennlinie

    //Start: LineRenderer for I-n Kennlinie
    public LineRenderer StromKurvenRenderer;
    //Ende: LineRenderer for I-n Kennlinie


    // Variablen

    //private const int p = 1; // Polpaarzahl
    //private const int m = 3; // Phasen
    public float Netzfrequenz; // Netzfrequenz
    public float n; // Netzdrehzahl (FU ändert diese Variable)
    //public float nr; // Rotordrehzahl
    public float ws; // Synchrondrehfrequenz
    public float U; // Eingangsspannung; Vorsicht bei f > 50Hz; Extra Funktion notwendig
    //private float In = 1; // Bemessungsstrom
    //private float R1 = 69.07f; // Statorwirkwiderstand
    private float R2 = 40.9f; // Rotorwirkwiderstand
    //private float Xh = 1552f; // Hauptblindwiderstand
    //private float X1 = 116.000048f; // Blindwiderstand Stator
    //private float X2 = 101.649952f; // Blindwiderstand Rotor
    public float Xsigma = 217.65f; // Hauptblindwiderstand
    private float Lsigmas = 0.369f; // Statorstreuinduktivität
    private float Lsigmar = 0.324f; // Rotorstreuinduktivität
    //private float Lh = 4.94f; // Hauptinduktivität
    //private float Ls = 5.309f; // Statorinduktivität
    //private float Lr = 5.264f; // Rotorinduktivität
    //private float sigma = 0.127f; // Streuung
    //private float SigmaS = 23.481f; // Statorstreuung
    //private float SigmaR = 20.576f; // Rotorstreuung
    //public float sn = 0.067f; // Bemessungsschlupf
    //public float sk; // Kippschlupf
    //public float Mk; // Kippmoment
    //public float M; // Drehmoment
    //public float Drehmoment; // DRehmomenttemp
    //public float drehmomentBremse; // Drehmoment der Bremse
    public float Strom; // Momentanstrom

    private float schlupf; // Schlupf-temp






    //Start: I-n Kennlinie:
    void BerechneUndZeigeStromKurve()
    {
        // Liste zum Speichern der Strom-Kurve (x: Schlupf, y: Strom)
        List<Vector3> StromKurve = new List<Vector3>();

        // Iteriere durch verschiedene Schlupfwerte und berechne den Strom
        for (float schlupf = 0f; schlupf <= 1.0f; schlupf += 0.01f)
        {
            //float Strom; // Variable für den Strom
            float Umdrehung; // Variable für die Umdrehungszahl des Rotors
            Xsigma = ws * (Lsigmas + Lsigmar);


            // Hier die Berechnung des Stroms basierend auf dem gegebenen Schlupf
            //if (schlupf != 0) // Wenn der Schlupf nicht null ist, wird der Strom wie folgt berechnet.
            //{
            // Berechne den Nenner der Formel
            float Nenner = Mathf.Sqrt(Mathf.Pow(R2 / schlupf, 2) + Mathf.Pow(Xsigma, 2));

            // Berechne den Strom
            //Strom ist zu gering; für n=2800 --> In= 0,62A; Koeffizient alpha dient zur Korrektur
            float alpha = 1.62f;
            Strom = alpha * U / Nenner;



            Umdrehung = Netzfrequenz * 60f * (1 - schlupf); // Umdrehung basierend auf Schlupf

            // Füge das Drehmoment zur Kurve hinzu (x: Drehzahl, y: Strom, z: 0)
            StromKurve.Add(new Vector3(Umdrehung, Strom, 0f));
        }

        // Zeige die Kurve mit dem Line Renderer
        StromKurvenRenderer.positionCount = StromKurve.Count;
        for (int i = 0; i < StromKurve.Count; i++)
        {
            // Setze die Position für den Line Renderer
            StromKurvenRenderer.SetPosition(i, StromKurve[i]);

            // Debug-Log für jeden Schleifen-Durchlauf
            //Debug.Log($"Setze Position {i}: Umdrehung={StromKurve[i].x}, Drehmoment={StromKurve[i].y}");
        }
    }
    //Ende: I-n Kennlinie




    // Start is called before the first frame update
    void Start()
    {
        Netzfrequenz = 0f;
        ws = 2 * Mathf.PI * Netzfrequenz; // Drehfrequenz als f(w)
        /*   
        n = Netzfrequenz * 60; // Netzdrehzahl
        sn = 0.067f; // Bemessungsschlupf
        nr = n * sn; // Rotordrehzahl als Funktion des Schlupfes
       */
    }





    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Update-Methode wird aufgerufen.");

        //Start: Werte aus anderen Skripten oder GameObjects 

        // Zugriff auf die Variable frequenz aus Potentiometer.cs. Die Instanz wurde geschrieben, so dass Netzfrequenz sich refresht
        Netzfrequenz = ReglerDrehung.Frequenz;

        //Ende: Werte aus anderen Skripten oder GameObjects 


        // Start: Spannungsanpassung
        //Debug.Log("Spannungsanpassung wird berechnet.");
        if (Netzfrequenz <= 50)
        {
            U = 400 * Netzfrequenz / 50; // SPannung ist proportional zu der Netzfrequenz, kein Boost
        }
        else
        {
            U = 400;                    // Über 50Hz bleibt die SPannung maximal auf 400V
        }
        // Ende: Spannungsanpassung








        /*

        //Start: Berechnung verschiedener Variablen
        Debug.Log("verschiedene Variablen werden berechnet.");

        //Drehzahl, Drehfrequenz, Schlupf
        n = Netzfrequenz * 60; // Netzdrehzahl
        */
        ws = 2 * Mathf.PI * Netzfrequenz; // synchronDrehfrequenz
        /*
        nr = n * (1 - sn); //
                           //schlupf = (n - nr) / n;


        // Kippschlupf
        sk = R2 / Xsigma;

        //Kippmoment
        Mk = 3 * Mathf.Pow(U, 2) / (ws * 2 * ws * (Lsigmas + Lsigmar));
        //Ende: Berechnung verschiedener Variablen


        */


        //Start: Aufruf der Funktionen
        //Debug.Log("Funktionen werden aufgerufen.");

        BerechneUndZeigeStromKurve();

        //Ende: Aufruf der Funktionen
    }

}
