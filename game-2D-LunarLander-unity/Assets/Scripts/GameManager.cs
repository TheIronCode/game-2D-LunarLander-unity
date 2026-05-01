using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private int score;
    private float time;
    private bool isTimerActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChange += Langer_OnStateChange;

        LoadCurrentLevel();
    }

    private void Langer_OnStateChange(object sender, Lander.OnStateChangeEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;
        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemacnineCameraZoom2D.Instance.SetNormalOrthoghaphicsSize();
        }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        score = e.score;
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(100);
    }

    private void LoadCurrentLevel()
    {
        foreach (GameLevel gamelevel in gameLevelList)
        {
            if(gamelevel.GetLevelNumber() == levelNumber)
            {
                GameLevel spawnedGameLevel = Instantiate(gamelevel, Vector3.zero, Quaternion.identity);
                Lander.Instance.transform.position = spawnedGameLevel.GetLandetStartPosition();
                cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetcameraStartTargetTransform();
                CinemacnineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
            }
        }
    }

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        SceneManager.LoadScene(0);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }
}
