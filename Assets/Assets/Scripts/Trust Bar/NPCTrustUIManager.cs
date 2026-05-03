using System.Collections.Generic;
using UnityEngine;

public class NPCTrustUIManager : MonoBehaviour
{
    public NpcTrustManagers trustManager;

    public Transform contentParent; // isi dengan "Content"
    public GameObject itemPrefab;

    private List<NPCTrustItemUI> uiItems = new List<NPCTrustItemUI>();

    void Start()
    {
        foreach (var npc in trustManager.GetAllNPC())
        {
            GameObject obj = Instantiate(itemPrefab, contentParent);
            NPCTrustItemUI ui = obj.GetComponent<NPCTrustItemUI>();

            ui.Setup(npc);
            uiItems.Add(ui);
        }
    }

    void Update()
    {
        foreach (var ui in uiItems)
        {
            ui.UpdateUI();
        }
    }
}