using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;
using static UnityEngine.Rendering.DebugUI;

public class Objectclick : MonoBehaviour
{
    [Header("Panels")]
    public GameObject Panel01U;
    public GameObject PanelFU;
    public GameObject PanelAusgang;
    public GameObject PanelTyristoren1;
    public GameObject PanelTyristoren2;
    public GameObject PanelEinspeisung;
    public GameObject PanelPotFrei;
    public GameObject PanelPoti;
    public GameObject PanelBrems;
    public GameObject PanelTrenn;
    public GameObject Panel24V;
    public GameObject GIO;
    public GameObject RFR;
    public GameObject V24ext;
    public GameObject DI3brem;
    public GameObject DI3Dreh;
    public GameObject DI1;
    public GameObject DI2;
    public GameObject DO1;
    public GameObject T1;
    public GameObject CZ;
    public GameObject VB;
    public GameObject Erde;
    public GameObject PanelMotorklemmbrett;
    public GameObject PanelBremswiderstandExtern; // Neues Panel hinzugefügt
    public GameObject PanelAnschlussplan_und_Aufgabe; // Neues Panel hinzugefügt
    public GameObject PanelRückmeldungmonitorlinks; // Neues Panel hinzugefügt
    public GameObject PanelRückmeldungmonitorrechts; // Neues Panel hinzugefügt



    [Header("Player Object")]
    public GameObject playerObject; // Hier "Cylinder" reinziehen

