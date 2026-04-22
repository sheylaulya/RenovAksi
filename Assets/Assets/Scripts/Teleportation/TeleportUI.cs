using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TeleportUI : MonoBehaviour
{
    public static TeleportUI Instance;

    [Header("UI References")]
    public GameObject panel;                  // The popup panel
    public TextMeshProUGUI titleText;         // Portal name label
    public Transform buttonContainer;        // Vertical layout group parent
    public GameObject buttonPrefab;          // Button prefab with TMP label
    public Button closeButton;

    private PortalTrigger currentPortal;
    private Transform currentPlayer;
    private List<GameObject> spawnedButtons = new List<GameObject>();


    [Header("Layout")]
    public float buttonSpacing = 10f;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
        closeButton.onClick.AddListener(HideUI);

        var layout = buttonContainer.GetComponent<VerticalLayoutGroup>();
        if (layout != null) layout.spacing = buttonSpacing;
    }

    public void ShowUI(PortalTrigger portal, Transform player)
    {
        if (!portal.CanTeleport()) return;

        // Don't re-spawn if already showing this exact portal
        if (panel.activeSelf && currentPortal == portal) return;

        // Always wipe before rebuilding
        HideUI();

        currentPortal = portal;
        currentPlayer = player;

        titleText.text = portal.portalDisplayName;

        foreach (var dest in portal.destinations)
        {
            var btnObj = Instantiate(buttonPrefab, buttonContainer);
            btnObj.SetActive(true); // ensure it's visible (since prefab is hidden)
            var label = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (label) label.text = dest.portalName;

            var destCopy = dest;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentPortal.Teleport(currentPlayer, destCopy);
            });

            spawnedButtons.Add(btnObj);
        }

        panel.SetActive(true);
    }

    public void HideUI()
    {
        // Always destroy spawned buttons on hide
        foreach (var btn in spawnedButtons)
        {
            if (btn != null) Destroy(btn);
        }
        spawnedButtons.Clear();

        panel.SetActive(false);
        currentPortal = null;
        currentPlayer = null;
    }
}