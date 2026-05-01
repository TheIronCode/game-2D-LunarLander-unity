using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;
    [SerializeField] private GameObject landerExplosionVfx;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();

        lander.OnUpForse += Lander_OnUpForse;
        lander.OnLeftForse += Lander_OnLeftForse;
        lander.OnRightForse += Lander_OnRightForse;
        lander.OnBeforeForse += Lander_OnBeforeForse;

        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.TooFastLanding:
            case Lander.LandingType.WrongLendingArea:
                //Crash
                Instantiate(landerExplosionVfx, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
            case Lander.LandingType.Success:
                break;
        }
    }

    private void Lander_OnBeforeForse(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void Lander_OnRightForse(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, true);
    }

    private void Lander_OnLeftForse(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void Lander_OnUpForse(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, true);
        SetEnableThrusterParticleSystem(middleThrusterParticleSystem, true);
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void SetEnableThrusterParticleSystem(ParticleSystem particleSystem, bool enable)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enable;
    }
}
