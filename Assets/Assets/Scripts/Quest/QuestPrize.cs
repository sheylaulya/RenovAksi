using UnityEngine;

public class QuestPrize : MonoBehaviour
{
    private bool rewardGiven = false;

    private NpcTrustManagers trustManager;

    void Start()
    {
        trustManager = FindObjectOfType<NpcTrustManagers>();
    }

    public void GiveReward(QuestData questData)
    {
        if (rewardGiven) return;
        if (questData == null) return;
        if (trustManager == null)
        {
            Debug.LogError("NpcTrustManagers tidak ditemukan");
            return;
        }

        rewardGiven = true;

        foreach (var reward in questData.trustRewards)
        {
            trustManager.AddTrust(
                reward.npcName,
                reward.amount
            );

            Debug.Log(
                $"{reward.npcName} +{reward.amount} trust"
            );
        }
    }
}