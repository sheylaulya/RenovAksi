// InteractionDetector.cs
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private Iinteractable interactableInRange = null;
    public GameObject interactionIcon;

    [Header("Tutorial")]
    public GameObject tutorialPanel;          // assign di Inspector
    private bool hasTutorialBeenShown = false; // flag, reset tiap scene

    void Start()
    {
        interactionIcon.SetActive(false);

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    void Update()
    {
        if (interactableInRange != null)
            interactionIcon.SetActive(interactableInRange.CanInteract());

        if (Input.GetKeyDown(KeyCode.E) && interactableInRange != null)
        {
            if (interactableInRange.CanInteract())
            {
                interactableInRange.Interact();

                if (tutorialPanel != null)
                {
                    tutorialPanel.SetActive(false);
                    hasTutorialBeenShown = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Iinteractable interactable))
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(interactable.CanInteract());

            // Tampilkan tutorial hanya jika belum pernah shown
            if (tutorialPanel != null && !hasTutorialBeenShown)
                tutorialPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Iinteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);

            // Sembunyikan tutorial saat keluar range
            if (tutorialPanel != null)
                tutorialPanel.SetActive(false);
        }
    }

    public void ResetDetector()
    {
        interactableInRange = null;
        interactionIcon.SetActive(false);

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }
}