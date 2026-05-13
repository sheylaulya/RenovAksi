using UnityEngine;

public class MancingTeleport : MonoBehaviour
{
    public GameObject mancingTeleportPanel;

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

        // Simpan posisi sekarang
        Vector3 originalPos = player.transform.position;

        // Geser sedikit ke kiri untuk data save
        player.transform.position = new Vector3(
            originalPos.x - 2f,
            originalPos.y,
            originalPos.z
        );

        SaveController save = FindObjectOfType<SaveController>();
        save.SaveGame();

        Debug.Log("SAVE DIPANGGIL");

        // Balikin lagi posisi player aslinya
        player.transform.position = originalPos;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Fishing");
    }

    public void TeleportBackToDesa()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
    }
}