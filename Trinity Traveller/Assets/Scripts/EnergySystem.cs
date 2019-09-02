using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public float currentEnergy;
    public Image imageEnergy;
    public float maxEnergy;
    public Color normalColor;
    public Text textEnergy;
    public Color warningColor;

    private void Update()
    {
        if (currentEnergy > maxEnergy * 0.3f)  // 
        {
            imageEnergy.color = Color.Lerp(imageEnergy.color, normalColor, Time.deltaTime * 6f);
        }
        else
        {
            imageEnergy.color = Color.Lerp(imageEnergy.color, warningColor, Time.deltaTime);
        }
        imageEnergy.fillAmount = currentEnergy / maxEnergy;
        textEnergy.text = Mathf.RoundToInt(currentEnergy).ToString() + " / " + Mathf.RoundToInt(maxEnergy).ToString();
    }
}
