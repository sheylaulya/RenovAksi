using UnityEngine;

public class EnvironmentSystem : MonoBehaviour
{
    public float Value;
    void Update()
    {
        float wave = Mathf.Sin(Time.time * 0.2f);
        Value = Mathf.Lerp(40f, 80f, (wave + 1f) / 2f);
    }
    public void ApplyQuestImpact(QuestData quest)
    {
        float result = quest.impact.environment * quest.impact.quality;

        Value = Mathf.Clamp(Value + result, 0, 100);
    }
}