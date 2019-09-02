using UnityEngine;
using UnityEngine.UI;

public class FreeAspectCamera : MonoBehaviour
{
    public CanvasScaler[] canvases;
    public float targetHeight;
    public float targetWidth;

    private new Camera camera;
    private int lastScreenHeight;
    private int lastScreenWidth;

    private void Start()
    {
        camera = GetComponent<Camera>();
        lastScreenHeight = Screen.height;
        lastScreenWidth = Screen.width;
        Fit();
    }

    private void Update()
    {
        if (lastScreenHeight != Screen.height || lastScreenWidth != Screen.width)
        {
            Fit();
            lastScreenHeight = Screen.height;
            lastScreenWidth = Screen.width;
        }
    }

    private void Fit()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        if (aspectRatio * targetHeight < targetWidth)
        {
            camera.orthographicSize = targetWidth * 0.5f / aspectRatio;
            for (int a = 0; a < canvases.Length; ++a)
            {
                canvases[a].matchWidthOrHeight = 0f;
            }
        }
        else
        {
            camera.orthographicSize = targetHeight * 0.5f;
            for (int a = 0; a < canvases.Length; ++a)
            {
                canvases[a].matchWidthOrHeight = 1f;
            }
        }
    }
}
