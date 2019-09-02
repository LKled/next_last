using UnityEngine;

public class WorldBorder : MonoBehaviour
{
    public Object borderBlock;
    public Transform[] borders;
    public float maxX;
    public float maxY;
    public float maxZ;
    public float minX;
    public float minY;
    public float minZ;
    public Transform player;

    private float alpha;
    private Vector2Int[] centers;
    private Vector2Int[] lastCenters;
    private Renderer[] renderers;

    private void Start()
    {
        centers = new Vector2Int[3];
        lastCenters = new Vector2Int[3];
        lastCenters[0] = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));
        borders[0].position = new Vector3(lastCenters[0].x, lastCenters[0].y, maxZ);
        borders[1].position = new Vector3(lastCenters[0].x, lastCenters[0].y, minZ);
        lastCenters[1] = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.z));
        borders[2].position = new Vector3(lastCenters[1].x, maxY, lastCenters[1].y);
        borders[3].position = new Vector3(lastCenters[1].x, minY, lastCenters[1].y);
        lastCenters[2] = new Vector2Int(Mathf.RoundToInt(player.position.y), Mathf.RoundToInt(player.position.z));
        borders[4].position = new Vector3(maxX, lastCenters[2].x, lastCenters[2].y);
        borders[5].position = new Vector3(minX, lastCenters[2].x, lastCenters[2].y);
        renderers = new Renderer[486];
        Quaternion[] borderBlockRotations = new Quaternion[3];
        borderBlockRotations[0] = Quaternion.Euler(90f, 0f, 0f);
        borderBlockRotations[1] = Quaternion.Euler(0f, 0f, 0f);
        borderBlockRotations[2] = Quaternion.Euler(0f, 0f, 90f);
        GameObject gameObject;
        for (int a = 0; a < 9; ++a)
        {
            for (int b = 0; b < 9; ++b)
            {
                gameObject = Instantiate(borderBlock) as GameObject;
                gameObject.transform.parent = borders[0];
                gameObject.transform.localPosition = new Vector3(a - 4f, b - 4f, 0.1f);
                gameObject.transform.rotation = borderBlockRotations[0];
                renderers[a * 9 + b] = gameObject.GetComponent<Renderer>();
                gameObject = Instantiate(borderBlock) as GameObject;
                gameObject.transform.parent = borders[1];
                gameObject.transform.localPosition = new Vector3(a - 4f, b - 4f, -0.1f);
                gameObject.transform.rotation = borderBlockRotations[0];
                renderers[a * 9 + b + 81] = gameObject.GetComponent<Renderer>();
            }
        }
        for (int a = 0; a < 9; ++a)
        {
            for (int b = 0; b < 9; ++b)
            {
                gameObject = Instantiate(borderBlock) as GameObject;
                gameObject.transform.parent = borders[2];
                gameObject.transform.localPosition = new Vector3(a - 4f, 0.1f, b - 4f);
                gameObject.transform.rotation = borderBlockRotations[1];
                renderers[a * 9 + b + 162] = gameObject.GetComponent<Renderer>();
                gameObject = Instantiate(borderBlock) as GameObject;
                gameObject.transform.parent = borders[3];
                gameObject.transform.localPosition = new Vector3(a - 4f, -0.1f, b - 4f);
                gameObject.transform.rotation = borderBlockRotations[1];
                renderers[a * 9 + b + 243] = gameObject.GetComponent<Renderer>();
            }
        }
        for (int a = 0; a < 9; ++a)
        {
            for (int b = 0; b < 9; ++b)
            {
                gameObject = Instantiate(borderBlock) as GameObject;
                gameObject.transform.parent = borders[4];
                gameObject.transform.localPosition = new Vector3(0.1f, a - 4f, b - 4f);
                gameObject.transform.rotation = borderBlockRotations[2];
                renderers[a * 9 + b + 324] = gameObject.GetComponent<Renderer>();
                gameObject = Instantiate(borderBlock) as GameObject;
                gameObject.transform.parent = borders[5];
                gameObject.transform.localPosition = new Vector3(-0.1f, a - 4f, b - 4f);
                gameObject.transform.rotation = borderBlockRotations[2];
                renderers[a * 9 + b + 405] = gameObject.GetComponent<Renderer>();
            }
        }
    }

    private void Update()
    {
        centers[0] = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));
        centers[1] = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.z));
        centers[2] = new Vector2Int(Mathf.RoundToInt(player.position.y), Mathf.RoundToInt(player.position.z));
        if (centers[0].x != lastCenters[0].x || centers[0].y != lastCenters[0].y)
        {
            borders[0].position = new Vector3(centers[0].x, centers[0].y, maxZ + 0.1f);
            borders[1].position = new Vector3(centers[0].x, centers[0].y, minZ - 0.1f);
            lastCenters[0] = centers[0];
        }
        if (centers[1].x != lastCenters[1].x || centers[1].y != lastCenters[1].y)
        {
            borders[2].position = new Vector3(centers[1].x, maxY + 0.1f, centers[1].y);
            borders[3].position = new Vector3(centers[1].x, minY - 0.1f, centers[1].y);
            lastCenters[1] = centers[1];
        }
        if (centers[2].x != lastCenters[2].x || centers[2].y != lastCenters[2].y)
        {
            borders[4].position = new Vector3(maxX + 0.1f, centers[2].x, centers[2].y);
            borders[5].position = new Vector3(minX - 0.1f, centers[2].x, centers[2].y);
            lastCenters[2] = centers[2];
        }
        for (int a = 0; a < 486; ++a)
        {
            alpha = Vector3.Distance(player.position, renderers[a].transform.position);
            if (alpha > 4f)
            {
                alpha = 0f;
            }
            else if (alpha > 0.5f)
            {
                alpha = (4f - alpha) / 5f;
            }
            else
            {
                alpha = 0.8f;
            }
            renderers[a].material.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
