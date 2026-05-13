using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach to: FishSpawner GameObject
/// Setup:
///   - fishPrefabs → beberapa prefab ikan
///   - waterArea   → BoxCollider2D dari Water Area
/// </summary>
public class FishSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] fishPrefabs;   // bisa isi beberapa prefab berbeda
    public Collider2D waterArea;

    [Header("Spawn Settings")]
    public int maxFish = 5;
    public float spawnInterval = 4f;

    [Header("Spawn Margin")]
    public float marginX = 1f;
    public float marginY = 0.4f;

    private float timer;

    void Start()
    {
        if (waterArea == null)
        {
            Debug.LogError("[FishSpawner] waterArea belum di-assign!");
            return;
        }

        // Spawn awal dengan distribusi grid supaya tidak numpuk
        SpawnInitialFish();
    }

    void Update()
    {
        if (FishingGameManager.instance == null || FishingGameManager.instance.IsGameEnded) return;
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (CountFish() < maxFish)
                SpawnOne(GetRandomPos());
        }
    }

    // ── Initial Spawn: bagi area jadi grid, spawn 1 per cell ──────────────
    void SpawnInitialFish()
    {
        Bounds b = waterArea.bounds;
        float usableW = (b.max.x - marginX) - (b.min.x + marginX);
        float usableH = (b.max.y - marginY) - (b.min.y + marginY);

        // Tentukan grid: misal maxFish=5 → ~3 kolom x 2 baris
        int cols = Mathf.CeilToInt(Mathf.Sqrt(maxFish * (usableW / usableH)));
        int rows = Mathf.CeilToInt((float)maxFish / cols);

        float cellW = usableW / cols;
        float cellH = usableH / rows;

        List<Vector2> cells = new List<Vector2>();
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                cells.Add(new Vector2(c, r));

        // Acak urutan cell supaya tidak selalu dari pojok
        for (int i = cells.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (cells[i], cells[j]) = (cells[j], cells[i]);
        }

        for (int i = 0; i < maxFish && i < cells.Count; i++)
        {
            float x = b.min.x + marginX + cells[i].x * cellW + Random.Range(0.1f, cellW * 0.8f);
            float y = b.min.y + marginY + cells[i].y * cellH + Random.Range(0.1f, cellH * 0.8f);

            // Clamp ke dalam bounds
            x = Mathf.Clamp(x, b.min.x + marginX, b.max.x - marginX);
            y = Mathf.Clamp(y, b.min.y + marginY, b.max.y - marginY);

            SpawnOne(new Vector3(x, y, 0f));
        }
    }

    // ── Spawn satu ikan ───────────────────────────────────────────────────
    void SpawnOne(Vector3 pos)
    {
        if (fishPrefabs == null || fishPrefabs.Length == 0) return;

        GameObject prefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        GameObject fish = Instantiate(prefab, pos, Quaternion.identity);

        // Paksa gravity = 0
        Rigidbody2D rb = fish.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Set batas gerak dari water area
        Bounds b = waterArea.bounds;
        FishMovement fm = fish.GetComponent<FishMovement>();
        if (fm != null)
        {
            fm.leftBound = b.min.x + marginX;
            fm.rightBound = b.max.x - marginX;
            fm.topBound = b.max.y - marginY;
            fm.bottomBound = b.min.y + marginY;
        }
    }

    // ── Random pos untuk respawn ──────────────────────────────────────────
    Vector3 GetRandomPos()
    {
        Bounds b = waterArea.bounds;
        float x = Random.Range(b.min.x + marginX, b.max.x - marginX);
        float y = Random.Range(b.min.y + marginY, b.max.y - marginY);
        return new Vector3(x, y, 0f);
    }

    int CountFish() => GameObject.FindGameObjectsWithTag("Fish").Length;
}