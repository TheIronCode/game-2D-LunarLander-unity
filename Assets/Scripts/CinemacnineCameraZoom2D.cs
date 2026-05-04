using Unity.Cinemachine;
using UnityEngine;

public class CinemacnineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTOGRAPHICS_SIZE = 10f;

    public static CinemacnineCameraZoom2D Instance { get; private set; }

    [SerializeField] private CinemachineCamera cinemachineCamera;

    private float targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float currentOrthographicSize = cinemachineCamera.Lens.OrthographicSize;
        if (Mathf.Abs(currentOrthographicSize - targetOrthographicSize) < 0.01f)
        {
            cinemachineCamera.Lens.OrthographicSize = targetOrthographicSize;
            return;
        }
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(currentOrthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);
    }

    public void SetTargetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }

    public void SetNormalOrthoghaphicsSize()
    {
        SetTargetOrthographicSize(NORMAL_ORTOGRAPHICS_SIZE);
    }
}
