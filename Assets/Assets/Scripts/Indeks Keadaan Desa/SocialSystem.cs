using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SocialGroup
{
    public string name;
    public float trust;
    public float weight;
}

public class SocialSystem : MonoBehaviour
{
    public List<SocialGroup> groups;
    public float Value;
    void Update()
    {
        float wave = Mathf.Sin(Time.time * 0.2f);
        Value = Mathf.Lerp(40f, 80f, (wave + 1f) / 2f);
    }

    public void AddTrust(string groupName, float amount)
    {
        var g = groups.Find(x => x.name == groupName);
        if (g != null)
        {
            g.trust = Mathf.Clamp(g.trust + amount, 0, 100);
            UpdateIndex();
        }
    }

    void UpdateIndex()
    {
        float total = 0;
        float weight = 0;

        foreach (var g in groups)
        {
            total += g.trust * g.weight;
            weight += g.weight;
        }

        Value = total / weight;

        Debug.Log("Social Index: " + Value);
    }
}