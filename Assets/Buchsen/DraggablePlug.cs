using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePlug : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private string plugLabel;
    private Color plugColor;

    public void Setup(string label, Color color, Canvas canvas)
    {
        this.plugLabel = label;
        this.plugColor = color;
        this.canvas = canvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Drag an persistenten Manager übergeben
        PlugDragManager.Instance.StartDrag(plugLabel, plugColor, eventData.position);

        // Panel schließen - jetzt egal, Manager lebt weiter
        PlugInventoryUI.Instance.ClosePanel();
    }

    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }

    public string GetPlugLabel() => plugLabel;
}
