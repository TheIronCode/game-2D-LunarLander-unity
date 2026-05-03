using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const int MUSIC_VOLUME_MAX = 10;

    private static float musicTime;
    private static int musicVolume = 6;

    private AudioSource musicAudioSourse;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicAudioSourse = GetComponent<AudioSource>();
        musicAudioSourse.time = musicTime;
    }

    private void Start()
    {
        musicAudioSourse.volume = GetMusicVolumeNormalized();
    }

    private void Update()
    {
        musicTime = musicAudioSourse.time;
    }
    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
        musicAudioSourse.volume = GetMusicVolumeNormalized();
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetMusicVolumeNormalized()
    {
        return ((float)musicVolume) / MUSIC_VOLUME_MAX;
    }

}
