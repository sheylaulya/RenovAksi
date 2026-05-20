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

    public SocialSystem socialSystem;

    public void AddTrust(string npcName, float amount)
    {
        var npc = GetNPC(npcName);

        if (npc != null)
        {
            npc.AddTrust(amount);

            socialSystem.UpdateIndex();
        }
    }

    public void RemoveTrust(string npcName, float amount)
    {
        var npc = GetNPC(npcName);

        if (npc != null)
        {
            npc.RemoveTrust(amount);

            socialSystem.UpdateIndex();
        }
    }
}