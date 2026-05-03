using UnityEngine;

public class WaterSystem : MonoBehaviour
{
    public float Value;

    [Header("Natural")]
    public float baseTide = 10f;
    public float rainfall;

    [Header("Player Impact")]
    public float infrastructure;
    public float education;

    void Update()
    {
        float wave = Mathf.Sin(Time.time * 0.5f);
        float noise = Random.Range(-5f, 5f);

        Value = Mathf.Lerp(20f, 80f, (wave + 1f) / 2f) + noise;
        Value = Mathf.Clamp(Value, 0, 100);
    }


    public void UpdateWaterDaily(DayTimeCycle time)
    {
        Debug.Log("Water Index: " + Value);
        float seasonFactor = GetSeasonFactor(time.days);

        float natural = baseTide + rainfall + seasonFactor;
        float mitigation = infrastructure + education;

        Value = Mathf.Clamp(natural - mitigation, 0, 100);

        Debug.Log("Water Index: " + Value);
    }

    float GetSeasonFactor(int day)
    {
        // simple: tiap 30 hari ganti musim
        int season = (day / 30) % 2;

        return season == 0 ? 10f : -5f; // hujan vs kemarau
    }
}