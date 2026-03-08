using UnityEngine;
using TMPro;

public class SocketManager : MonoBehaviour
{
    [Header("Buchsen-Einstellungen")]
    public string socketName = "U1";        // Name der Buchse

    [Header("3D Objekte")]
    public GameObject highlightRing;         // Gelber Ring
    public GameObject plugVisual;            // Der Stecker-Kreis (3D Objekt)
    public TextMeshPro plugLabel;            // Text auf dem Stecker (3D TextMeshPro)
    public Renderer plugRenderer;            // Renderer für Farbwechsel

    [Header("Interaktion")]
    public float interaktionsDistanz = 2f;

    // Status
    [HideInInspector] public string currentPlug = "";  // z.B. "A_L1"
    [HideInInspector] public bool isOccupied = false;

    private bool isHighlighted = false;
    private Camera playerCam;

    void Start()
    {
        playerCam = Camera.main;
        highlightRing.SetActive(false);
        plugVisual.SetActive(false);
    }

    void Update()
    {
        if (PlayerFreeze.Instance.IsFrozen) return;

        // Raycast vom Fadenkreuz
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interaktionsDistanz))
        {
            if (hit.transform == this.transform || hit.transform.IsChildOf(this.transform))
            {
                if (!isHighlighted)
                {
                    highlightRing.SetActive(true);
                    isHighlighted = true;
                }

                // Linksklick → Panel öffnen
                if (Input.GetMouseButtonDown(0))
                {
                    OpenPanel();
                }

                // Rechtsklick → Stecker entfernen
                if (Input.GetMouseButtonDown(1) && isOccupied)
                {
                    RemovePlug();
                }
            }
            else
            {
                HideHighlight();
            }
        }
        else
        {
            HideHighlight();
        }
    }

    void HideHighlight()
    {
        if (isHighlighted)
        {
            highlightRing.SetActive(false);
            isHighlighted = false;
        }
    }

    void OpenPanel()
    {
        PlayerFreeze.Instance.Freeze();
        PlugInventoryUI.Instance.OpenForSocket(this);
    }

    public void InsertPlug(string label)
    {
        currentPlug = label;
        isOccupied = true;

        // Farbe setzen
        Color col = CableManager.Instance.GetColorForLabel(label);
        plugRenderer.material.color = col;

        // Label setzen (nur den Teil nach _ anzeigen, z.B. "L1")
        string displayName = label;
        if (label.Contains("_"))
            displayName = label.Substring(label.IndexOf("_") + 1);
        plugLabel.text = displayName;

        // Stecker sichtbar machen
        plugVisual.SetActive(true);
        highlightRing.SetActive(false);

        // Im CableManager registrieren
        CableManager.Instance.SetPlugUsed(label, this);
    }

    public void RemovePlug()
    {
        CableManager.Instance.SetPlugFree(currentPlug);
        currentPlug = "";
        isOccupied = false;
        plugVisual.SetActive(false);
        plugLabel.text = "";
    }
}
