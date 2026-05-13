using UnityEngine;

/// <summary>
/// Attach to: Hook GameObject
/// 
/// SETUP (tidak perlu atur Layer apapun):
///   1. waterArea → drag BoxCollider2D dari Water Area GameObject ke field ini
///   2. Pastikan Hook punya Rigidbody2D: Gravity Scale=0, Constraints=Freeze All
/// </summary>
public class HookController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Water Area")]
    [Tooltip("Drag BoxCollider2D dari Water Area GameObject ke sini")]
    public Collider2D waterArea;

    // ── Internal ───────────────────────────────────────────────────────────
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isMoving;
    private FishCatch fishCatch;
    private bool waitingForReturn; // hook harus balik dulu sebelum bisa cast lagi

    // ── Unity Lifecycle ────────────────────────────────────────────────────
    void Start()
    {
        startPos = transform.position;
        targetPos = transform.position;
        fishCatch = GetComponent<FishCatch>();

        if (waterArea == null)
            Debug.LogError("[HookController] waterArea belum di-assign! Drag BoxCollider2D Water Area ke Inspector.");
    }

    void Update()
    {
        HandleInput();
        MoveToTarget();
    }

    // ── Input ──────────────────────────────────────────────────────────────
    void HandleInput()
    {
        if (FishingGameManager.instance == null || FishingGameManager.instance.IsGameEnded) return;
        if (fishCatch != null && fishCatch.HasFish) return;   // lagi narik ikan
        if (waitingForReturn && isMoving) return;    // hook belum balik

        Vector3 worldPos = Vector3.zero;
        bool gotInput = false;

        // Mouse
        if (Input.GetMouseButtonDown(0))
        {
            worldPos = GetWorldPos(Input.mousePosition);
            gotInput = true;
        }
        // Touch
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            worldPos = GetWorldPos(Input.GetTouch(0).position);
            gotInput = true;
        }

        if (!gotInput) return;

        // Cek apakah klik di dalam Water Area
        bool inWater = waterArea != null && waterArea.OverlapPoint(worldPos);

        if (inWater)
        {
            waitingForReturn = false;
            SetTarget(worldPos);
            Debug.Log($"[Hook] Cast ke {worldPos}");
        }
        else
        {
            Debug.Log($"[Hook] Klik di luar water area: {worldPos}");
        }
    }

    // ── Movement ───────────────────────────────────────────────────────────
    void MoveToTarget()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            transform.position = targetPos;
            isMoving = false;

            if (waitingForReturn)
                waitingForReturn = false; // sudah sampai start, siap cast lagi
        }
    }

    // ── Public ─────────────────────────────────────────────────────────────
    /// <summary>
    /// Dipanggil FishCatch setelah ikan berhasil diangkat.
    /// Hook wajib balik ke posisi awal sebelum bisa cast lagi.
    /// </summary>
    public void ReturnToStart()
    {
        waitingForReturn = true;
        SetTarget(startPos);
    }

    // ── Helpers ────────────────────────────────────────────────────────────
    void SetTarget(Vector3 pos)
    {
        targetPos = pos;
        isMoving = true;
    }

    Vector3 GetWorldPos(Vector3 screenPos)
    {
        // Untuk kamera Orthographic 2D, z harus 0 di world space
        Vector3 wp = Camera.main.ScreenToWorldPoint(screenPos);
        wp.z = 0f;
        return wp;
    }

    void OnDrawGizmosSelected()
    {
        if (waterArea == null) return;
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.3f);
        Gizmos.DrawCube(waterArea.bounds.center, waterArea.bounds.size);
    }
}