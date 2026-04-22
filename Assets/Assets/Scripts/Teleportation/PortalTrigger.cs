// PortalTrigger.cs
using UnityEngine;
using System.Collections.Generic;

public class PortalTrigger : MonoBehaviour, Iinteractable
{
    [Header("Portal Settings")]
    public string portalDisplayName = "Mystic Portal";
    public List<PortalDestination> destinations = new List<PortalDestination>();

    [Header("Cooldown")]
    public float cooldown = 2f;

    private float lastTeleportTime = -Mathf.Infinity;
    private bool playerInRange = false;
    private Transform playerTransform;
    public GameObject tutorialPanel;
    private static bool sudahPernahBuka = false;

    public bool CanInteract() => CanTeleport() && playerInRange;

    public void Interact()
    {

        if (!sudahPernahBuka)
        {
            tutorialPanel.SetActive(true);
            sudahPernahBuka = true;
        }

        if (playerTransform == null) return;
        TeleportUI.Instance.ShowUI(this, playerTransform);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = collision.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
            TeleportUI.Instance.HideUI();
            tutorialPanel.SetActive(false);

        }
    }

    public bool CanTeleport() => Time.time >= lastTeleportTime + cooldown;

    // PortalTrigger.cs — update Teleport() untuk panggil ResetDetector
    public void Teleport(Transform player, PortalDestination dest)
    {
        if (!CanTeleport()) return;

        lastTeleportTime = Time.time;

        // Reset detector SEBELUM pindah posisi
        var detector = player.GetComponentInChildren<InteractionDetector>();
        detector?.ResetDetector();

        player.position = dest.arrivalPoint.position;

        // Paksa Unity re-evaluasi semua trigger setelah teleport
        Physics2D.SyncTransforms();

        TeleportUI.Instance.HideUI();
    }
}