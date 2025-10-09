using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource bgmAudio;
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
