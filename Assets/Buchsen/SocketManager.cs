using UnityEngine;
using TMPro;

public class SocketManager : MonoBehaviour
{
    [Header("Buchsen-Einstellungen")]
    public string socketName = "U1";

    [Header("3D Objekte")]
    public GameObject highlightRing;
    public GameObject plugVisual;
    public TextMeshPro plugLabel;
    public Renderer plugRenderer;

    [Header("Interaktion")]
    public float interaktionsDistanz = 2f;

    [HideInInspector] public string currentPlug = "";
    [HideInInspector] public bool isOccupied = false;

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

    Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
    RaycastHit[] hits = Physics.RaycastAll(ray, interaktionsDistanz);

    // Nach Distanz sortieren
    System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

    foreach (RaycastHit hit in hits)
    {
        if (hit.transform == this.transform || hit.transform.IsChildOf(this.transform))
        {
            if (Input.GetMouseButtonDown(0))
            {
                highlightRing.SetActive(true);
                OpenPanel();
            }

            if (Input.GetMouseButtonDown(1) && isOccupied)
            {
                RemovePlug();
            }
            break;
        }
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

        Color col = CableManager.Instance.GetColorForLabel(label);
        plugRenderer.material.color = col;

        string displayName = label;
        if (label.Contains("_"))
            displayName = label.Substring(label.IndexOf("_") + 1);
        plugLabel.text = displayName;

        plugVisual.SetActive(true);
        highlightRing.SetActive(false);
    }

    public void RemovePlug()
    {
        CableManager.Instance.SetPlugFree(currentPlug);
        currentPlug = "";
        isOccupied = false;
        plugVisual.SetActive(false);
        plugLabel.text = "";
        highlightRing.SetActive(false);
    }
}
