using UnityEngine;
using System;

// ─────────────────────────────────────────────────────────
// Satu entri jadwal: dari jam berapa, sampai jam berapa,
// pergi ke lokasi mana (pakai Unity Tag), dan label aktivitas.
// ─────────────────────────────────────────────────────────
[System.Serializable]
public class ScheduleEntry
{
    [Tooltip("Jam mulai aktivitas (0–23).")]
    public int startHour;

    [Tooltip("Jam selesai aktivitas (0–23, eksklusif).")]
    public int endHour;

    [Tooltip("Unity Tag dari GameObject lokasi tujuan. Contoh: 'Beach', 'Market', 'Mosque'.")]
    public string locationTag;

    [Tooltip("Label untuk UI atau debug. Contoh: 'Melaut', 'Berjualan'.")]
    public string activityLabel;
}
