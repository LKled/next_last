using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public Material[] skyboxes;

    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ++currentIndex;
            if (currentIndex == skyboxes.Length)
            {
                currentIndex = 0;
            }
            RenderSettings.skybox = skyboxes[currentIndex];
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync("Test Demo");
        }
    }
}
