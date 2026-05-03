using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCTrustItemUI : MonoBehaviour
{
    public Image npcIcon;
    public Slider trustSlider;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI percentText;

    private NPCTrustRuntime npcData;

    public void Setup(NPCTrustRuntime data)
    {
        npcData = data;

        npcNameText.text = data.data.npcName;
        npcIcon.sprite = data.data.spriteNPC;

        trustSlider.maxValue = data.data.maxTrust;
        UpdateUI();
    }

    public void UpdateUI()
    {
        trustSlider.value = npcData.currentTrust;

        float percent = npcData.currentTrust / npcData.data.maxTrust * 100f;
        percentText.text = percent.ToString("0") + "%";
    }
}