using UnityEngine;

public class TrustTester : MonoBehaviour
{
    public NpcTrustManagers trustManager;

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
    }
}