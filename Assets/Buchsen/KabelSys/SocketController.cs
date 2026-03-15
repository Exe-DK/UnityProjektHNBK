using UnityEngine;
using TMPro;

public enum SocketCategory
{
    Einspeisung,
    Abgang,
    Steuerung
}

public class SocketController : MonoBehaviour
{
    [Header("Buchsen-Einstellungen")]
    public string socketName = "X1";
    public SocketCategory socketCategory = SocketCategory.Einspeisung;

    [Header("3D Objekte")]
    public GameObject highlightRing;
    public GameObject plugVisual;
    public TextMeshPro plugLabel;
    public Renderer plugRenderer;

    [Header("Interaktion")]
    public float interaktionsDistanz = 2f;

    // --- Kabel-System Daten ---
    private CableConnection2 currentConnection = null;
    private CableTypeSO connectedCableType = null;

    private Camera playerCam;

    void Start()
    {
        playerCam = Camera.main;
        if (highlightRing != null) highlightRing.SetActive(false);
        if (plugVisual != null) plugVisual.SetActive(false);
    }

    void Update()
    {
        if (PlayerFreeze.Instance != null && PlayerFreeze.Instance.IsFrozen) return;

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, interaktionsDistanz);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == this.transform || hit.transform.IsChildOf(this.transform))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (highlightRing != null) highlightRing.SetActive(true);

                    if (CableManager2.Instance != null)
                    {
                        CableManager2.Instance.OnSocketClicked(this);
                    }
                }
                break;
            }
        }
    }

    // ===== Kabel-System Methoden =====

    public bool HasCableConnection()
    {
        return currentConnection != null;
    }

    public void SetCableConnection(CableConnection2 connection, CableTypeSO type)
    {
        currentConnection = connection;
        connectedCableType = type;

        if (plugRenderer != null)
            plugRenderer.material.color = type.cableColor;

        if (plugLabel != null)
            plugLabel.text = type.cableName;

        if (plugVisual != null)
            plugVisual.SetActive(true);

        if (highlightRing != null)
            highlightRing.SetActive(false);
    }

    public CableConnection2 GetCableConnection()
    {
        return currentConnection;
    }

    public void ClearCableConnection()
    {
        currentConnection = null;
        connectedCableType = null;

        if (plugVisual != null)
            plugVisual.SetActive(false);

        if (plugLabel != null)
            plugLabel.text = "";

        if (highlightRing != null)
            highlightRing.SetActive(false);
    }
}
