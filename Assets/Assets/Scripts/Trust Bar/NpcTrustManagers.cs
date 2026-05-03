using System.Collections.Generic;
using UnityEngine;

public class NpcTrustManagers : MonoBehaviour
{
    public List<NPCTrustData> npcDatas;

    private List<NPCTrustRuntime> npcRuntimes = new List<NPCTrustRuntime>();

    void Start()
    {
        foreach (var data in npcDatas)
        {

            npcRuntimes.Add(new NPCTrustRuntime(data));
        }
    }

    public List<NPCTrustRuntime> GetAllNPC()
    {
        return npcRuntimes;
    }

    public NPCTrustRuntime GetNPC(string npcName)
    {
        return npcRuntimes.Find(npc => npc.data.npcName == npcName);
    }

    public void AddTrust(string npcName, float amount)
    {
        var npc = GetNPC(npcName);
        if (npc != null)
        {
            npc.AddTrust(amount);
        }
    }

    public void RemoveTrust(string npcName, float amount)
    {
        var npc = GetNPC(npcName);
        if (npc != null)
        {
            npc.RemoveTrust(amount);
        }
        else
        {
            Debug.LogWarning("NPC dengan nama " + npcName + " tidak ditemukan!");
        }
    }
}