using UnityEngine;
using UnityEngine.UI;

public class CircularIndexUI : MonoBehaviour
{
    public Image waterCircle;
    public Image envCircle;
    public Image socialCircle;

    void Update()
    {
        if (IndexManager.instance == null) return;

        waterCircle.fillAmount = IndexManager.instance.water.Value / 100f;
        envCircle.fillAmount = IndexManager.instance.environment.Value / 100f;
        socialCircle.fillAmount = IndexManager.instance.social.Value / 100f;
    }
}