using UnityEngine;

public class BuchsenManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject Auswahl_Fenster;

    [Header("Player (optional)")]
    public GameObject playerObject;

    private Buchse_Click aktiveBuchse;

    private MouseLook playerMouseLook;

    void Start()
    {
        Debug.Log("BuchsenManager aktiv");

        if (Auswahl_Fenster != null)
            Auswahl_Fenster.SetActive(false);

        if (playerObject != null)
            playerMouseLook = playerObject.GetComponent<MouseLook>();
    }

    public void BuchseGeklickt(Buchse_Click buchse)
    {
        Debug.Log("Buchse geklickt: " + buchse.name);

        if (buchse == null) return;

        if (aktiveBuchse != null)
            aktiveBuchse.SetMarkierung(false);

        aktiveBuchse = buchse;
        aktiveBuchse.SetMarkierung(true);

        if (Auswahl_Fenster != null)
            Auswahl_Fenster.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMouseLook != null)
            playerMouseLook.enabled = false;
    }

    public void PhaseZuweisen(string phase)
    {
        Debug.Log("PhaseZuweisen: " + phase);

        if (aktiveBuchse != null)
        {
            aktiveBuchse.SetText(phase);
        }
        else
        {
            Debug.Log("KEINE aktive Buchse!");
        }

        Schliessen();
    }

    private void Schliessen()
    {
        if (aktiveBuchse != null)
            aktiveBuchse.SetMarkierung(false);

        aktiveBuchse = null;

        if (Auswahl_Fenster != null)
            Auswahl_Fenster.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMouseLook != null)
            playerMouseLook.enabled = true;
    }
}
