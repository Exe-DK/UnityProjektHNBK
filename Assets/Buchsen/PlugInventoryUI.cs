using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlugInventoryUI : MonoBehaviour
{
    public static PlugInventoryUI Instance;

    [Header("UI Referenzen")]
    public GameObject panel;
    public Transform buttonContainer;
    public GameObject buttonPrefab;
    public TextMeshProUGUI headerText;
    public Button closeButton;

    private SocketManager currentSocket;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        closeButton.onClick.AddListener(() =>
        {
            ClosePanelAndUnfreeze();
        });
    }

    void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanelAndUnfreeze();
        }
    }

    public void OpenForSocket(SocketManager socket)
    {
        currentSocket = socket;
        headerText.text = "Buchse: " + socket.socketName + "\nWas wollen Sie zuweisen?";

        foreach (GameObject go in spawnedButtons)
            Destroy(go);
        spawnedButtons.Clear();

        if (socket.isOccupied)
        {
            GameObject removeBtn = Instantiate(buttonPrefab, buttonContainer);
            spawnedButtons.Add(removeBtn);

            TextMeshProUGUI removeText = removeBtn.GetComponentInChildren<TextMeshProUGUI>();
            if (removeText != null)
                removeText.text = "✖ Entfernen";

            Image removeImage = removeBtn.GetComponent<Image>();
            if (removeImage != null)
                removeImage.color = new Color(0.8f, 0.2f, 0.2f);

            if (removeText != null)
                removeText.color = Color.white;

            Button removeBtnComp = removeBtn.GetComponent<Button>();
            removeBtnComp.onClick.AddListener(() =>
            {
                currentSocket.RemovePlug();
                ClosePanelAndUnfreeze();
            });
        }

        foreach (Cable cable in CableManager.Instance.cables)
        {
            if (!cable.aUsed)
                CreateButton(cable.labelA, cable.cableColor);

            if (!cable.bUsed)
                CreateButton(cable.labelB, cable.cableColor);
        }

        panel.SetActive(true);
    }

    void CreateButton(string label, Color color)
    {
        GameObject btn = Instantiate(buttonPrefab, buttonContainer);
        spawnedButtons.Add(btn);

        TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
            btnText.text = label;

        Image btnImage = btn.GetComponent<Image>();
        if (btnImage != null)
            btnImage.color = color;

        if (btnText != null)
        {
            float brightness = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
            btnText.color = brightness < 0.5f ? Color.white : Color.black;
        }

        Button buttonComp = btn.GetComponent<Button>();
        string plugLabel = label;
        buttonComp.onClick.AddListener(() => OnPlugSelected(plugLabel));

        Canvas canvas = panel.GetComponentInParent<Canvas>();
        var draggable = btn.AddComponent<DraggablePlug>();
        draggable.Setup(label, color, canvas);
    }

    void OnPlugSelected(string label)
    {
        if (currentSocket != null)
        {
            if (currentSocket.isOccupied)
                currentSocket.RemovePlug();

            currentSocket.InsertPlug(label);
        }

        ClosePanelAndUnfreeze();
    }

    /// <summary>
    /// Schließt das Panel UND gibt den Spieler immer frei.
    /// </summary>
    public void ClosePanelAndUnfreeze()
    {
        panel.SetActive(false);

        if (currentSocket != null)
            currentSocket.highlightRing.SetActive(false);

        currentSocket = null;
        PlayerFreeze.Instance.Unfreeze();
    }

    /// <summary>
    /// Nur Panel schließen ohne Unfreeze (falls extern gebraucht).
    /// </summary>
    public void ClosePanel()
    {
        panel.SetActive(false);

        if (currentSocket != null)
            currentSocket.highlightRing.SetActive(false);

        currentSocket = null;
    }
}
