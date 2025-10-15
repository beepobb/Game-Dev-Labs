using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource bgmAudio;

    void Awake()
    {
        // subscribe to Game Restart event
        // GameManager.instance.gameRestart.AddListener(PlayBGM);
        // No need since restart now reloads scene!

        // subscribe to Game Over event
        GameManager.instance.gameOver.AddListener(StopBGM);
    }
    void Start()
    {
        bgmAudio = GetComponent<AudioSource>();
    }
    public void PlayBGM()
    {
        bgmAudio.time = 0f;
        bgmAudio.Play();
    }

    public void StopBGM()
    {
        bgmAudio.Stop();
    }
}
