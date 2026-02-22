using UnityEngine;
using TMPro; // Für TextMeshPro

public class DisplayContent : MonoBehaviour
{
    // Verweise auf die TextMeshPro-Felder
    public TextMeshProUGUI Uberschrift;
    public TextMeshProUGUI Seitenzahl;
    public TextMeshProUGUI Daten1;
    public TextMeshProUGUI Daten2;
    public TextMeshProUGUI Daten3;

    private int currentPage = 0; // Startseite

    // Inhalte für die Seiten
    private string[,] pages = new string[3, 5]
    {
        { "Seite 1 - Überschrift", "Seite 1 - Seitenzahl", "Seite 1 - Daten1", "Seite 1 - Daten2", "Seite 1 - Daten3" },
        { "Seite 2 - Überschrift", "Seite 2 - Seitenzahl", "Seite 2 - Daten1", "Seite 2 - Daten2", "Seite 2 - Daten3" },
        { "Seite 3 - Überschrift", "Seite 3 - Seitenzahl", "Seite 3 - Daten1", "Seite 3 - Daten2", "Seite 3 - Daten3" }
    };

    void Start()
    {
        UpdateDisplay(); // Initialisiert die Inhalte der ersten Seite
    }

    void Update()
    {
        // Seitenwechsel mit Pfeiltasten
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
    }

    void NextPage()
    {
        currentPage = (currentPage + 1) % pages.GetLength(0); // Nächste Seite
        UpdateDisplay();
    }

    void PreviousPage()
    {
        currentPage = (currentPage - 1 + pages.GetLength(0)) % pages.GetLength(0); // Vorherige Seite
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // Aktualisiere die Inhalte der TextMeshPro-Felder
        Uberschrift.text = pages[currentPage, 0];
        Seitenzahl.text = pages[currentPage, 1];
        Daten1.text = pages[currentPage, 2];
        Daten2.text = pages[currentPage, 3];
        Daten3.text = pages[currentPage, 4];
    }
}

