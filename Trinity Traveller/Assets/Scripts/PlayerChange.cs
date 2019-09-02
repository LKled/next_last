using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    public int collectedStatus;
    public int currentStatus;
    public GameObject[] playerRoots;
    public Transform[] playerStatuses;
    public WorldBorder worldBorder;

    private BallMove ballMove;
    private PlaneMove planeMove;
    private RockMove rockMove;
    private TravellerMove travellerMove;

    private void Start()
    {
        ballMove = playerStatuses[1].GetComponent<BallMove>();
        planeMove = playerStatuses[2].GetComponent<PlaneMove>();
        rockMove = playerStatuses[3].GetComponent<RockMove>();
        travellerMove = playerStatuses[0].GetComponent<TravellerMove>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentStatus != 0)
            {
                PlayerStatusChange(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (collectedStatus != currentStatus)
            {
                PlayerStatusChange(collectedStatus);
            }
        }
    }

    private void PlayerStatusChange(int status)
    {
        playerStatuses[status].position = playerStatuses[currentStatus].position;
        worldBorder.player = playerStatuses[status];
        if (currentStatus == 0)
        {
            RotationYChange(travellerMove.rotationY);
            playerStatuses[status].rotation = Quaternion.Euler(0f, travellerMove.rotationY, 0f);
        }
        else if (currentStatus == 1)
        {
            RotationYChange(ballMove.rotationY);
            playerStatuses[status].rotation = Quaternion.Euler(0f, ballMove.rotationY, 0f);
        }
        else if (currentStatus == 2)
        {
            RotationYChange(planeMove.rotationY);
            playerStatuses[status].rotation = Quaternion.Euler(0f, planeMove.rotationY, 0f);
        }
        else if (currentStatus == 3)
        {
            RotationYChange(rockMove.rotationY);
            playerStatuses[status].rotation = Quaternion.Euler(0f, rockMove.rotationY, 0f);
        }
        playerRoots[currentStatus].SetActive(false);
        playerRoots[status].SetActive(true);
        currentStatus = status;
    }

    private void RotationYChange(float rotationY)
    {
        ballMove.rotationY = rotationY;
        planeMove.rotationY = rotationY;
        rockMove.rotationY = rotationY;
        travellerMove.rotationY = rotationY;
    }
}
