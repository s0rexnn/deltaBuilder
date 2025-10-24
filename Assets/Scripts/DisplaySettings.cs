using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ForceAspectRatio : MonoBehaviour
{
    [Header("Aspect Ratio Settings")]
    public float targetAspectWidth = 4f;
    public float targetAspectHeight = 3f;

    [Header("Resolution Settings")]
    public int resolutionWidth = 640;
    public int resolutionHeight = 480;
    public bool fullscreen = false;

    [Header("Performance Settings")]
    [Tooltip("Target framerate cap. Set to -1 for unlimited.")]
    public int targetFrameRate = 30;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        ApplyAspect();
        ApplyFrameRate();

        // Force resolution at startup
        Screen.SetResolution(resolutionWidth, resolutionHeight, fullscreen);
    }

    private void Update()
    {
        ApplyAspect();
    }

    private void ApplyAspect()
    {
        float targetAspect = targetAspectWidth / targetAspectHeight;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
            cam.rect = rect;
        }
    }

    private void ApplyFrameRate()
    {
        if (targetFrameRate > 0)
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 1;
        }
    }
}
