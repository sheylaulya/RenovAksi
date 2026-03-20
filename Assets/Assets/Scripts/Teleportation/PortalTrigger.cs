using UnityEngine;
using System.Collections.Generic;

public class PortalTrigger : MonoBehaviour
{
    [Header("Portal Settings")]
    public string portalDisplayName = "Mystic Portal";
    public List<PortalDestination> destinations = new List<PortalDestination>();

    [Header("Cooldown")]
    public float cooldown = 2f;

    private float lastTeleportTime = -Mathf.Infinity;
    private bool playerInRange = false;
    private GameObject playerInTrigger;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerInTrigger = collision.gameObject;
            TeleportUI.Instance.ShowUI(this, collision.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            playerInTrigger = null;
            TeleportUI.Instance.HideUI(); // This now also clears buttons
        }
    }

    public bool CanTeleport()
    {
        return Time.time >= lastTeleportTime + cooldown;
    }

    public void Teleport(Transform player, PortalDestination dest)
    {
        if (!CanTeleport()) return;

        lastTeleportTime = Time.time;
        player.position = dest.arrivalPoint.position;
        TeleportUI.Instance.HideUI();

        // Brief cooldown visual feedback (optional)
        StartCoroutine(CooldownRoutine());
    }

    private System.Collections.IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        // Re-show UI if player is still in range
        if (playerInRange && playerInTrigger != null)
        {
            TeleportUI.Instance.ShowUI(this, playerInTrigger.transform);
        }
    }
}