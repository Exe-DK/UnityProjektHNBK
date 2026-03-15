using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CableSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject cableSelectionPanel;
    public Transform contentParent;
    public GameObject cableButtonPrefab;
    public Button cancelButton;

    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Start()
    {
        cancelButton.onClick.AddListener(OnCancelClicked);
        cableSelectionPanel.SetActive(false);
    }

    public void OpenPanel()
    {
        foreach (var btn in spawnedButtons)
            Destroy(btn);
        spawnedButtons.Clear();

        CableTypeSO[] cables = CableManager2.Instance.GetAvailableCableTypes();

        foreach (CableTypeSO cable in cables)
        {
            GameObject btnObj = Instantiate(cableButtonPrefab, contentParent);
            spawnedButtons.Add(btnObj);

            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
                btnText.text = cable.cableName;

            Image btnImage = btnObj.GetComponent<Image>();
            if (btnImage != null)
                btnImage.color = cable.cableColor;

            CableTypeSO capturedCable = cable;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                CableManager2.Instance.OnCableTypeSelected(capturedCable);
                ClosePanel();
            });
        }

        cableSelectionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePanel()
    {
        cableSelectionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnCancelClicked()
    {
        cableSelectionPanel.SetActive(false);

        if (CableManager2.Instance != null)
            CableManager2.Instance.CancelAction();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
