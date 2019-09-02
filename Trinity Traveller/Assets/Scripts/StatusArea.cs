using UnityEngine;
using UnityEngine.UI;

public class StatusArea : MonoBehaviour
{
    public static Color[] statusColor;

    public Image imageCollectedStatus;
    public PlayerChange playerChange;
    public int status;

    private bool isStaying;

    private void Start()
    {
        isStaying = false;
        statusColor = new Color[]
        {
            new Color(0f,1f,0f),
            new Color(0f,0f,1f),
            new Color(1f,0f,0f)
        };
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isStaying)
            {
                imageCollectedStatus.color = statusColor[status - 1];
                playerChange.collectedStatus = status;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isStaying)
        {
            isStaying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isStaying = false;
    }
}
