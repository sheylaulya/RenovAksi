using TMPro;
using UnityEngine;

public class MancingTeleport : MonoBehaviour
{
    public GameObject mancingTeleportPanel;

    // Data global antar scene
    public static int mancingResultData;
    public TMP_Text resultText;

    private bool loaded = false;

    private void Update()
    {

        // if (!loaded && resultText != null && SaveController.instance != null)
        // {
        //     Debug.Log("Loading last fishing result: " + SaveController.instance.lastFishingResult);
        resultText.text =
            "Hasil Mancing Terakhir: " +
            SaveController.instance.lastFishingResult +
            " ikan";

        loaded = true;
        // }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (mancingTeleportPanel != null)
                mancingTeleportPanel.SetActive(true);
        }
    }

    public void TeleportToMancing()
    {
        Debug.Log("Teleporting to Mancing Area...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 originalPos = player.transform.position;

        player.transform.position = new Vector3(
            originalPos.x - 2f,
            originalPos.y,
            originalPos.z
        );

        SaveController save = FindObjectOfType<SaveController>();
        save.SaveGame();

        player.transform.position = originalPos;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Fishing");
    }

    public void TeleportBackToDesa()
    {
        SaveController save = FindObjectOfType<SaveController>();

        save.SaveFishingResult();

        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
    }
}