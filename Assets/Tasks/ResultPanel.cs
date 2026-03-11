using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [Header("UI Referenzen")]
    public GameObject panel;
    public Transform contentParent;
    public GameObject resultRowPrefab;
    public Button closeButton;

    [Header("Farben")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    void Start()
    {
        panel.SetActive(false);
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void ShowResults()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (TaskManager.SocketTask task in TaskManager.Instance.tasks)
        {
            GameObject row = Instantiate(resultRowPrefab, contentParent);
            TextMeshProUGUI text = row.GetComponentInChildren<TextMeshProUGUI>();

            string current = task.socket.isOccupied ? task.socket.currentPlug : "leer";
            bool correct = IsTaskCorrect(task);

            if (correct)
            {
                text.text = "[OK] " + task.socket.socketName + ": " + current;
                text.color = correctColor;
            }
            else
            {
                text.text = "[FAIL] " + task.socket.socketName + ": " + current + "  (soll: " + task.correctCableName + ")";
                text.color = wrongColor;
            }
        }

        panel.SetActive(true);
        PlayerFreeze.Instance.Freeze();
    }

    bool IsTaskCorrect(TaskManager.SocketTask task)
    {
        if (!task.socket.isOccupied)
            return false;

        string plugLabel = task.socket.currentPlug;
        Cable cable = CableManager.Instance.GetCableByLabel(plugLabel);

        if (cable == null)
            return false;

        return cable.cableName == task.correctCableName;
    }

    void ClosePanel()
    {
        panel.SetActive(false);
        PlayerFreeze.Instance.Unfreeze();
    }
}
