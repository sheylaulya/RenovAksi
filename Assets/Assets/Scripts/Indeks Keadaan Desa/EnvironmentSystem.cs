using UnityEngine;

public class EnvironmentSystem : MonoBehaviour
{
    public float Value;
    public float dailyDecay = 2f;

    public void ApplyQuestImpact(QuestData quest)
    {
        float result =
            quest.impact.environment *
            quest.impact.quality;

        Value = Mathf.Clamp(
            Value + result,
            0,
            100
        );

        Debug.Log("Environment: " + Value);
    }

    public void UpdateEnvironmentDaily()
    {
        Value = Mathf.Clamp(
            Value - dailyDecay,
            0,
            100
        );

        Debug.Log("Environment menurun: " + Value);
    }
}