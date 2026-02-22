using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReglerDrehung : MonoBehaviour
{
    public Transform Drehtaster; // Transform des Drehtasters
    public Transform playerObject; // Transform des Spieler-Objekts (Zylinder)
    public static float Frequenz; // Variable f�r die abgeleitete Frequenz
    public float verhältnisMouseDrehtaster;
    public static float motorSpeed;
    private float Drehwinkel = 0f; // Anfangsrotation des Drehtasters
    private float minDrehwinkel = -90f; // Minimale Rotation des Drehtasters (in Grad)
    private float maxDrehwinkel = 0f; // Maximale Rotation des Drehtasters (in Grad)
    private bool Tastendruck = false; // Flag, um zu �berpr�fen, ob der Drehtaster gerade gezogen wird

    private void Start()
    {
        // Speichere die Anfangsrotation des Drehtasters
        Drehwinkel = Drehtaster.rotation.eulerAngles.x;
        Drehwinkel = (Drehwinkel + 360) % 360; // Normalisierung auf den Bereich [0, 360)
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // PR�FE, OB DER DREHTASTER GEDR�CKT WIRD!!!
            RaycastHit hit;
            // Der Strahl wird mit der Funktion Ray in ray wird mit der Position der Maus gleichgesetzt.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == Drehtaster) // �berpr�ft, ob hit.transform identisch mit Drehtaster ist
                {
                    Tastendruck = true; //isDragging wird verwendet, um den Drehtaster zu ziehen/drehen
                    //Debug.Log("Regler geklickt");
                }
            }
        }
        else if (Input.GetMouseButtonUp(0)) //negative Flanke der linken Maustaste
        {
            Tastendruck = false;
        }

        // DREHTASTER WIRD GEDR�CKT!!!
        if (Tastendruck || playerObject.position.y > 7f)
        {
            //Drehung aufgrund Mausradbewegung
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float Umdrehung = Input.GetAxisRaw("Mouse ScrollWheel") * 100f * Time.deltaTime;
                Drehwinkel -= Umdrehung;
            }

            // Drehung aufgrund Tastendruck "E" und "Q"
            if (Input.GetKey(KeyCode.E))
            {
                Drehwinkel -= 100f * Time.deltaTime; // Verringert den Drehwinkel, erh�ht die Frequenz
            }
            if (Input.GetKey(KeyCode.Q))
            {
                Drehwinkel += 100f * Time.deltaTime; // Erh�ht den Drehwinkel, verringert die Frequenz
            }

            Drehwinkel = Mathf.Clamp(Drehwinkel, minDrehwinkel, maxDrehwinkel);
            // Wende die Rotation auf das Drehtaster-Objekt an
            Drehtaster.localRotation = Quaternion.Euler(Drehwinkel, 0f, 90f);
        }

        //Betrag Berechnen
        float absMinDrehwinkel = Mathf.Abs(minDrehwinkel);
        float absMaxDrehwinkel = Mathf.Abs(maxDrehwinkel);
        float absDrehwinkel = Mathf.Abs(Drehwinkel);
        // Berechne die relative Drehung des Drehtasters im Bereich von 0 bis 1
        verhältnisMouseDrehtaster = Mathf.InverseLerp(absMaxDrehwinkel, absMinDrehwinkel, absDrehwinkel);

        // Passe die Geschwindigkeit des Motors entsprechend an
        motorSpeed = verhältnisMouseDrehtaster * 2800f;
        //Debug.Log("RPM: " + motorSpeed);

        // Berechne die Frequenz basierend auf der begrenzten Drehung
        Frequenz = verhältnisMouseDrehtaster * 50f; // Annahme, dass 1 Einheit der relativen Drehung 1 Hz entspricht
        //Debug.Log("Frequenz: " + Frequenz);
    }
}





