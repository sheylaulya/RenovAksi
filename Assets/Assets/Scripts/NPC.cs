using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour, Iinteractable
{
    public bool givesQuest;
    public QuestData questData;

    public NPCDialogue startDialogueData;
    public NPCDialogue inProgressDialogueData;
    public NPCDialogue completedDialogueData;

    private NPCDialogue dialogueData;
    private DialogueController dialogUI;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private void Start()
    {
        dialogUI = DialogueController.instance;
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (dialogUI.questPreviewPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        // Kalau ada choices aktif, space diabaikan
        if (dialogUI.choiceContainer.childCount > 0) return;

        if (isTyping)
        {
            StopAllCoroutines();
            dialogUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else
        {
            nextLine();
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (givesQuest)
        {
            HandleQuestDialogue();
        }
        else
        {
            StartDialogueWithData(startDialogueData);
        }
    }

    void HandleQuestDialogue()
    {
        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestManager belum ada di scene!");
            return;
        }

        if (questData == null)
        {
            Debug.LogError("QuestData belum di-assign di NPC!");
            return;
        }

        var state = QuestManager.instance.GetQuestState(questData);

        switch (state)
        {
            case QuestState.NotStarted:
            case QuestState.Declined:
                Debug.Log("SHOW START DIALOGUE");
                StartDialogueWithData(startDialogueData);
                break;

            case QuestState.InProgress:
                Debug.Log("QUEST MASIH BERJALAN");
                break;

            case QuestState.Completed:
                Debug.Log("QUEST SUDAH SELESAI");
                break;
        }
    }

    void StartDialogueWithData(NPCDialogue data)
    {
        dialogueData = data;
        startDialogue();
    }

    void startDialogue()
    {
        Debug.Log("Start Dialogue Dipanggil");

        isDialogueActive = true;
        dialogueIndex = 0;

        dialogUI.SetPreviewButton(false);
        if (givesQuest) dialogUI.SetCurrentQuest(questData);

        dialogUI.SetNPCInfo(
            dialogueData.npcName,
            dialogueData.npcTitleName,
            dialogueData.npcPotrait
        );

        dialogUI.showDialogueUI(true);
        displayCurrentLine();
    }

    void nextLine()
    {
        dialogUI.questPreviewPanel.SetActive(false);
        dialogUI.SetPreviewButton(false);

        if (isTyping)
        {
            StopAllCoroutines();
            dialogUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        dialogUI.ClearChoices();

        if (dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            endDialogue();
            return;
        }

        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            displayCurrentLine();
        }
        else
        {
            endDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogUI.SetDialogueText("");

        string currentText = "";

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            currentText += letter;
            dialogUI.SetDialogueText(currentText);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        // 🔥 auto progress tetap jalan
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            nextLine();
        }
    }
    void DisplayChoices(DialogueChoice choice)
    {

        Debug.Log("Choices: " + choice.choices.Length);
        Debug.Log("NextIndex: " + choice.nextDialogueIndexes.Length);
        Debug.Log("Types: " + choice.choiceTypes.Length);

        bool hasPreviewQuest = System.Array.Exists(choice.choiceTypes, t => t == ChoiceType.PreviewQuest);
        dialogUI.SetPreviewButton(givesQuest && hasPreviewQuest);

        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            ChoiceType type = choice.choiceTypes[i];

            if (type == ChoiceType.PreviewQuest)
            {
                dialogUI.CreateChoiceButton(
                    choice.choices[i],
                    () => ToggleQuestPreview()
                );
            }
            else
            {
                dialogUI.CreateChoiceButton(
                    choice.choices[i],
                    () => ChooseOption(nextIndex, type)
                );
            }
        }
    }
    void ToggleQuestPreview()
    {
        bool isActive = dialogUI.questPreviewPanel.activeSelf;

        dialogUI.questPreviewPanel.SetActive(!isActive);

        if (!isActive)
        {
            dialogUI.SetQuestPreviewData(questData);
        }
    }

    void ChooseOption(int nextIndex, ChoiceType type)
    {
        dialogUI.questPreviewPanel.SetActive(false);
        StopAllCoroutines();
        dialogUI.ClearChoices();

        if (givesQuest)
        {
            if (type == ChoiceType.AcceptQuest)
                QuestManager.instance.AcceptQuest(questData);
            else if (type == ChoiceType.DeclineQuest)
                QuestManager.instance.DeclineQuest(questData);
        }

        dialogUI.SetPlayerInfo();
        dialogUI.SetDialogueText(GetChoiceText(nextIndex));

        StartCoroutine(ContinueToNPC(nextIndex));
    }

    IEnumerator ContinueToNPC(int nextIndex)
    {
        yield return new WaitForSeconds(1.5f);

        dialogUI.SetNPCInfo(
            dialogueData.npcName,
            dialogueData.npcTitleName,
            dialogueData.npcPotrait
        );

        dialogueIndex = nextIndex;
        displayCurrentLine();
    }

    string GetChoiceText(int nextIndex)
    {
        foreach (DialogueChoice choice in dialogueData.choices)
        {
            for (int i = 0; i < choice.nextDialogueIndexes.Length; i++)
            {
                if (choice.nextDialogueIndexes[i] == nextIndex)
                    return choice.choices[i];
            }
        }
        return "";
    }

    void displayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void endDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogUI.SetDialogueText("");
        dialogUI.showDialogueUI(false);

        dialogUI.SetPreviewButton(false);
        dialogUI.questPreviewPanel.SetActive(false);
    }
}