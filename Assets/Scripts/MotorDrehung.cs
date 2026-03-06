using UnityEngine;

public class MotorDrehung : MonoBehaviour
{
    public static bool motorLäuft = false;
    public AudioSource motorSound;

    void Update()
    {
        if (!motorLäuft)
        {
            if (motorSound.isPlaying) motorSound.Stop();
            return;
        }
        
        if (!motorSound.isPlaying) motorSound.Play();
        
        float Drehzahl = Berechnung.nAP;
        transform.Rotate(Vector3.up * Time.deltaTime * Drehzahl);
    }
}
