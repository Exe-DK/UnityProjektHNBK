using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class Berechnung : MonoBehaviour
{
    public TextMeshProUGUI drehmomentText;
    public TextMeshProUGUI drehzahlText;
    public TextMeshProUGUI bremseText;
    public TextMeshProUGUI stromText;
    public TextMeshProUGUI drehmoment2Text;
    public TextMeshProUGUI drehzahl2Text;
    public LineRenderer drehmomentenKurvenRenderer;

    public static float nAP;    //Drehzahl im Arbeitspunkt
    public static float Map;    //Drehmoment im Arbeitspunkt
    public static float nI;

        // Konstanten
    public float Netzfrequenz = 50.0f;
    public int AnzahlPole = 2;
    public float RotorWirkwiderstand = 40.9f;
    public float Hauptblindwiderstand = 217.65f;
    public float Statorstreuniduktivität = 0.369f;
    public float Rotorstreuinduktivität = 0.324f;
    public float konstantSpannung = 400.0f;
    public float Alpha = 1.62f;
    public float Nennstrom = 1.0f;
    public float Kippmoment = 3.51f;
    public float Nennmoment = 1.26f;
    float cosphi = 0.83f;   //Bemessungswirkfaktor

        // Variablen
    public float synchronDrehzahl;
    public float Schlupf;
    public float Kippschlupf;
    public float synchronFrequenz;
    public float Spannung;
    public float Drehmoment;
    public float DrehmomentAP;  //Drehmoment im Arbeitspunkt
    public float SchlupfAP;     //Schlupf im Arbeitspunkz
    public float DrehzahlAP;    //Drehzahl im Arbeitspunkt
    public float Strom;
    public float Pzu;
    public float Pab;
    public float Wirkungsgrad;
    public float Iap;           //Strom im Arbeitspunkt, nach Näherung 2. Ordnung
   
void Start()
{

    if (drehmomentText == null) Debug.LogError("drehmomentText nicht zugewiesen!");
    if (drehzahlText == null) Debug.LogError("drehzahlText nicht zugewiesen!");
    if (bremseText == null) Debug.LogError("bremseText nicht zugewiesen!");
    if (stromText == null) Debug.LogError("stromText nicht zugewiesen!");
    if (drehmoment2Text == null) Debug.LogError("drehmoment2Text nicht zugewiesen!");
    if (drehzahl2Text == null) Debug.LogError("drehzahl2Text nicht zugewiesen!");
}

    void Update()
    {
        float aktuelleDrehzahl = ReglerDrehung.motorSpeed;
        float drehmomentBremse = DrehmomentBremse.AktuellesDrehmomentBremse;
        float Frequenz = ReglerDrehung.Frequenz;

        Kippschlupf = RotorWirkwiderstand / Hauptblindwiderstand;
        synchronDrehzahl = (120 * Frequenz) / AnzahlPole;
        // Schutz gegen Division durch 0 bei synchronDrehzahl
        if (Mathf.Abs(synchronDrehzahl) < 0.0001f)
        {
        synchronDrehzahl = 0.0001f;
        }

        synchronFrequenz = synchronDrehzahl / 60f;

            // Berechnung Schlupf gemäß der neuen Formel
        float nsr = 200f; //Bemessungsschlupfdrehzahl 
        Schlupf = nsr / synchronDrehzahl;

            // Berechnung Spannung
        Spannung = (Frequenz <= 50) ? 400 * (Frequenz / 50) : 400;

            // Berechnung Drehmoment
        Drehmoment = (Schlupf != 0) ? Kippmoment * 2 / ((Schlupf / Kippschlupf) + (Kippschlupf / Schlupf)) : 0f;

            // Berechnung Drehmoment im Arbeitspunkt (mit Drehmomentbremse)
        DrehmomentAP = Nennmoment + drehmomentBremse;
     
                 // --- Schutz gegen Division durch 0 (physikalisch sinnvoll) ---
        if (Mathf.Abs(SchlupfAP) < 0.0001f)
        {
        SchlupfAP = 0.0001f;
        }
        if (Mathf.Abs(Nennmoment) < 0.0001f)
        {
        Nennmoment = 0.0001f;
        }

            //Berechnung anderer Werte im Arbeitspunkt nach Näherung 2. Ordnung
        SchlupfAP = Schlupf * (DrehmomentAP /  Nennmoment); 
        SchlupfAP = Mathf.Max(Mathf.Abs(SchlupfAP), 0.0001f); 

            //SchlupfAP = ((Kippschlupf * (Kippmoment - Mathf.Pow(((Kippmoment + DrehmomentAP) * (Kippmoment - DrehmomentAP)), 1f / 2f))) / DrehmomentAP);
        DrehzahlAP = (1 - SchlupfAP) * synchronDrehzahl;
        Iap = Nennstrom * (DrehmomentAP / Nennmoment);

        if (DrehmomentAP < Kippmoment)
        {
            nAP = DrehzahlAP;
            Map = DrehmomentAP;
        }
        else
        {
            nAP = 0f;
            Map = 0f;
        }

            // Berechnung Motorstrom I
        Strom = Alpha * Spannung / Mathf.Sqrt(Mathf.Pow(RotorWirkwiderstand / SchlupfAP, 2) + Mathf.Pow(Hauptblindwiderstand, 2));
        nI = Strom;

            // Berechnung der abgegebenen Leistung (Pab)
        Pab = (2 * Mathf.PI * DrehzahlAP / 60) * DrehmomentAP; // Pab = (2π * n / 60) * M

            // Berechnung der aufgenommenen Leistung (Pzu) im Drehstromsystem
        Pzu = Mathf.Sqrt(3) * Spannung * Iap * cosphi; // Pzu = √3 * U * I * cos(phi)

            // Berechnung des Wirkungsgrads
        Wirkungsgrad = (Pzu != 0) ? (Pab / Pzu) * 100f : 0f;
        if (stromText != null)
        stromText.text = nI.ToString("F2");



// Aktualisieren der Textfelder
if (drehmomentText != null)
    drehmomentText.text = Map.ToString("F2");

if (drehzahlText != null)
    drehzahlText.text = nAP.ToString("F2");

if (bremseText != null)
    bremseText.text = drehmomentBremse.ToString("F1");

if (stromText != null)
    stromText.text = nI.ToString("F2");

if (drehmoment2Text != null)
    drehmoment2Text.text = Map.ToString("F2");

if (drehzahl2Text != null)
    drehzahl2Text.text = nAP.ToString("F2");

        
    }
}
