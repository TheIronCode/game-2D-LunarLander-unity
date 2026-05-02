using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    private const float GRAVITY_NORMAL = 0.7f;

    public static Lander Instance { get; private set; }

    public event EventHandler OnUpForse;
    public event EventHandler OnLeftForse;
    public event EventHandler OnRightForse;
    public event EventHandler OnBeforeForse;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType
    {
        Success,
        WrongLendingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private State state;


    private void Awake()
    {
        Instance = this;

        fuelAmount = fuelAmountMax;
        SetState(State.WaitingToStart);

        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        OnBeforeForse?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (GameInput.Instance.IsUpActionPressed() ||
                    GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed())
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0)
                {
                    return;
                }

                if (GameInput.Instance.IsUpActionPressed() ||
                    GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed())
                {
                    ConsumeFuel();
                }

                if (GameInput.Instance.IsUpActionPressed())
                {
                    float forse = 15f;
                    landerRigidbody2D.AddForce(forse * transform.up);
                    OnUpForse?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.IsLeftActionPressed())
                {
                    float turnSpeed = 1.5f;
                    landerRigidbody2D.AddTorque(turnSpeed);
                    OnLeftForse?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.IsRightActionPressed())
                {
                    float turnSpeed = -1.5f;
                    landerRigidbody2D.AddTorque(turnSpeed);
                    OnRightForse?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLendingArea,
                dotVector = 0,
                landingSpeed = 0,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;

        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            // Landed too hard!
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0,
                landingSpeed = 0,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.9f;

        if (dotVector < minDotVector)
        {
            // Landed on a too steep angle!
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        Debug.Log("Succesful landing!");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        int landingAngleScore = Mathf.RoundToInt(maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle);

        float maxScoreAmountLandingSpeed = 100;
        int landingSpeedScore = Mathf.RoundToInt((softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed);

        Debug.Log("LandingAngleScore: " + landingAngleScore);
        Debug.Log("LandingSpeedScore: " + landingSpeedScore);

        int score = (landingAngleScore + landingSpeedScore + GameManager.Instance.GetScore()) * landingPad.GetScoreMultiplier();

        Debug.Log("Score: " +  score);

        OnLanded?.Invoke(this, new OnLandedEventArgs {
            landingType = LandingType.Success,
            dotVector = landingAngleScore,
            landingSpeed = landingSpeedScore,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 5f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
        {
            state = state
        });
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocityY;
    }
}
