using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public string npcName;
    public bool isCollectible;

    [Header("Index Impact")]
    public IndexImpact impact;

    [Header("Trust Reward")]
    public List<TrustReward> trustRewards;
}