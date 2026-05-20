using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanelUI : MonoBehaviour
{
    public static QuestPanelUI instance;

    [Header("Panel")]
    public GameObject panel;

    [Header("Text")]
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public Button actionButton;
    public Button successMarkButton;
    public TextMeshProUGUI collectibleText;

    QuestData questData;
    public QuestPrize questPrize;

    void Awake()
    {
        instance = this;

        // panel awal disembunyikan
        panel.SetActive(false);
    }

    public void ShowQuest(QuestData quest)
    {
        questData = quest; // simpan quest yang sedang aktif

        panel.SetActive(true);

        questTitle.text = quest.questName;
        questDescription.text = quest.description;

        if (quest.isCollectible)
        {
            collectibleText.gameObject.SetActive(true);
            actionButton.gameObject.SetActive(false);
            CollectibleAction(questData.npcName);
        }
        else
        {
            actionButton.gameObject.SetActive(true);
            collectibleText.gameObject.SetActive(false);
        }
    }

    public void HideQuest()
    {
        panel.SetActive(false);
    }

    public void OnActionButtonClicked()
    {
        Debug.Log("Quest Completed: " + questTitle.text);

        HideQuest();

        if (questData.npcName == "Bu Dita")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Quiz");
        }
    }

    public void CollectibleAction(string NPCName)
    {
        if (NPCName == "Pak Harjo")
        {
            int progress =
                BerkenalanQuest.instance.GetProgress();

            int total =
                BerkenalanQuest.instance.targetCount;

            collectibleText.text =
                $"Ngobrol dengan warga: {progress}/{total}";

            if (BerkenalanQuest.instance.IsCompleted())
            {
                collectibleText.text =
                    "Ngobrol dengan warga: Selesai";

                successMarkButton.gameObject.SetActive(true);
                collectibleText.gameObject.SetActive(false);

            }

        }
    }

    public void OnSuccessMarkButtonClicked()
    {
        questPrize.GiveReward(questData);
        panel.SetActive(false);
    }
}