using UnityEngine;
using UnityEngine.UI;

public class FPSUpdate : MonoBehaviour
{
    public float updateInterval;

    private string fps;
    private string[] fpsParts;
    private float fpsValue;
    private int frame;
    private float lastUpdateTime;
    private float nextUpdateTime;
    private Text textFPS;

    private void Start()
    {
        frame = 0;
        lastUpdateTime = Time.time;
        nextUpdateTime = ((int)(lastUpdateTime / updateInterval) + 1) * updateInterval;
        textFPS = GetComponent<Text>();
    }

    private void Update()
    {
        ++frame;
        if (nextUpdateTime < Time.time)
        {
            fpsValue = frame / (Time.time - lastUpdateTime);
            if (fpsValue > 999.9f)
            {
                textFPS.text = "999.9 FPS";
            }
            else
            {
                fpsParts = fpsValue.ToString().Split('.');
                fps = fpsParts[0] + ".";
                if (fpsParts.Length == 1)
                {
                    textFPS.text = fps + "0 FPS";
                }
                else
                {
                    textFPS.text = fps + fpsParts[1].Substring(0, 1) + " FPS";
                }
            }
            frame = 0;
            lastUpdateTime = Time.time;
            nextUpdateTime = ((int)(lastUpdateTime / updateInterval) + 1) * updateInterval;
        }
    }
}
