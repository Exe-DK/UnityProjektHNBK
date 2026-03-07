using UnityEngine;

public class MotorDrehung : MonoBehaviour
{
    public static bool motorLäuft = false;
    public AudioSource motorSound;

    [Header("Sound Einstellungen")]
    public float minPitch = 0.3f;   // bei niedrigster Drehzahl
    public float maxPitch = 1.5f;   // bei höchster Drehzahl
    public float minVolume = 0.2f;  // leise bei wenig Drehzahl
    public float maxVolume = 1.0f;  // laut bei voller Drehzahl

    void Update()
    {
        if (!motorLäuft)
        {
            if (motorSound.isPlaying) motorSound.Stop();
            return;
        }

        if (!motorSound.isPlaying) motorSound.Play();

        float Drehzahl = Berechnung.nAP;
        float maxDrehzahl = 2800f;

        // Verhältnis 0 bis 1
        float t = Mathf.Clamp01(Drehzahl / maxDrehzahl);

        // Pitch und Lautstärke anpassen
        motorSound.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        motorSound.volume = Mathf.Lerp(minVolume, maxVolume, t);

        transform.Rotate(Vector3.up * Time.deltaTime * Drehzahl);
    }
}
