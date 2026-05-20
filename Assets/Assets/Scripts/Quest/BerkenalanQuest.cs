using System.Collections.Generic;
using UnityEngine;

public class BerkenalanQuest : MonoBehaviour
{
    public static BerkenalanQuest instance;

    [Header("Quest Pak Harjo")]
    public QuestData questData;

    [Header("Jumlah warga yang harus diajak ngobrol")]
    public int targetCount = 2;

    private HashSet<string> talkedNPCs = new HashSet<string>();

    void Awake()
    {
        instance = this;
    }

    public void RegisterConversation(string npcName)
    {
        // quest belum diterima → jangan hitung
        if (npcName == "Pak Harjo")
        {
            return;
        }
        else
        {

            if (QuestManager.instance.GetQuestState(questData)
                != QuestState.InProgress)
            {
                return;
            }

            if (string.IsNullOrEmpty(npcName))
                return;

            // jangan hitung NPC yang sama
            if (talkedNPCs.Contains(npcName))
                return;

            talkedNPCs.Add(npcName);
        }


        Debug.Log(
            $"Progress: {talkedNPCs.Count}/{targetCount}"
        );

        if (QuestPanelUI.instance != null)
        {
            QuestPanelUI.instance.CollectibleAction("Pak Harjo");
        }

        if (IsCompleted())
        {
            Debug.Log("Quest selesai");
        }
    }

    public int GetProgress()
    {
        return talkedNPCs.Count;
    }

    public bool IsCompleted()
    {
        return talkedNPCs.Count >= targetCount;
    }
}