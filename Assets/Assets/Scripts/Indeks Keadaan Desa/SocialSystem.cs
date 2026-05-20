using UnityEngine;

public class SocialSystem : MonoBehaviour
{
    public NpcTrustManagers npcManager;

    public float Value;

    public void UpdateIndex()
    {
        var npcs = npcManager.GetAllNPC();

        if (npcs.Count == 0)
        {
            Value = 0;
            return;
        }

        float totalTrust = 0;

        foreach (var npc in npcs)
        {
            totalTrust += npc.currentTrust;
        }

        Value = totalTrust / npcs.Count;

        Debug.Log("Social Index: " + Value);
    }
}