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

    public void AcceptQuest(QuestData data)
    {
        var quest = GetQuest(data);
        quest.state = QuestState.InProgress;
        Debug.Log("Quest Accepted: " + data.questName);
    }

    public void DeclineQuest(QuestData data)
    {
        var quest = GetQuest(data);
        quest.state = QuestState.Declined;
        Debug.Log("Quest Declined: " + data.questName);
    }

    public void CompleteQuest(QuestData data)
    {
        var quest = GetQuest(data);
        quest.state = QuestState.Completed;
        Debug.Log("Quest Completed: " + data.questName);
    }
}