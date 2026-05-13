using UnityEngine;

/// <summary>
/// Attach to: Hook GameObject (sama object dengan HookController)
/// Setup:
///   - reelTarget → assign Transform di darat/dock (bisa bikin empty GameObject di tepi)
///   - Pastikan Hook punya CircleCollider2D dengan IsTrigger = TRUE
///   - Fish prefab harus punya tag "Fish"
/// </summary>
public class FishCatch : MonoBehaviour
{
    [Header("References")]
    public Transform reelTarget; // Empty GameObject di darat (titik ikan diangkat)

    [Header("Reel Settings")]
    public float reelSpeed = 3f;   // Kecepatan hook bergerak saat direel
    public float reelDecay = 1.5f; // Seberapa cepat reel power habis (drag effect)
    public float reelPerTap = 1.2f; // Power yang ditambah per tap tombol Reel

    // ── State ──────────────────────────────────────────────────────────────
    public bool HasFish => hasFish;

    private bool hasFish = false;
    private GameObject currentFish;
    private float reelPower = 0f;       // "stamina" reel – berkurang otomatis
    private HookController hookController;

    void Start()
    {
        hookController = GetComponent<HookController>();

        // Auto-set reelTarget ke posisi awal hook kalau belum di-assign
        if (reelTarget == null)
        {
            GameObject auto = new GameObject("ReelTarget_Auto");
            auto.transform.position = transform.position;
            reelTarget = auto.transform;
            Debug.LogWarning("[FishCatch] reelTarget belum di-assign! Pakai posisi awal hook. " +
                             "Sebaiknya assign manual ke titik di darat.");
        }
    }

    // ── Trigger: Ikan Kena Kail ────────────────────────────────────────────
    [System.Obsolete]
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Fish")) return;
        if (hasFish) return;
        if (FishingGameManager.instance == null || FishingGameManager.instance.IsGameEnded) return;

        hasFish = true;
        currentFish = other.gameObject;

        // Stop pergerakan ikan
        FishMovement fm = currentFish.GetComponent<FishMovement>();
        if (fm != null) fm.enabled = false;

        Rigidbody2D rb = currentFish.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true; // biar gak jatuh karena gravity
        }

        FishingGameManager.instance.OnFishHooked();
    }

    // ── Update: Logika Reel ────────────────────────────────────────────────
    void Update()
    {
        if (!hasFish || currentFish == null) return;

        // Ikan ikutin posisi hook
        currentFish.transform.position = transform.position;

        // Reel power berkurang sendiri (player harus terus tap)
        reelPower -= reelDecay * Time.deltaTime;
        reelPower = Mathf.Max(0f, reelPower);

        // Gerakkan hook ke darat kalau ada power
        if (reelPower > 0f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                reelTarget.position,
                reelSpeed * Time.deltaTime
            );
        }

        // Cek apakah sudah sampai darat
        if (Vector3.Distance(transform.position, reelTarget.position) < 0.25f)
        {
            LandFish();
        }
    }

    // ── Public: Dipanggil oleh UI Button "Reel" ────────────────────────────
    public void Reel()
    {
        if (!hasFish) return;
        if (FishingGameManager.instance == null || FishingGameManager.instance.IsGameEnded) return;

        reelPower += reelPerTap;
        reelPower = Mathf.Min(reelPower, reelPerTap * 5f); // cap biar gak exploit spam
    }

    // ── Private: Ikan Berhasil Diangkat ───────────────────────────────────
    void LandFish()
    {
        Destroy(currentFish);
        currentFish = null;
        hasFish = false;
        reelPower = 0f;

        FishingGameManager.instance.AddScore();
        hookController.ReturnToStart();
    }

    // ── Gizmo buat debug di Scene view ────────────────────────────────────
    void OnDrawGizmosSelected()
    {
        if (reelTarget == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, reelTarget.position);
        Gizmos.DrawWireSphere(reelTarget.position, 0.25f);
    }
}