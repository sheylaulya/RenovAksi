using UnityEngine;

[System.Serializable]
public class NPCTrustRuntime
{
    public NPCTrustData data;
    public float currentTrust;

    public NPCTrustRuntime(NPCTrustData data)
    {
        this.data = data;
        this.currentTrust = data.initialTrust;
    }

    public void AddTrust(float amount)
    {
        currentTrust = Mathf.Clamp(currentTrust + amount, 0, data.maxTrust);
    }

    public void RemoveTrust(float amount)
    {
        currentTrust = Mathf.Clamp(currentTrust - amount, 0, data.maxTrust);
    }
}