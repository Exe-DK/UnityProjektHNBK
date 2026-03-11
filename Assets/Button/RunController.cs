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

    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    public ResultPanel resultPanel;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnRunButtonPressed()
    {
        connectionsCorrect = TaskManager.Instance.CheckAll();

        videoPlayer.clip = countdownVideo;
        videoPlayer.playbackSpeed = countdownSpeed;
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnCountdownEnde;
    }

    public void OnStopButtonPressed()
    {
        videoPlayer.loopPointReached -= OnCountdownEnde;
        videoPlayer.loopPointReached -= OnErfolgVideoEnde;
        videoPlayer.loopPointReached -= OnFehlschlagVideoEnde;
        videoPlayer.Stop();

        MotorDrehung.motorLäuft = false;

        videoPlayer.clip = fehlschlagVideo;
        videoPlayer.playbackSpeed = fehlschlagSpeed;
        videoPlayer.Play();
        audioSource.PlayOneShot(wrongSound);
        videoPlayer.loopPointReached += OnFehlschlagVideoEnde;
    }

    void OnCountdownEnde(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnCountdownEnde;

        if (connectionsCorrect)
        {
            videoPlayer.clip = erfolgVideo;
            videoPlayer.playbackSpeed = erfolgSpeed;
            videoPlayer.Play();
            audioSource.PlayOneShot(correctSound);
            videoPlayer.loopPointReached += OnErfolgVideoEnde;
        }
        else
        {
            videoPlayer.clip = fehlschlagVideo;
            videoPlayer.playbackSpeed = fehlschlagSpeed;
            videoPlayer.Play();
            audioSource.PlayOneShot(wrongSound);
            videoPlayer.loopPointReached += OnFehlschlagVideoEnde;
        }
    }

    void OnErfolgVideoEnde(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnErfolgVideoEnde;
        MotorDrehung.motorLäuft = true;
        resultPanel.ShowResults();
    }

    void OnFehlschlagVideoEnde(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnFehlschlagVideoEnde;
        resultPanel.ShowResults();
    }
}
