using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSourse;

    private Lander lander;
    private bool isThrusterActive;
    private int activeThrustersCount = 0;
    private float fadeSpeed = 20f;

    private void Start()
    {
        lander = GetComponent<Lander>();

        lander.OnBeforeForse += Lander_OnBeforeForse;
        lander.OnUpForse += Lander_OnUpForse;
        lander.OnLeftForse += Lander_OnLeftForse;
        lander.OnRightForse += Lander_OnRightForse;

        thrusterAudioSourse.loop = true;
        thrusterAudioSourse.volume = 0f;
        thrusterAudioSourse.Play();
    }

    private void Update()
    {
        float soundVolumeNormalized = SoundManager.Instance != null ? SoundManager.Instance.GetSoundVolumeNormalized() : 1f;
        float targetVolume = isThrusterActive ? soundVolumeNormalized : 0f;
        thrusterAudioSourse.volume = Mathf.MoveTowards(thrusterAudioSourse.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (lander != null)
        {
            lander.OnBeforeForse -= Lander_OnBeforeForse;
            lander.OnUpForse -= Lander_OnUpForse;
            lander.OnLeftForse -= Lander_OnLeftForse;
            lander.OnRightForse -= Lander_OnRightForse;
        }
    }

    private void Lander_OnRightForse(object sender, System.EventArgs e)
    {
        activeThrustersCount++;
        isThrusterActive = true;
    }

    private void Lander_OnLeftForse(object sender, System.EventArgs e)
    {
        activeThrustersCount++;
        isThrusterActive = true;
    }

    private void Lander_OnUpForse(object sender, System.EventArgs e)
    {
        activeThrustersCount++;
        isThrusterActive = true;
    }

    private void Lander_OnBeforeForse(object sender, System.EventArgs e)
    {
        if (activeThrustersCount <= 0)
        {
            isThrusterActive = false;
        }
        activeThrustersCount = 0;
    }
}