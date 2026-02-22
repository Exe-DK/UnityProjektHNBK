using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Verwende TextMeshPro-Namespace

public class HUDAnzeige : MonoBehaviour
{
   


    public TextMeshProUGUI drehmomentHUDText;   // Hier wird die Textkomponente für Drehmoment im HUD referenziert
    public TextMeshProUGUI drehzahlHUDText;   // Hier wird die Textkomponente für Drehzahl im HUD referenziert
    public TextMeshProUGUI stromHUDText;   // Hier wird die Textkomponente für Strom im HUD referenziert

    void Update()
    {
     

        float Drehmoment = Berechnung.Map;
        float Umdrehungen = Berechnung.nAP;
        float Strom = Berechnung.nI;

      
        // Aktualisiere die Textobjekte im HUD mit den Werten aus dem Berechnungsskript
        drehmomentHUDText.text = "" + Drehmoment.ToString("F2");
        drehzahlHUDText.text = "" + Umdrehungen.ToString();
        stromHUDText.text = "" + Strom.ToString("F2");
    }
}