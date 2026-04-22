using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public TutorialData movementTutorial;

    private int currentStepIndex = 0;
    private TutorialStepData currentStep;
    public TutorialPanelUI movementUI;
    public TutorialPanelUI dialogUI;
    public TutorialPanelUI shopUI;
    public TutorialPanelUI teleportUI;
    private TutorialData currentTutorial;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartTutorial(movementTutorial);
    }

    public void StartTutorial(TutorialData tutorial)
    {
        currentTutorial = tutorial;
        currentStepIndex = 0;
        LoadStep();
    }
    IEnumerator EnableStepAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // BUKA LOCK
        InputManager.Instance.UnlockAllInput();

        // izinkan input step ini
        InputManager.Instance.AllowTemporaryInputs(currentStep.allowedInputs);
    }

    void LoadStep()
    {
        currentStep = currentTutorial.steps[currentStepIndex];

        // lock semua dulu
        InputManager.Instance.LockAllInput();

        ShowInstruction(currentStep);

        StartCoroutine(EnableStepAfterDelay(0.5f));
    }

    public void ReportInput(InputType input)
    {
        if (input == currentStep.requiredInput)
        {
            CompleteStep();
        }
    }

    void CompleteStep()
    {
        // unlock permanent
        InputManager.Instance.UnlockInput(currentStep.requiredInput);

        currentStepIndex++;

        if (currentStepIndex >= currentTutorial.steps.Count)
        {
            FinishTutorial();
            return;
        }

        LoadStep();
    }
    void FinishTutorial()
    {
        Debug.Log("Tutorial selesai!");

        // Unlock semua input
        InputManager.Instance.SetAllowedInputs(
            new System.Collections.Generic.List<InputType>
            {
                InputType.MoveLeft,
                InputType.MoveRight,
                InputType.Run
            }
        );

        HideInstruction();
    }

    void ShowInstruction(TutorialStepData step)
    {
        HideInstruction();

        TutorialPanelUI ui = null;

        switch (step.panelType)
        {
            case TutorialPanelType.Movement:
                ui = movementUI;
                break;
            case TutorialPanelType.Dialog:
                ui = dialogUI;
                break;
            case TutorialPanelType.Shop:
                ui = shopUI;
                break;
            case TutorialPanelType.Teleport:
                ui = teleportUI;
                break;
        }

        if (ui != null)
        {
            ui.panel.SetActive(true);
            ui.titleText.text = step.title;
            ui.instructionText.text = step.instructionText;
        }
    }
    void HideInstruction()
    {
        movementUI.panel.SetActive(false);
        dialogUI.panel.SetActive(false);
        shopUI.panel.SetActive(false);
        teleportUI.panel.SetActive(false);
    }


}