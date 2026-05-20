using UnityEngine;
using System;


// ─────────────────────────────────────────────────────────
// ScriptableObject profil NPC.
// Buat file baru: klik kanan → Create → RenovAksi → NPC Data
// ─────────────────────────────────────────────────────────
[CreateAssetMenu(fileName = "NPC_", menuName = "NPC Brain/NPC Data")]
public class NPCData : ScriptableObject
{
    [Header("Identitas")]
    public string npcName = "Warga";
    public int age = 30;
    public string job = "Nelayan";

    [Header("Lokasi Tetap")]
    [Tooltip("Unity Tag rumah NPC ini. Harus ada GameObject dengan tag ini di scene.")]
    public string homeTag = "Home_Default";

    [Header("Jadwal Harian")]
    [Tooltip("Daftar aktivitas berdasarkan jam. Urutan tidak penting.")]
    public ScheduleEntry[] schedule;

    [Header("Perilaku Malam")]
    [Tooltip("Centang jika NPC ini adalah petugas ronda. NPC ronda TIDAK dipaksa pulang malam.")]
    public bool isNightPatrol = false;

    // ─────────────────────────────────────────────
    // Digunakan oleh NPCBrain untuk mendapatkan
    // jadwal aktif berdasarkan jam sekarang.
    // Mengembalikan null jika tidak ada entri cocok
    // (NPCBrain akan default ke rumah).
    // ─────────────────────────────────────────────
    public ScheduleEntry GetEntryForHour(int hour)
    {
        foreach (var entry in schedule)
        {
            // Tangani kasus yang melewati tengah malam, misal 22–04
            bool wrapsAroundMidnight = entry.endHour < entry.startHour;
            if (wrapsAroundMidnight)
            {
                if (hour >= entry.startHour || hour < entry.endHour)
                    return entry;
            }
            else
            {
                if (hour >= entry.startHour && hour < entry.endHour)
                    return entry;
            }
        }
        return null;
    }
}

// ─────────────────────────────────────────────────────────
// CONTOH PENGISIAN DI INSPECTOR (Pak Harjo – Nelayan)
//
// npcName    : Pak Harjo
// age        : 52
// job        : Nelayan
// homeTag    : Home_Harjo
// isNightPatrol: false
//
// schedule[0]: startHour=6,  endHour=10, locationTag=Beach,    activityLabel=Melaut
// schedule[1]: startHour=10, endHour=12, locationTag=Market,   activityLabel=Jual ikan
// schedule[2]: startHour=13, endHour=17, locationTag=Home_Harjo, activityLabel=Istirahat
//
// CONTOH (Pak Karso – Ronda)
//
// npcName    : Pak Karso
// age        : 45
// job        : Warga
// homeTag    : Home_Karso
// isNightPatrol: true
//
// schedule[0]: startHour=21, endHour=1,  locationTag=Patrol_Permukiman, activityLabel=Ronda malam
// ─────────────────────────────────────────────────────────