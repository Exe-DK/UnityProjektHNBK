using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlugInventoryUI : MonoBehaviour
{
    public static PlugInventoryUI Instance;

    [Header("UI Referenzen")]
    public GameObject panel;                  // Das ganze Panel
    public Transform buttonContainer;         // Parent für die Buttons
    public GameObject buttonPrefab;           // Button Prefab
    public TextMeshProUGUI headerText;        // "Was wollen Sie der Buchse zuweisen?"
    public Button closeButton;                // Schließen-Button

    private SocketManager currentSocket;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void OpenForSocket(SocketManager socket)
    {
        currentSocket = socket;
        headerText.text = "Buchse: " + socket.socketName + "\nWas wollen Sie zuweisen?";

        // Alte Buttons löschen
        foreach (GameObject go in spawnedButtons)
            Destroy(go);
        spawnedButtons.Clear();

        // Buttons für alle verfügbaren Stecker erstellen
        foreach (Cable cable in CableManager.Instance.cables)
        {
            // A-Seite Button
            if (!cable.aUsed)
            {
                CreateButton(cable.labelA, cable.cableColor);
            }

            // B-Seite Button
            if (!cable.bUsed)
            {
                CreateButton(cable.labelB, cable.cableColor);
            }
        }

        panel.SetActive(true);
    }

    void CreateButton(string label, Color color)
    {
        GameObject btn = Instantiate(buttonPrefab, buttonContainer);
        spawnedButtons.Add(btn);

        // Text setzen
        TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
            btnText.text = label;

        // Farbe setzen
        Image btnImage = btn.GetComponent<Image>();
        if (btnImage != null)
            btnImage.color = color;

        // Textfarbe anpassen (dunkle Hintergründe → weißer Text)
        if (btnText != null)
        {
            float brightness = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
            btnText.color = brightness < 0.5f ? Color.white : Color.black;
        }

        // Click Event
        Button buttonComp = btn.GetComponent<Button>();
        string plugLabel = label; // Closure-safe
        buttonComp.onClick.AddListener(() => OnPlugSelected(plugLabel));
    }

    void OnPlugSelected(string label)
    {
        if (currentSocket != null)
        {
            // Falls schon ein Stecker drin ist, erst entfernen
            if (currentSocket.isOccupied)
                currentSocket.RemovePlug();

            currentSocket.InsertPlug(label);
        }

        ClosePanel();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        currentSocket = null;
        PlayerFreeze.Instance.Unfreeze();
    }
}
