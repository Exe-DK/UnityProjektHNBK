using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Objectclick : MonoBehaviour
{
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
    //public GameObject ASYM;
    public int isPaused = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //ASYM.SetActive(false);
        PanelAusgang.SetActive(false);
        PanelTyristoren1.SetActive(false);
        PanelTyristoren2.SetActive(false);
        PanelEinspeisung.SetActive(false);
        PanelPotFrei.SetActive(false);
        PanelPoti.SetActive(false);
        PanelBrems.SetActive(false);
        PanelTrenn.SetActive(false);
        Panel24V.SetActive(false);
        GIO.SetActive(false);
        RFR.SetActive(false);
        V24ext.SetActive(false);
        DI3brem.SetActive(false);
        DI3Dreh.SetActive(false);
        DI1.SetActive(false);
        DI2.SetActive(false);
        DO1.SetActive(false);
        T1.SetActive(false);
        CZ.SetActive(false);
        VB.SetActive(false);
        Erde.SetActive(false);
        PanelFU.SetActive(false);
        Panel01U.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        

        
        if (Mouse.current.rightButton.wasPressedThisFrame && isPaused == 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.CompareTag("Trigger Kontakte ausgang"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelAusgang.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
     
                }
                if (hit.collider.CompareTag("Trigger Tyristoren1"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelTyristoren1.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger Tyristoren2"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelTyristoren2.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger Einspeisung"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelEinspeisung.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger Pot.Frei.Kont"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelPotFrei.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger Poti"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelPoti.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                if (hit.collider.CompareTag("Trigger Brems"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelBrems.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                if (hit.collider.CompareTag("Trigger Trenn"))
                {
                    
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelTrenn.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger 24V"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    Panel24V.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger GIO"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    GIO.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger RFR"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    RFR.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger 24V Ext"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    V24ext.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger DI3 High Pegel Brem"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    DI3brem.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger DI3 Dreh"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    DI3Dreh.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger DI1 40%"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    DI1.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger DI2 60%"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    DI2.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("TriggerDO1"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    DO1.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger T1"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    T1.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger CZ"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    CZ.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger VB"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    VB.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }
                if (hit.collider.CompareTag("Trigger Erde"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    Erde.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;

                }     
                if (hit.collider.CompareTag("TriggerFU"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    PanelFU.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                if (hit.collider.CompareTag("Trigger01U"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    Panel01U.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }
               /* if (hit.collider.CompareTag("Trigger Motor"))
                {
                    isPaused = 1;
                    PrintName(hit.transform.gameObject);
                    ASYM.SetActive(true);
                    Time.timeScale = 0;
                    GameObject varGameObject = GameObject.Find("Cylinder");
                    varGameObject.GetComponent<InputManager>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }*/

            }
        }
        if (isPaused == 1 && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)) //NEU zus�tzliche Bedingungen
        { 
            PanelAusgang.SetActive(false);
            PanelTyristoren1.SetActive(false);
            PanelTyristoren2.SetActive(false);
            PanelEinspeisung.SetActive(false);
            PanelPotFrei.SetActive(false);
            PanelPoti.SetActive(false);
            PanelBrems.SetActive(false);
            PanelTrenn.SetActive(false);
            Panel24V.SetActive(false);
            GIO.SetActive(false);
            RFR.SetActive(false);
            V24ext.SetActive(false);
            DI3brem.SetActive(false);
            DI3Dreh.SetActive(false);
            DI1.SetActive(false);
            DI2.SetActive(false);
            DO1.SetActive(false);
            T1.SetActive(false);
            CZ.SetActive(false);
            VB.SetActive(false);
            Erde.SetActive(false);
            PanelFU.SetActive(false);
            Panel01U.SetActive(false);
            //ASYM.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = 0;
            GameObject varGameObject = GameObject.Find("Cylinder");
            varGameObject.GetComponent<InputManager>().enabled = true;
        }
    }
    private void PrintName(GameObject go)
    {
        print(go.name);
    }
    public void Pausegame()
    {

    }
    public void resumeGame()
    {

    }
}
