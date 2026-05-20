using UnityEngine;

/// <summary>
/// Komponen jalur patrol untuk NPC ronda malam.
/// Pasang ke GameObject kosong, lalu isi array Waypoints
/// dengan Transform di titik-titik yang ingin dipatroli.
///
/// NPCBrain memanggil GetNextWaypoint() setiap kali NPC
/// sampai di satu titik dan siap bergerak ke titik berikutnya.
/// </summary>
/// 
/// 

[System.Serializable]
public class PatrolPath : MonoBehaviour
{
    [Tooltip("Titik-titik yang dikunjungi NPC ronda secara berurutan (lalu kembali ke awal).")]
    public Transform[] waypoints;

    private int _index = 0;

    /// <summary>
    /// Mengembalikan waypoint berikutnya secara melingkar.
    /// Setiap panggilan maju satu langkah.
    /// </summary>
    public Transform GetNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0)
            return transform;   // fallback: diam di posisi PatrolPath itu sendiri

        Transform next = waypoints[_index];
        _index = (_index + 1) % waypoints.Length;
        return next;
    }

    /// <summary>Reset ke waypoint pertama (opsional, untuk event tertentu).</summary>
    public void ResetPath() => _index = 0;

    // ─────────────────────────────────────────────
    // Visualisasi jalur di Scene view
    // ─────────────────────────────────────────────

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.9f);

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            // Titik waypoint
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);

            // Garis ke waypoint berikutnya (termasuk kembali ke awal)
            int next = (i + 1) % waypoints.Length;
            if (waypoints[next] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
        }
    }
}

// ─────────────────────────────────────────────────────────
// SETUP DI SCENE:
//
//   1. Buat GameObject kosong bernama "PatrolPath_Permukiman".
//   2. Pasang komponen PatrolPath.
//   3. Buat beberapa GameObject kosong anak (child) sebagai titik,
//      misal: WP_01, WP_02, WP_03, WP_04.
//   4. Isi array Waypoints dengan WP_01 s.d WP_04.
//   5. Di NPCBrain NPC ronda, isi field patrolPath dengan
//      PatrolPath_Permukiman.
// ─────────────────────────────────────────────────────────