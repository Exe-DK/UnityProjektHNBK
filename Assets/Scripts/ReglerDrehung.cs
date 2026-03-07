using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReglerDrehung : MonoBehaviour
{
    [Header("Referenzen")]
    public Transform Drehtaster;

    [Header("Einstellungen")]
    public float drehGeschwindigkeit = 100f;
    public float interaktionsDistanz = 2f;
    public KeyCode tastePlus = KeyCode.E;
    public KeyCode tasteMinus = KeyCode.Q;
    public float minVerhältnis = 0.05f;

    [Header("Frequenzumrichter Tasten")]
    public float tastenSchrittweite = 0.02f; // 2% pro Klick = 1 Hz

    [Header("Ausgabe (nur lesen)")]
    public float verhältnisMouseDrehtaster;
    public static float Frequenz;
    public static float motorSpeed;

    private float Drehwinkel = 0f;
    private float minDrehwinkel = -210f;
    private float maxDrehwinkel = 30f;
    private Transform playerCamera;

    private void Start()
    {
        playerCamera = Camera.main.transform;

        Drehwinkel = Mathf.Lerp(maxDrehwinkel, minDrehwinkel, minVerhältnis);
        Drehtaster.localRotation = Quaternion.Euler(Drehwinkel, 0f, 90f);
    }

    private void Update()
    {
        float distanz = Vector3.Distance(playerCamera.position, Drehtaster.position);

        if (distanz <= interaktionsDistanz)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                Drehwinkel -= scroll * drehGeschwindigkeit * 10f * Time.deltaTime;
            }

            if (Input.GetKey(tastePlus))
            {
                Drehwinkel -= drehGeschwindigkeit * Time.deltaTime;
            }
            if (Input.GetKey(tasteMinus))
            {
                Drehwinkel += drehGeschwindigkeit * Time.deltaTime;
            }

            float minDrehwinkelBegrenzt = Mathf.Lerp(maxDrehwinkel, minDrehwinkel, minVerhältnis);
            Drehwinkel = Mathf.Clamp(Drehwinkel, minDrehwinkel, minDrehwinkelBegrenzt);

            Drehtaster.localRotation = Quaternion.Euler(Drehwinkel, 0f, 90f);
        }

        verhältnisMouseDrehtaster = Mathf.InverseLerp(maxDrehwinkel, minDrehwinkel, Drehwinkel);
        verhältnisMouseDrehtaster = Mathf.Max(verhältnisMouseDrehtaster, minVerhältnis);

        motorSpeed = verhältnisMouseDrehtaster * 2800f;
        Frequenz = verhältnisMouseDrehtaster * 50f;
    }

    // ───────────────────────────────────────────
    // Öffentliche Methoden für FU-Tasten
    // ───────────────────────────────────────────

    public void FrequenzHoch()
    {
        float neuesVerhältnis = verhältnisMouseDrehtaster + tastenSchrittweite;
        neuesVerhältnis = Mathf.Clamp(neuesVerhältnis, minVerhältnis, 1f);
        Drehwinkel = Mathf.Lerp(maxDrehwinkel, minDrehwinkel, neuesVerhältnis);
        Drehtaster.localRotation = Quaternion.Euler(Drehwinkel, 0f, 90f);
    }

    public void FrequenzRunter()
    {
        float neuesVerhältnis = verhältnisMouseDrehtaster - tastenSchrittweite;
        neuesVerhältnis = Mathf.Clamp(neuesVerhältnis, minVerhältnis, 1f);
        Drehwinkel = Mathf.Lerp(maxDrehwinkel, minDrehwinkel, neuesVerhältnis);
        Drehtaster.localRotation = Quaternion.Euler(Drehwinkel, 0f, 90f);
    }
}
