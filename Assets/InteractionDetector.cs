// InteractionDetector.cs
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private Iinteractable interactableInRange = null;
    public GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    void Update()
    {
        if (interactableInRange != null)
            interactionIcon.SetActive(interactableInRange.CanInteract());

        if (Input.GetKeyDown(KeyCode.E) && interactableInRange != null)
        {
            if (interactableInRange.CanInteract())
                interactableInRange.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Iinteractable interactable))
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(interactable.CanInteract());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Iinteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }

    // ✅ FIX 2: Reset detector saat teleport agar portal baru bisa ke-detect
    public void ResetDetector()
    {
        interactableInRange = null;
        interactionIcon.SetActive(false);
    }
}