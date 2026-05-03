using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndexUI : MonoBehaviour
{
    public Slider waterBar;
    public TMP_Text waterValueText;
    public Slider envBar;
    public TMP_Text envValueText;

    public Slider socialBar;
    public TMP_Text socialValueText;

    void Update()
    {

        Debug.Log("IndexUI jalan");

        if (IndexManager.instance == null)
        {
            Debug.Log("IndexManager NULL");
            return;
        }


        waterBar.value += Time.deltaTime * 50f;
        Debug.Log("Water Value: " + IndexManager.instance.water.Value);

        if (IndexManager.instance == null) return;

        waterBar.value = Mathf.Lerp(
            waterBar.value,
            IndexManager.instance.water.Value,
            Time.deltaTime * 5f
        );
        waterValueText.text = Mathf.RoundToInt(waterBar.value) + "%";

        envBar.value = Mathf.Lerp(
            envBar.value,
            IndexManager.instance.environment.Value,
            Time.deltaTime * 5f
        );
        envValueText.text = Mathf.RoundToInt(envBar.value) + "%";

        socialBar.value = Mathf.Lerp(
            socialBar.value,
            IndexManager.instance.social.Value,
            Time.deltaTime * 5f
        );
        socialValueText.text = Mathf.RoundToInt(socialBar.value) + "%";
    }
}