    private int isPaused = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        HideAllPanels();
    }

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && isPaused == 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
            {
                PrintName(hit.transform.gameObject);

                if (hit.collider.CompareTag("Trigger Kontakte ausgang"))
                    OpenPanel(PanelAusgang);
                else if (hit.collider.CompareTag("Trigger Tyristoren1"))
                    OpenPanel(PanelTyristoren1);
                else if (hit.collider.CompareTag("Trigger Tyristoren2"))
                    OpenPanel(PanelTyristoren2);
                else if (hit.collider.CompareTag("Trigger Einspeisung"))
                    OpenPanel(PanelEinspeisung);
                else if (hit.collider.CompareTag("Trigger Pot.Frei.Kont"))
                    OpenPanel(PanelPotFrei);
                else if (hit.collider.CompareTag("Trigger Poti"))
                    OpenPanel(PanelPoti);
                else if (hit.collider.CompareTag("Trigger Brems"))
                    OpenPanel(PanelBrems);
                else if (hit.collider.CompareTag("Trigger Trenn"))
                    OpenPanel(PanelTrenn);
                else if (hit.collider.CompareTag("Trigger 24V"))
                    OpenPanel(Panel24V);
                else if (hit.collider.CompareTag("Trigger GIO"))
                    OpenPanel(GIO);
                else if (hit.collider.CompareTag("Trigger RFR"))
                    OpenPanel(RFR);
                else if (hit.collider.CompareTag("Trigger 24V Ext"))
                    OpenPanel(V24ext);
                else if (hit.collider.CompareTag("Trigger DI3 High Pegel Brem"))
                    OpenPanel(DI3brem);
                else if (hit.collider.CompareTag("Trigger DI3 Dreh"))
                    OpenPanel(DI3Dreh);
                else if (hit.collider.CompareTag("Trigger DI1 40%"))
                    OpenPanel(DI1);
                else if (hit.collider.CompareTag("Trigger DI2 60%"))
                    OpenPanel(DI2);
                else if (hit.collider.CompareTag("TriggerDO1"))
                    OpenPanel(DO1);
                else if (hit.collider.CompareTag("Trigger T1"))
                    OpenPanel(T1);
                else if (hit.collider.CompareTag("Trigger CZ"))
                    OpenPanel(CZ);
                else if (hit.collider.CompareTag("Trigger VB"))
                    OpenPanel(VB);
                else if (hit.collider.CompareTag("Trigger Erde"))
                    OpenPanel(Erde);
                else if (hit.collider.CompareTag("TriggerFU"))
                    OpenPanel(PanelFU);
                else if (hit.collider.CompareTag("Trigger01U"))
                    OpenPanel(Panel01U);
                else if (hit.collider.CompareTag("Trigger Motorklemmbrett"))
                    OpenPanel(PanelMotorklemmbrett);
                else if (hit.collider.CompareTag("Trigger Bremswiderstand Extern"))
                    OpenPanel(PanelBremswiderstandExtern); // Neuer Trigger hinzugefügt
                else if (hit.collider.CompareTag("Trigger Anschlussplan_und_Aufgabe"))
                    OpenPanel(PanelAnschlussplan_und_Aufgabe); // Neuer Trigger hinzugefügt
                else if (hit.collider.CompareTag("Trigger Rückmeldungmonitor links"))
                    OpenPanel(PanelRückmeldungmonitorlinks); // Neuer Trigger hinzugefügt
                else if (hit.collider.CompareTag("Trigger Rückmeldungsmonitor rechts"))
                    OpenPanel(PanelRückmeldungmonitorrechts); // Neuer Trigger hinzugefügt
            }
        }

        if (isPaused == 1 &&
            (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            ClosePanelsAndResume();
        }
    }

    private void OpenPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Panel ist nicht im Inspector zugewiesen!");
            return;
        }

        isPaused = 1;
        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;

        if (playerObject != null)
        {
            InputManager inputManager = playerObject.GetComponent<InputManager>();
            if (inputManager != null)
            {
                inputManager.enabled = false;
            }
            else
            {
                Debug.LogWarning("InputManager wurde auf playerObject nicht gefunden.");
            }
        }
        else
        {
            Debug.LogWarning("playerObject ist nicht gesetzt.");
        }
    }

    private void ClosePanelsAndResume()
    {
        HideAllPanels();

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = 0;

        if (playerObject != null)
        {
            InputManager inputManager = playerObject.GetComponent<InputManager>();
            if (inputManager != null)
            {
                inputManager.enabled = true;
            }
        }
    }

    private void HideAllPanels()
    {
        if (PanelAusgang != null) PanelAusgang.SetActive(false);
        if (PanelTyristoren1 != null) PanelTyristoren1.SetActive(false);
        if (PanelTyristoren2 != null) PanelTyristoren2.SetActive(false);
        if (PanelEinspeisung != null) PanelEinspeisung.SetActive(false);
        if (PanelPotFrei != null) PanelPotFrei.SetActive(false);
        if (PanelPoti != null) PanelPoti.SetActive(false);
        if (PanelBrems != null) PanelBrems.SetActive(false);
        if (PanelTrenn != null) PanelTrenn.SetActive(false);
        if (Panel24V != null) Panel24V.SetActive(false);
        if (GIO != null) GIO.SetActive(false);
        if (RFR != null) RFR.SetActive(false);
        if (V24ext != null) V24ext.SetActive(false);
        if (DI3brem != null) DI3brem.SetActive(false);
        if (DI3Dreh != null) DI3Dreh.SetActive(false);
        if (DI1 != null) DI1.SetActive(false);
        if (DI2 != null) DI2.SetActive(false);
        if (DO1 != null) DO1.SetActive(false);
        if (T1 != null) T1.SetActive(false);
        if (CZ != null) CZ.SetActive(false);
        if (VB != null) VB.SetActive(false);
        if (Erde != null) Erde.SetActive(false);
        if (PanelFU != null) PanelFU.SetActive(false);
        if (Panel01U != null) Panel01U.SetActive(false);
        if (PanelMotorklemmbrett != null) PanelMotorklemmbrett.SetActive(false);
        if (PanelBremswiderstandExtern != null) PanelBremswiderstandExtern.SetActive(false); // Neues Panel hinzugefügt
        if (PanelAnschlussplan_und_Aufgabe != null) PanelAnschlussplan_und_Aufgabe.SetActive(false);
        if (PanelRückmeldungmonitorlinks != null) PanelRückmeldungmonitorlinks.SetActive(false);
        if (PanelRückmeldungmonitorrechts != null) PanelRückmeldungmonitorrechts.SetActive(false);



    }

    private void PrintName(GameObject go)
    {
        Debug.Log(go.name);
    }

    public void Pausegame()
    {
    }

    public void resumeGame()
    {
    }
}
