using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private HashSet<InputType> allowedInputs = new HashSet<InputType>();
    private bool inputLocked = false;
    private HashSet<InputType> unlockedInputs = new HashSet<InputType>();

    private HashSet<InputType> temporaryAllowedInputs = new HashSet<InputType>();

    public void AllowTemporaryInputs(List<InputType> inputs)
    {
        temporaryAllowedInputs = new HashSet<InputType>(inputs);
    }
    public void LockAllInput()
    {
        inputLocked = true;
    }


    public void UnlockAllInput()
    {
        inputLocked = false;
    }
    public void UnlockInput(InputType input)
    {
        unlockedInputs.Add(input);
    }

    public void UnlockAllInputTemporary(List<InputType> inputs)
    {
        foreach (var input in inputs)
        {
            unlockedInputs.Add(input);
        }
    }
    public bool IsAllowed(InputType input)
    {
        if (inputLocked) return false;

        // kalau sudah permanent unlock → boleh
        if (unlockedInputs.Contains(input))
            return true;

        // kalau step sekarang → boleh
        if (temporaryAllowedInputs.Contains(input))
            return true;

        return false;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SetAllowedInputs(List<InputType> inputs)
    {
        allowedInputs = new HashSet<InputType>(inputs);
    }

}