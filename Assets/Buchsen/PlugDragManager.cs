using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlugDragManager : MonoBehaviour
{
    public static PlugDragManager Instance;
    
    private GameObject dragIcon;
    private string plugLabel;
    private bool isDragging;
    private Canvas canvas;

    void Awake()
    {
        Instance = this;
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null) canvas = FindFirstObjectByType<Canvas>();
    }

    public void StartDrag(string label, Color color, Vector2 screenPos)
    {
        if (isDragging) return;
        isDragging = true;
        plugLabel = label;

        // Icon erstellen
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);

        var img = dragIcon.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = false;

        var rt = dragIcon.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(60, 60);

        var textObj = new GameObject("Label");
        textObj.transform.SetParent(dragIcon.transform, false);
        var tmp = textObj.AddComponent<TextMeshProUGUI>();
        string displayName = label.Contains("_")
            ? label.Substring(label.IndexOf("_") + 1)
            : label;
        tmp.text = displayName;
        tmp.fontSize = 20;
        tmp.alignment = TextAlignmentOptions.Center;
        float brightness = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
        tmp.color = brightness < 0.5f ? Color.white : Color.black;
        var textRt = textObj.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        dragIcon.transform.position = screenPos;
    }

    void Update()
    {
        if (!isDragging) return;

        // Icon folgt Maus
        if (dragIcon != null)
            dragIcon.transform.position = Input.mousePosition;

        // Loslassen
        if (Input.GetMouseButtonUp(0))
        {
            FinishDrop();
        }
    }

    private void FinishDrop()
{
    isDragging = false;

    if (dragIcon != null)
    {
        Destroy(dragIcon);
        dragIcon = null;
    }

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

    // Sortiere nach Distanz
    System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

    foreach (RaycastHit hit in hits)
    {
        SocketManager socket = hit.transform.GetComponent<SocketManager>();
        if (socket == null)
            socket = hit.transform.GetComponentInParent<SocketManager>();

        if (socket != null)
        {
            if (socket.isOccupied)
                socket.RemovePlug();
            socket.InsertPlug(plugLabel);
            break;
        }
    }

    PlayerFreeze.Instance.Unfreeze();
}

}
