using System.IO;
using UnityEngine;
using SaveGame.Data;
using System.Collections;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private inventoryController inventoryController;
    public int lastFishingResult;
    public static SaveController instance;
    private DayTimeCycle dayTimeCycle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    IEnumerator Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log($"Save Path: {saveLocation}");

        inventoryController = FindAnyObjectByType<inventoryController>();

        if (inventoryController == null)
        {
            Debug.LogError("inventoryController NOT FOUND!");
        }

        dayTimeCycle = FindAnyObjectByType<DayTimeCycle>();

        yield return null;

        LoadGame();
    }

    public void SaveGame()
    {
        Debug.Log("=== SAVE GAME CALLED ===");

        SaveData data = new()
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            quests = QuestManager.instance.GetSaveData(),
            inventorySaveDatas = inventoryController.GetInventoryItems(),

            mancingSaveData = FishingGameManager.instance != null
                ? FishingGameManager.instance.GetSaveData()
                : new MancingSaveData(),

            // Simpan waktu
            timeData = new TimeSaveData()
            {
                hours = dayTimeCycle.hours,
                mins = dayTimeCycle.mins,
                days = dayTimeCycle.days,
                tick = dayTimeCycle.tick
            }
        };

        string json = JsonUtility.ToJson(data, true);

        Debug.Log("SAVE JSON:\n" + json);

        File.WriteAllText(saveLocation, json);

        Debug.Log("GAME SAVED SUCCESSFULLY");
    }

    public void SaveFishingResult()
    {
        SaveData data;

        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            data = new SaveData();
        }

        data.mancingSaveData =
            FishingGameManager.instance.GetSaveData();

        File.WriteAllText(
            saveLocation,
            JsonUtility.ToJson(data, true)
        );
    }

    public void LoadGame()
    {
        Debug.Log("=== LOAD GAME CALLED ===");

        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            GameObject.FindGameObjectWithTag("Player").transform.position =
                data.playerPosition;

            QuestManager.instance.LoadFromSaveData(data.quests);

            inventoryController.SetInventoryItems(data.inventorySaveDatas);

            lastFishingResult =
                data.mancingSaveData.lastFishingResult;

            if (FishingGameManager.instance != null)
            {
                FishingGameManager.instance.LoadFromSaveData(data.mancingSaveData);
            }

            dayTimeCycle.LoadTime(data.timeData);
        }
        else
        {
            Debug.LogWarning("SAVE FILE NOT FOUND → Creating new save");
            SaveGame();
        }
    }
}