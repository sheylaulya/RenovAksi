using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; }

    [Header("NPC Info UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public TMP_Text titleText; // 🔥 NEW (buat npcTitleName)
    public Image potraitImage;

    [Header("Player Info")]
    public string playerName;
    public string playerTitle;
    public Sprite playerPortrait;

    [Header("Choices")]
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void showDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    // 🔥 UPDATED
    public void SetNPCInfo(string npcName, string npcTitle, Sprite potrait)
    {
        nameText.text = npcName;

        // handle title kosong
        if (string.IsNullOrEmpty(npcTitle))
        {
            titleText.gameObject.SetActive(false);
        }
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
        {
            titleText.gameObject.SetActive(false);
        }
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
}