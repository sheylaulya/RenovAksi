using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPC : MonoBehaviour, Iinteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private void Start()
    {
        dialogUI = DialogueController.instance;
    }
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null)
        {
            return;
        }
        if (isDialogueActive)
        {
            nextLine();
        }
        else
        {
            startDialogue();
        }
    }

    void startDialogue()
    {
        Debug.Log(dialogUI);
        Debug.Log(dialogueData);
        Debug.Log(dialogueData.dialogueLines);
        isDialogueActive = true;
        dialogueIndex = 0;

        dialogUI.SetNPCInfo(
            dialogueData.npcName,
            dialogueData.npcTitleName,
            dialogueData.npcPotrait
        );

        dialogUI.showDialogueUI(true);

        displayCurrentLine();
        StartCoroutine(TypeLine());
    }

    void nextLine()
    {
        Debug.Log("Current Index: " + dialogueIndex);
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

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            nextLine();
        }
    }
    void DisplayChoices(DialogueChoice choice)
    {
        for (var i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    void ChooseOption(int nextIndex)
    {
        StopAllCoroutines();
        dialogUI.ClearChoices();

        // 🔥 tampilkan player dulu
        dialogUI.SetPlayerInfo();
        dialogUI.SetDialogueText(GetChoiceText(nextIndex));

        StartCoroutine(ContinueToNPC(nextIndex));
    }

    string GetChoiceText(int nextIndex)
    {
        foreach (DialogueChoice choice in dialogueData.choices)
        {
            for (int i = 0; i < choice.nextDialogueIndexes.Length; i++)
            {
                if (choice.nextDialogueIndexes[i] == nextIndex)
                {
                    return choice.choices[i];
                }
            }
        }
        return "";
    }

    IEnumerator ContinueToNPC(int nextIndex)
    {
        yield return new WaitForSeconds(1.5f); // delay biar keliatan player ngomong

        // balik ke NPC
        dialogUI.SetNPCInfo(
            dialogueData.npcName,
            dialogueData.npcTitleName,
            dialogueData.npcPotrait
        );

        dialogueIndex = nextIndex;
        displayCurrentLine();
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

    }
}
