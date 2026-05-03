using UnityEngine;
using UnityEngine.UI;

public class NPCTrustUI : MonoBehaviour
{
    public Slider trustSlider;
    public NPCTrustRuntime npc;

    void Update()
    {
        if (npc != null)
        {
            trustSlider.maxValue = npc.data.maxTrust;
            trustSlider.value = npc.currentTrust;
        }
    }
}