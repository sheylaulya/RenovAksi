using UnityEngine;

public class TrustTester : MonoBehaviour
{
    public NpcTrustManagers trustManager;
    public EnvironmentSystem environmentSystem;

    void Update()
    {
        // Tekan T untuk TAMBAH (Trust)
        if (Input.GetKeyDown(KeyCode.T))
        {
            trustManager.AddTrust("Bu Lastri", 10f);
        }

        // Tekan G untuk GAGAL / KURANG (Decrease)
        if (Input.GetKeyDown(KeyCode.G))
        {
            trustManager.RemoveTrust("Bu Lastri", 10f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            environmentSystem.UpdateEnvironmentDaily();
        }
    }
}