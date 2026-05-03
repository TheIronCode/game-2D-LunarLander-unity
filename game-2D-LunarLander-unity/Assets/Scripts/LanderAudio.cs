using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSourse;

    private Lander lander;

    private void Start()
    {
        lander = GetComponent<Lander>();

        lander.OnBeforeForse += Lander_OnBeforeForse;
        lander.OnUpForse += Lander_OnUpForse;
        lander.OnLeftForse += Lander_OnLeftForse;
        lander.OnRightForse += Lander_OnRightForse;
        SoundManager.Instance.OnSoundVolumeChange += SoundManager_OnSoundVolumeChange;

        thrusterAudioSourse.Pause();
    }

    private void SoundManager_OnSoundVolumeChange(object sender, System.EventArgs e)
    {
        thrusterAudioSourse.volume = SoundManager.Instance.GetSoundVolumeNormalized();
    }

    private void Lander_OnRightForse(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSourse.isPlaying)
        {
            thrusterAudioSourse.Play();
        }
    }

    private void Lander_OnLeftForse(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSourse.isPlaying)
        {
            thrusterAudioSourse.Play();
        }
    }

    private void Lander_OnUpForse(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSourse.isPlaying)
        {
            thrusterAudioSourse.Play();
        }
    }

    private void Lander_OnBeforeForse(object sender, System.EventArgs e)
    {
        thrusterAudioSourse.Pause();
    }
}