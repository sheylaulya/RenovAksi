using System.IO;
using UnityEngine;
using SaveGame.Data;
using System.Collections;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private inventoryController inventoryController;

    IEnumerator Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log($"Save Path: {saveLocation}");

        inventoryController = FindAnyObjectByType<inventoryController>();

        if (inventoryController == null)
        {
            Debug.LogError("inventoryController NOT FOUND!");
        }

        yield return null; // 🔥 IMPORTANT: tunggu semua Awake & Start selesai

        LoadGame();
    }

    public void SaveGame()
    {
        Debug.Log("=== SAVE GAME CALLED ===");

        SaveData data = new()
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            quests = QuestManager.instance.GetSaveData(),
            inventorySaveDatas = inventoryController.GetInventoryItems()
        };

        string json = JsonUtility.ToJson(data, true);

        Debug.Log("SAVE JSON:\n" + json);

        File.WriteAllText(saveLocation, json);

        Debug.Log("GAME SAVED SUCCESSFULLY");
    }

    public void LoadGame()
    {
        Debug.Log("=== LOAD GAME CALLED ===");

        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);
            Debug.Log("LOADED JSON:\n" + json);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            GameObject.FindGameObjectWithTag("Player").transform.position = data.playerPosition;

            QuestManager.instance.LoadFromSaveData(data.quests);

            inventoryController.SetInventoryItems(data.inventorySaveDatas);
        }
        else
        {
            Debug.LogWarning("SAVE FILE NOT FOUND → Creating new save");
            SaveGame();
        }
    }
}