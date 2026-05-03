using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;

    [Header("Index Impact")]
    public IndexImpact impact;
}