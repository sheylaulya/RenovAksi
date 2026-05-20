using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private Dictionary<string, QuestInstance> questDictionary = new Dictionary<string, QuestInstance>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("TEST QUEST COMPLETE");

            foreach (var q in questDictionary.Values)
            {
                CompleteQuest(q.data);
                break;
            }
        }
    }

    public QuestInstance GetQuest(QuestData data)
    {
        if (!questDictionary.ContainsKey(data.questID))
        {
            questDictionary[data.questID] = new QuestInstance(data);
        }

        return questDictionary[data.questID];
    }

    public QuestState GetQuestState(QuestData data)
    {
        return GetQuest(data).state;
    }

    void SaveGameState()
    {
        SaveController save = FindObjectOfType<SaveController>();

        if (save != null)
        {
            save.SaveGame();

            Debug.Log("Quest state disimpan");
        }
    }

    public void AcceptQuest(QuestData data)
    {
        var quest = GetQuest(data);
        quest.state = QuestState.InProgress;
        Debug.Log("Quest Accepted: " + data.questID + " - " + data.questName);
        SaveGameState();
    }

    public void DeclineQuest(QuestData data)
    {
        var quest = GetQuest(data);
        quest.state = QuestState.Declined;
        Debug.Log("Quest Declined: " + data.questID + " - " + data.questName);
        SaveGameState();
    }

    public void CompleteQuest(QuestData data)
    {
        var quest = GetQuest(data);

        quest.state = QuestState.Completed;

        Debug.Log("Quest Completed: " + data.questName);

        if (QuestPanelUI.instance != null)
        {
            QuestPanelUI.instance.HideQuest();
        }

        SaveGameState();
    }

    public void ShowQuestPanel(QuestData data)
    {
        if (QuestPanelUI.instance != null)
        {
            QuestPanelUI.instance.ShowQuest(data);
        }
    }

    public List<QuestSaveData> GetSaveData()
    {
        List<QuestSaveData> list = new();

        foreach (var quest in questDictionary.Values)
        {
            list.Add(new QuestSaveData
            {
                questID = quest.data.questID,
                state = quest.state
            });
        }

        return list;
    }
    public void LoadFromSaveData(List<QuestSaveData> savedQuests)

    {
        Debug.Log("=== LOAD QUEST START ===");

        foreach (var q in savedQuests)
        {
            Debug.Log($"Loading {q.questID} state {q.state}");
        }

        questDictionary.Clear();

        foreach (var q in savedQuests)
        {
            QuestData data = Resources.Load<QuestData>("Quests/" + q.questID);

            if (data != null)
            {
                QuestInstance instance = new QuestInstance(data);
                instance.state = q.state;

                questDictionary[data.questID] = instance;
            }
        }
    }
}