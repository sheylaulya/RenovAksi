using UnityEngine;

/// <summary>
/// Komponen penanda lokasi. Pasang ke setiap GameObject lokasi di scene,
/// lalu beri tag yang sesuai (misal: "Beach", "Market", "Mosque").
///
/// NPCBrain menemukan lokasi melalui tag, bukan melalui referensi langsung,
/// sehingga Anda bisa menambah atau memindahkan lokasi tanpa menyentuh kode NPC.
///
/// SETUP:
///   1. Buat GameObject kosong di posisi lokasi (misal di tengah area Pantai).
///   2. Pasang komponen LocationPoint ini.
///   3. Beri tag yang sesuai di Inspector (harus sama dengan NPCData.locationTag / homeTag).
///   4. Nama tag harus didaftarkan dulu di Edit → Project Settings → Tags and Layers.
/// </summary>
/// 
[System.Serializable]
public class LocationPoint : MonoBehaviour
{
    [Tooltip("Label opsional untuk identifikasi di Inspector. Tidak dipakai secara kode.")]
    public string locationName;

    void OnDrawGizmos()
    {
        // Visualisasi di Scene view saja — tidak tampil saat play
        Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 0.6f);
        Gizmos.DrawSphere(transform.position, 0.3f);
        Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 1f);
        Gizmos.DrawWireSphere(transform.position, 0.35f);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(
            transform.position + Vector3.up * 0.5f,
            string.IsNullOrEmpty(locationName) ? gameObject.tag : locationName
        );
#endif
    }
}

// ─────────────────────────────────────────────────────────
// DAFTAR TAG YANG HARUS DIBUAT DI PROJECT SETTINGS:
//
//   Home_Harjo, Home_Karso, Home_Budi   (satu tag per rumah NPC)
//   Beach                               (Pantai / Area Tanggul)
//   Market                              (Pasar)
//   Mosque                              (Masjid / Balai Desa)
//   Tambak                              (Zona Tambak)
//   Mangrove                            (Zona Mangrove)
//   BalaiDesa                           (Balai Desa)
//   Patrol_Permukiman                   (titik patrol ronda)
//   Patrol_Tanggul                      (titik patrol ronda)
// ─────────────────────────────────────────────────────────