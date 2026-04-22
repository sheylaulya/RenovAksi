using System.Collections.Generic;

[System.Serializable]
public class TutorialStepData
{
    public string title;

    public string instructionText;
    public List<InputType> allowedInputs;
    public InputType requiredInput;
    public TutorialPanelType panelType;
}