using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; }

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public TMP_Text titleText;
    public Image potraitImage;
    public GameObject previewButton;
    public GameObject questPreviewPanel;
    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    private QuestData currentQuest;

    [Header("Choices")]
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    [Header("Player Info")]
    public string playerName;
    public string playerTitle;
    public Sprite playerPortrait;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // 🔥 PINDAH KE SINI
        dialoguePanel.SetActive(false);
        previewButton.SetActive(false);
        questPreviewPanel.SetActive(false);


        Debug.Log("DialogueController Awake");
        previewButton.SetActive(false);
    }

    public void SetPreviewButton(bool state)
    {
        Debug.Log("SET PREVIEW BUTTON: " + state);
        previewButton.SetActive(state);
    }

    public void showDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetCurrentQuest(QuestData quest)
    {
        currentQuest = quest;
    }

    public void SetNPCInfo(string npcName, string npcTitle, Sprite potrait)
    {
        nameText.text = npcName;

        if (string.IsNullOrEmpty(npcTitle))
            titleText.gameObject.SetActive(false);
        else
        {
            titleText.gameObject.SetActive(true);
            titleText.text = npcTitle;
        }

        potraitImage.sprite = potrait;
    }

    public void SetPlayerInfo()
    {
        nameText.text = playerName;

        if (string.IsNullOrEmpty(playerTitle))
            titleText.gameObject.SetActive(false);
        else
        {
            titleText.gameObject.SetActive(true);
            titleText.text = playerTitle;
        }

        potraitImage.sprite = playerPortrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform item in choiceContainer)
        {
            Destroy(item.gameObject);
        }
    }

    public GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
        return choiceButton;
    }

    public void SetQuestPreviewData(QuestData quest)
    {
        questTitleText.text = quest.questName;
        questDescriptionText.text = quest.description;
    }

    public void ToggleQuestPreview()
    {
        bool isActive = questPreviewPanel.activeSelf;

        questPreviewPanel.SetActive(!isActive);

        if (!isActive)
        {
            SetQuestPreviewData(currentQuest);
        }
    }
}