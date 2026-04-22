using UnityEngine;

public class TokoController : MonoBehaviour, Iinteractable
{
    private bool playerInRange = false;
    public GameObject storePanel;
    public GameObject tutorialPanel;

    private static bool sudahPernahBuka = false;

    public bool CanInteract() => playerInRange;

    public void Interact()
    {
        storePanel.SetActive(true);

        if (!sudahPernahBuka)
        {
            tutorialPanel.SetActive(true);
            sudahPernahBuka = true;
        }
    }

    public void CloseUI()
    {
        tutorialPanel.SetActive(false);
        storePanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            CloseUI();
        }
    }
}