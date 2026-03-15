using UnityEngine;
using System.Collections.Generic;

public class CableManager2 : MonoBehaviour
{
    public static CableManager2 Instance { get; private set; }

    private enum CableState { Idle, FirstSocketSelected, WaitingForCableSelection }

    [Header("Kabeltypen")]
    [SerializeField] private CableTypeSO[] availableCableTypes;

    [Header("UI")]
    [SerializeField] private CableSelectionUI cableSelectionUI;

    [Header("Vorschau")]
    [SerializeField] private LineRenderer previewLine;

    [Header("Kabel-Visuals")]
    [SerializeField] private Material cableLineMaterial;
    [SerializeField] private float cableLineWidth = 0.02f;

    private CableState currentState = CableState.Idle;
    private SocketController firstSocket;
    private SocketController secondSocket;
    private CableTypeSO selectedCableType;

    private List<CableConnection2> allConnections = new List<CableConnection2>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (currentState == CableState.FirstSocketSelected && previewLine != null)
        {
            previewLine.enabled = true;
            previewLine.SetPosition(0, firstSocket.transform.position);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 endPoint = ray.GetPoint(2f);
            previewLine.SetPosition(1, endPoint);
        }
        else if (previewLine != null)
        {
            previewLine.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && currentState != CableState.Idle)
        {
            CancelAction();
        }
    }

    public void OnSocketClicked(SocketController socket)
    {
        if (currentState == CableState.Idle)
        {
            if (socket.HasCableConnection())
            {
                RemoveConnectionFromSocket(socket);
                return;
            }

            firstSocket = socket;
            currentState = CableState.WaitingForCableSelection;
            PlayerFreeze.Instance.Freeze();
            ShowCableSelectionUI(socket);
        }
        else if (currentState == CableState.FirstSocketSelected)
        {
            if (socket == firstSocket)
            {
                Debug.LogWarning("Kann nicht mit sich selbst verbinden!");
                return;
            }

            if (socket.HasCableConnection())
            {
                Debug.LogWarning("Diese Buchse ist bereits belegt!");
                return;
            }

            secondSocket = socket;
            CreateConnection();
        }
    }

    private void ShowCableSelectionUI(SocketController socket)
    {
        if (cableSelectionUI != null)
        {
            cableSelectionUI.OpenPanel();
        }
        else
        {
            Debug.LogError("CableSelectionUI ist nicht zugewiesen!");
        }
    }

    public void OnCableTypeSelected(CableTypeSO cableType)
    {
        if (currentState != CableState.WaitingForCableSelection) return;

        selectedCableType = cableType;
        currentState = CableState.FirstSocketSelected;
        PlayerFreeze.Instance.Unfreeze();

        Debug.Log($"Kabel '{cableType.cableName}' gewählt. Jetzt zweite Buchse anklicken.");
    }

    private void CreateConnection()
    {
        GameObject lineObj = new GameObject($"Cable_{selectedCableType.cableName}_{firstSocket.socketName}_{secondSocket.socketName}");
        lineObj.transform.SetParent(this.transform);

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, firstSocket.transform.position);
        lr.SetPosition(1, secondSocket.transform.position);
        lr.startWidth = cableLineWidth;
        lr.endWidth = cableLineWidth;

        if (cableLineMaterial != null)
        {
            lr.material = cableLineMaterial;
        }

        lr.startColor = selectedCableType.cableColor;
        lr.endColor = selectedCableType.cableColor;

        CableConnection2 connection = new CableConnection2(firstSocket, secondSocket, selectedCableType, lr);

        allConnections.Add(connection);

        firstSocket.SetCableConnection(connection, selectedCableType);
        secondSocket.SetCableConnection(connection, selectedCableType);

        Debug.Log($"Verbindung erstellt: {firstSocket.socketName} <-> {secondSocket.socketName} mit {selectedCableType.cableName}");

        ResetState();
    }

    private void RemoveConnectionFromSocket(SocketController socket)
    {
        CableConnection2 conn = null;

        foreach (CableConnection2 c in allConnections)
        {
            if (c.socketA == socket || c.socketB == socket)
            {
                conn = c;
                break;
            }
        }

        if (conn == null) return;

        conn.socketA.ClearCableConnection();
        conn.socketB.ClearCableConnection();

        if (conn.lineRenderer != null)
        {
            Destroy(conn.lineRenderer.gameObject);
        }

        allConnections.Remove(conn);

        Debug.Log($"Verbindung entfernt: {conn.socketA.socketName} <-> {conn.socketB.socketName}");
    }

    public void CancelAction()
    {
        Debug.Log("Aktion abgebrochen.");
        PlayerFreeze.Instance.Unfreeze();
        ResetState();
    }

    private void ResetState()
    {
        currentState = CableState.Idle;
        firstSocket = null;
        secondSocket = null;
        selectedCableType = null;

        if (previewLine != null)
        {
            previewLine.enabled = false;
        }
    }

    // ===== Getter für externe Systeme =====

    public List<CableConnection2> GetAllConnections()
    {
        return new List<CableConnection2>(allConnections);
    }

    public CableTypeSO[] GetAvailableCableTypes()
    {
        return availableCableTypes;
    }

    public bool IsCableTypeUsed(CableTypeSO type)
    {
        foreach (CableConnection2 conn in allConnections)
        {
            if (conn.cableType == type) return true;
        }
        return false;
    }

    public void ClearAllConnections()
    {
        for (int i = allConnections.Count - 1; i >= 0; i--)
        {
            CableConnection2 conn = allConnections[i];
            conn.socketA.ClearCableConnection();
            conn.socketB.ClearCableConnection();

            if (conn.lineRenderer != null)
            {
                Destroy(conn.lineRenderer.gameObject);
            }
        }
        allConnections.Clear();
        Debug.Log("Alle Verbindungen entfernt.");
    }

    // ===== Validierung =====

    [System.Serializable]
    public class RequiredConnection
    {
        public SocketController socketA;
        public SocketController socketB;
        public CableTypeSO requiredType;
    }

    [Header("Validierung (Optional)")]
    [SerializeField] private List<RequiredConnection> requiredConnections = new List<RequiredConnection>();

    public bool ValidateConnections()
    {
        List<CableConnection2> actual = GetAllConnections();
        int correctCount = 0;

        foreach (RequiredConnection req in requiredConnections)
        {
            bool found = false;

            foreach (CableConnection2 conn in actual)
            {
                bool matchForward = (conn.socketA == req.socketA && conn.socketB == req.socketB);
                bool matchReverse = (conn.socketA == req.socketB && conn.socketB == req.socketA);

                if ((matchForward || matchReverse) && conn.cableType == req.requiredType)
                {
                    found = true;
                    correctCount++;
                    break;
                }
            }

            if (found)
            {
                Debug.Log($"✓ {req.socketA.socketName} <-> {req.socketB.socketName} ({req.requiredType.cableName}) korrekt");
            }
            else
            {
                Debug.Log($"✗ {req.socketA.socketName} <-> {req.socketB.socketName} ({req.requiredType.cableName}) fehlt oder falsch");
            }
        }

        bool allCorrect = correctCount == requiredConnections.Count && actual.Count == requiredConnections.Count;

        if (allCorrect)
        {
            Debug.Log("=== ALLES RICHTIG! ===");
        }
        else
        {
            Debug.Log($"=== {correctCount}/{requiredConnections.Count} korrekt, {actual.Count} Verbindungen gesetzt ===");
        }

        return allCorrect;
    }
}
