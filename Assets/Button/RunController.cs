using UnityEngine;
using UnityEngine.Video;

public class RunController : MonoBehaviour
{
    public bool connectionsCorrect = true;
    
    public VideoPlayer videoPlayer;
    public VideoClip countdownVideo;
    public VideoClip erfolgVideo;
    public VideoClip fehlschlagVideo;
    
    [Range(0.1f, 5f)] public float countdownSpeed = 1f;
    [Range(0.1f, 5f)] public float erfolgSpeed = 1f;
    [Range(0.1f, 5f)] public float fehlschlagSpeed = 1f;
    
    public void OnRunButtonPressed()
    {
        videoPlayer.clip = countdownVideo;
        videoPlayer.playbackSpeed = countdownSpeed;
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnCountdownEnde;
    }
    
    public void OnStopButtonPressed()
    {
        // Alles abbrechen
        videoPlayer.loopPointReached -= OnCountdownEnde;
        videoPlayer.loopPointReached -= OnErfolgVideoEnde;
        videoPlayer.Stop();
        
        // Motor stoppen
        MotorDrehung.motorLäuft = false;
        
        // Fehlschlag-Video abspielen
        videoPlayer.clip = fehlschlagVideo;
        videoPlayer.playbackSpeed = fehlschlagSpeed;
        videoPlayer.Play();
    }
    
    void OnCountdownEnde(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnCountdownEnde;
        
        if (connectionsCorrect)
        {
            videoPlayer.clip = erfolgVideo;
            videoPlayer.playbackSpeed = erfolgSpeed;
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnErfolgVideoEnde;
        }
        else
        {
            videoPlayer.clip = fehlschlagVideo;
            videoPlayer.playbackSpeed = fehlschlagSpeed;
            videoPlayer.Play();
        }
    }
    
    void OnErfolgVideoEnde(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnErfolgVideoEnde;
        MotorDrehung.motorLäuft = true;
    }
}
