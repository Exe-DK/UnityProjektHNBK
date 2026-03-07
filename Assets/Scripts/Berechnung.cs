using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Berechnung : MonoBehaviour
{
    [Header("UI Texte")]
    public TextMeshProUGUI drehmomentText;
    public TextMeshProUGUI drehzahlText;
    public TextMeshProUGUI bremseText;
    public TextMeshProUGUI stromText;
    public TextMeshProUGUI drehmoment2Text;
    public TextMeshProUGUI drehzahl2Text;

    [Header("Drehmomentkurve")]
    public LineRenderer drehmomentenKurvenRenderer;

    // Statische Ausgabewerte für andere Skripte
    public static float nAP;
    public static float Map;
    public static float nI;

    [Header("Motorkonstanten")]
    public float Netzfrequenz = 50.0f;
    public int AnzahlPole = 2;
    public float RotorWirkwiderstand = 40.9f;
    public float Hauptblindwiderstand = 217.65f;
    public float Statorstreuinduktivität = 0.369f;
    public float Rotorstreuinduktivität = 0.324f;
    public float konstantSpannung = 400.0f;
    public float Alpha = 1.62f;
    public float Nennstrom = 1.0f;
    public float Kippmoment = 3.51f;
    public float Nennmoment = 1.26f;
    public float cosphi = 0.83f;
    public float Nenndrehzahl = 2800f;

    [Header("Berechnete Werte (nur lesen)")]
    public float synchronDrehzahl;
    public float Schlupf;
    public float Kippschlupf;
    public float synchronFrequenz;
    public float Spannung;
    public float Drehmoment;
    public float DrehmomentAP;
    public float SchlupfAP;
    public float DrehzahlAP;
    public float Strom;
    public float Pzu;
    public float Pab;
    public float Wirkungsgrad;
    public float Iap;

    [Header("Referenzen")]
    public RunController runController;

    private bool überlastAusgelöst = false;

    void Start()
    {
        if (drehmomentText == null) Debug.LogError("drehmomentText nicht zugewiesen!");
        if (drehzahlText == null) Debug.LogError("drehzahlText nicht zugewiesen!");
        if (bremseText == null) Debug.LogError("bremseText nicht zugewiesen!");
        if (stromText == null) Debug.LogError("stromText nicht zugewiesen!");
        if (drehmoment2Text == null) Debug.LogError("drehmoment2Text nicht zugewiesen!");
        if (drehzahl2Text == null) Debug.LogError("drehzahl2Text nicht zugewiesen!");
        if (runController == null)
            runController = FindFirstObjectByType<RunController>();
    }

    void Update()
    {
        // ───────────────────────────────────────────
        // Motor aus → prüfen ob Überlast oder normaler Stop
        // ───────────────────────────────────────────
        if (!MotorDrehung.motorLäuft)
        {
            nAP = 0f;
            Map = 0f;
            nI = 0f;

            if (überlastAusgelöst)
            {
                SetzeAlleTexte("Ueberlast!");
            }
            else
            {
                SetzeAlleTexte("Aus");
            }
            return;
        }

        // ───────────────────────────────────────────
        // Motor wurde neu gestartet (Run gedrückt)
        // → Überlast-Flag zurücksetzen
        // ───────────────────────────────────────────
        überlastAusgelöst = false;

        // ───────────────────────────────────────────
        // Werte vom Regler und Bremse holen
        // ───────────────────────────────────────────
        float Frequenz = ReglerDrehung.Frequenz;
        float drehmomentBremse = DrehmomentBremse.AktuellesDrehmomentBremse;

        // Frequenz zu niedrig → Motor steht praktisch still
        if (Frequenz < 0.1f)
        {
            nAP = 0f;
            Map = 0f;
            nI = 0f;

            SetzeAlleTexte("0 Hz");
            return;
        }

        // ───────────────────────────────────────────
        // Berechnungen
        // ───────────────────────────────────────────

        // Synchrondrehzahl
        synchronDrehzahl = (120f * Frequenz) / AnzahlPole;
        synchronFrequenz = synchronDrehzahl / 60f;

        // Spannung nach U/f-Kennlinie (bis 50 Hz linear, darüber konstant 400V)
        Spannung = (Frequenz <= 50f) ? konstantSpannung * (Frequenz / Netzfrequenz) : konstantSpannung;

        // Kippschlupf
        Kippschlupf = RotorWirkwiderstand / Hauptblindwiderstand;

        // Nennschlupf berechnen aus Nenndrehzahl
        float nSync50 = (120f * Netzfrequenz) / AnzahlPole;
        float Nennschlupf = (nSync50 - Nenndrehzahl) / nSync50;

        // Schlupf skaliert mit Frequenz
        Schlupf = Nennschlupf;

        // Drehmoment aus Kloss'scher Formel
        if (Schlupf > 0.0001f)
        {
            Drehmoment = Kippmoment * 2f / ((Schlupf / Kippschlupf) + (Kippschlupf / Schlupf));
        }
        else
        {
            Drehmoment = 0f;
        }

        // Arbeitspunkt-Drehmoment (Nennmoment + Bremse)
        DrehmomentAP = Nennmoment + drehmomentBremse;

        // Arbeitspunkt-Schlupf
        SchlupfAP = Nennschlupf * (DrehmomentAP / Nennmoment);
        SchlupfAP = Mathf.Clamp(SchlupfAP, 0.0001f, 1f);

        // Arbeitspunkt-Drehzahl
        DrehzahlAP = Mathf.Max(0f, (1f - SchlupfAP) * synchronDrehzahl);

        // Strom am Arbeitspunkt
        Iap = Nennstrom * (DrehmomentAP / Nennmoment);

        // ───────────────────────────────────────────
        // Prüfen ob Kippmoment überschritten
        // ───────────────────────────────────────────
        if (DrehmomentAP >= Kippmoment)
        {
            // Überlast! Motor kippt → einmalig Stop auslösen
            if (!überlastAusgelöst)
            {
                überlastAusgelöst = true;
                runController.OnStopButtonPressed();
            }

            nAP = 0f;
            Map = 0f;
            nI = 0f;

            SetzeAlleTexte("Ueberlast!");
            return;
        }

        // ───────────────────────────────────────────
        // Normaler Betrieb - Arbeitspunkt setzen
        // ───────────────────────────────────────────
        nAP = DrehzahlAP;
        Map = DrehmomentAP;

        // Strom aus Ersatzschaltbild
        float impedanz = Mathf.Sqrt(Mathf.Pow(RotorWirkwiderstand / SchlupfAP, 2f) + Mathf.Pow(Hauptblindwiderstand, 2f));
        Strom = Alpha * Spannung / impedanz;
        nI = Strom;

        // Leistungen
        Pab = (2f * Mathf.PI * DrehzahlAP / 60f) * DrehmomentAP;
        Pab = Mathf.Max(0f, Pab);

        Pzu = Mathf.Sqrt(3f) * Spannung * Iap * cosphi;
        Wirkungsgrad = (Pzu > 0.001f) ? (Pab / Pzu) * 100f : 0f;

        // ───────────────────────────────────────────
        // UI aktualisieren
        // ───────────────────────────────────────────
        if (drehmomentText != null) drehmomentText.text = Map.ToString("F2") + " Nm";
        if (drehzahlText != null) drehzahlText.text = nAP.ToString("F0") + " 1/min";
        if (bremseText != null) bremseText.text = drehmomentBremse.ToString("F1") + " Nm";
        if (stromText != null) stromText.text = nI.ToString("F2") + " A";
        if (drehmoment2Text != null) drehmoment2Text.text = Map.ToString("F2") + " Nm";
        if (drehzahl2Text != null) drehzahl2Text.text = nAP.ToString("F0") + " 1/min";
    }

    // ───────────────────────────────────────────
    // Hilfsfunktion: Alle Texte auf einen Wert setzen
    // ───────────────────────────────────────────
    private void SetzeAlleTexte(string text)
    {
        if (drehmomentText != null) drehmomentText.text = text;
        if (drehzahlText != null) drehzahlText.text = text;
        if (bremseText != null) bremseText.text = text;
        if (stromText != null) stromText.text = text;
        if (drehmoment2Text != null) drehmoment2Text.text = text;
        if (drehzahl2Text != null) drehzahl2Text.text = text;
    }
}
