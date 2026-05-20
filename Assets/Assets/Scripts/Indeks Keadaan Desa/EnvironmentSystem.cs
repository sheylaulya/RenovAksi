using UnityEngine;

public class EnvironmentSystem : MonoBehaviour
{
    public float Value;

    public void ApplyQuestImpact(QuestData quest)
    {
        float result = quest.impact.environment * quest.impact.quality;

        Value = Mathf.Clamp(Value + result, 0, 100);
    }
}