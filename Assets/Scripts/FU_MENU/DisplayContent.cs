using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Hardware;

public class DisplayContent : MonoBehaviour
{
    public TextMeshProUGUI Uberschrift;
    public TextMeshProUGUI Seitenzahl;
    public TextMeshProUGUI Daten1;
    public TextMeshProUGUI Daten2;
    public TextMeshProUGUI Daten3;
    public TextMeshProUGUI Daten4;
    public TextMeshProUGUI Daten5;
    public TextMeshProUGUI Daten6;
    public TextMeshProUGUI FU_Menu_text;
    public TextMeshProUGUI Skala25;
    public TextMeshProUGUI Skala50;

    public GameObject pwmDiagramm;
    public GameObject sinusDiagrammPWM;

    public int currentPage = 0;
    public LineRenderer[] lineRenderers;

    private string[,] pages = new string[7, 8]
    {
        {"Steuerung","1/7" ,"q und e oder Mausrad:       Frequenz steuern", "Pfeiltasten <- -> :          Seiten wechseln", "F: Ansicht verlassen", "", "","" },
        {"Netzdaten","2/7", "", "Netzfrequenz f<sub>0</sub> = 50Hz", "", "Netzspannung U<sub>0</sub> = 400V", "",""},
        {"Bemessungsdaten des Motors ","3/7","Spannung U<sub>n</sub> = 400V ∆  ","Drehzahl n<sub>n</sub> = 2800 <sub>1/min</sub>" ,"Leistung P<sub>ab</sub> = 0,37 kW","Strom I<sub>n</sub> = 1A" ,"Drehmoment M<sub>n</sub> = 1,3 Nm" ,""},
        { "Aktuelle Motorwerte","4/7","", "", "", "", "","" },
        {"Frequenzumrichter-Daten","5/7","", "", "", "", "","" },
        {"PWM L1", "6/7", "", "", "", "", "" ,""},
        {"I<sub>N</sub>/M<sub>N</sub> Kennlinie", "7/7", "           I<sub>N</sub>/M<sub>N</sub>", "", "                                                                      n", "", "","" }
    };

    private Dictionary<TextMeshProUGUI, float> originalFontSizes = new Dictionary<TextMeshProUGUI, float>();
    private Dictionary<TextMeshProUGUI, Vector3> originalPositions = new Dictionary<TextMeshProUGUI, Vector3>();
    private Dictionary<TextMeshProUGUI, Vector3> originalScales = new Dictionary<TextMeshProUGUI, Vector3>();

    void Start()
    {
        SaveOriginalValues();
        UpdateDisplay();
    }

    void SaveOriginalValues()
    {
        TextMeshProUGUI[] texts = { Uberschrift, Seitenzahl, Daten1, Daten2, Daten3, Daten4, Daten5, Daten6, FU_Menu_text, Skala25, Skala50 };

        foreach (TextMeshProUGUI text in texts)
        {
            if (text != null)
            {
                originalFontSizes[text] = text.fontSize;
                originalPositions[text] = text.rectTransform.localPosition;
                originalScales[text] = text.rectTransform.localScale;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }

        if (currentPage == 3 || currentPage == 4)
        {
            UpdateLiveValues();
        }
    }

    public void NextPage()
    {
        currentPage = (currentPage + 1) % pages.GetLength(0);
        UpdateDisplay();
        StartCoroutine(PageTransitionEffect());
    }

    public void PreviousPage()
    {
        currentPage = (currentPage - 1 + pages.GetLength(0)) % pages.GetLength(0);
        UpdateDisplay();
        StartCoroutine(PageTransitionEffect());
    }

    IEnumerator PageTransitionEffect()
    {
        float duration = 0.15f;
        float jumpHeight = 10f;
        float scaleAmount = 1.05f;

        TextMeshProUGUI[] texts = { Uberschrift, Seitenzahl, Daten1, Daten2, Daten3, Daten4, Daten5, Daten6, FU_Menu_text, Skala25, Skala50 };

        foreach (TextMeshProUGUI text in texts)
        {
            if (text != null)
            {
                text.rectTransform.localPosition += new Vector3(0, jumpHeight, 0);
                text.rectTransform.localScale *= scaleAmount;
                text.fontSize *= 1.1f;
            }
        }
        yield return new WaitForSeconds(duration / 2);

        foreach (TextMeshProUGUI text in texts)
        {
            if (text != null)
            {
                text.rectTransform.localPosition -= new Vector3(0, jumpHeight * 1.5f, 0);
                text.rectTransform.localScale /= scaleAmount;
                text.fontSize *= 0.9f;
            }
        }
        yield return new WaitForSeconds(duration / 2);

        foreach (TextMeshProUGUI text in texts)
        {
            if (text != null && originalPositions.ContainsKey(text) && originalFontSizes.ContainsKey(text) && originalScales.ContainsKey(text))
            {
                text.rectTransform.localPosition = originalPositions[text];
                text.rectTransform.localScale = originalScales[text];
                text.fontSize = originalFontSizes[text];
            }
        }
    }

    void UpdateLiveValues()
    {
        Berechnung berechnung = FindObjectOfType<Berechnung>();
        if (berechnung != null)
        {
            if (currentPage == 3)
            {
                Daten1.text = "Drehzahl n: " + berechnung.DrehzahlAP.ToString("F2") + " 1/min";
                Daten4.text = "Drehmoment M: " + berechnung.DrehmomentAP.ToString("F2") + " Nm";
                Daten2.text = berechnung.synchronFrequenz != 0 ? "Schlupf : " + (berechnung.SchlupfAP * 100).ToString("F2") + "%" : "Schlupf : -";
                Daten5.text = "P<sub>ab</sub>: " + berechnung.Pab.ToString("F2") + " W";
                Daten3.text = "P<sub>zu</sub>: " + berechnung.Pzu.ToString("F2") + " W";
                Daten6.text = "Wirkungsgrad: " + berechnung.Wirkungsgrad.ToString("F2") + " %";
            }
            else if (currentPage == 4)
            {
                Daten1.text = "Spannung U: " + berechnung.Spannung.ToString("F2") + "V";
                Daten4.text = "Frequenz f: " + berechnung.synchronFrequenz.ToString("F2") + "Hz";
                Daten2.text = "Strom I : " + berechnung.Iap.ToString("F2") +"A";
                Daten5.text = "Zwischenkreisspannung U<sub>z</sub>: 537,5V";
                Daten3.text = "";
                Daten6.text = " ";
            }
        }
    }

    void UpdateDisplay()
    {
        Uberschrift.text = pages[currentPage, 0];
        Seitenzahl.text = pages[currentPage, 1];

        if (currentPage != 3 && currentPage != 4)
        {
            Daten1.text = pages[currentPage, 2];
            Daten2.text = pages[currentPage, 3];
            Daten3.text = pages[currentPage, 4];
            Daten4.text = pages[currentPage, 5];
            Daten5.text = pages[currentPage, 6];
            Daten6.text = pages[currentPage, 7];
        }

        if (currentPage == 5)
        {
            pwmDiagramm.SetActive(true);
            sinusDiagrammPWM.SetActive(true);
            Skala25.text = "20ms";
            Skala50.text = "10ms";
        }
        else
        {
            pwmDiagramm.SetActive(false);
            sinusDiagrammPWM.SetActive(false);
            Skala25.text = "";
            Skala50.text = "";
        }

        foreach (var lr in lineRenderers)
        {
            if (lr != null)
            {
                lr.enabled = (currentPage == 6);
            }
        }
    }
}
