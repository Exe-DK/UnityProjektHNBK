using UnityEngine;
using TMPro;

public class Buchse_Click : MonoBehaviour
{
    public Renderer markierungsRenderer;
    public BuchsenManager manager;
    public TMP_Text textAnzeige;

    void Start()
    {
        if (markierungsRenderer != null)
            markierungsRenderer.enabled = false;
    }

    void OnMouseDown()
    {
        Debug.Log("Buchse angeklickt: " + gameObject.name);

        if (manager != null)
            manager.BuchseGeklickt(this);
        else
            Debug.Log("Manager ist NULL!");
    }

    public void SetMarkierung(bool an)
    {
        if (markierungsRenderer != null)
            markierungsRenderer.enabled = an;
    }

    public void SetText(string wert)
    {
        Debug.Log("SetText wird aufgerufen für: " + gameObject.name);

        if (textAnzeige == null)
        {
            Debug.Log("TEXTANZEIGE IST NULL!");
            return;
        }

        textAnzeige.text = wert;
        Debug.Log("Text gesetzt auf: " + wert);
    }
}