using UnityEngine;
using System.Collections;


public class NPCBrain : MonoBehaviour
{
    [Header("Data NPC")]
    public NPCData data;

    [Header("Ronda (isi hanya jika isNightPatrol = true)")]
    public PatrolPath patrolPath;


    [Header("Debug (read-only)")]
    [SerializeField] private NPCStates _currentState;
    [SerializeField] private string _currentActivity;

    private SpriteRenderer _sprite;
    private Transform _patrolTarget;
    private bool _isPatrolWaiting;

    // Cache lokasi agar tidak FindWithTag setiap jam
    private System.Collections.Generic.Dictionary<string, Vector3> _locationCache
        = new System.Collections.Generic.Dictionary<string, Vector3>();

    // ─────────────────────────────────────────────
    // Unity lifecycle
    // ─────────────────────────────────────────────

    void OnEnable() => DayTimeCycle.OnHourChanged += OnHourChanged;
    void OnDisable() => DayTimeCycle.OnHourChanged -= OnHourChanged;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();

        // Guard data null sebelum apapun dijalankan
        if (data == null)
        {
            Debug.LogError($"[NPCBrain] 'Data' (NPCData) belum diisi di Inspector! → {gameObject.name}");
            enabled = false;
            return;
        }

        BuildLocationCache();
    }

    void Start()
    {
        if (!enabled) return;   // skip jika Awake sudah disable

        if (DayTimeCycle.Instance != null)
            EvaluateState(DayTimeCycle.Instance.hours);
        else
        {
            // Instance belum ready — tunggu frame berikutnya
            StartCoroutine(WaitForTimeSystem());
        }
    }
    IEnumerator WaitForTimeSystem()
    {
        // Tunggu sampai DayTimeCycle.Instance tersedia
        yield return new WaitUntil(() => DayTimeCycle.Instance != null);
        EvaluateState(DayTimeCycle.Instance.hours);
    }

    // ─────────────────────────────────────────────
    // AI inti: dipanggil setiap jam berganti
    // ─────────────────────────────────────────────

    void OnHourChanged(int hour) => EvaluateState(hour);

    void EvaluateState(int hour)
    {
        // Ambil referensi sekali — sudah pasti ada karena Singleton
        var time = DayTimeCycle.Instance;

        // ── Prioritas 1: Malam ──────────────────
        if (time.IsNightCurfew())
        {
            if (data.isNightPatrol)
                EnterState(NPCStates.Patrolling, "Ronda malam", null);
            else
                EnterSleep();
            return;
        }

        // ── Prioritas 2: Jam sholat ─────────────
        if (time.IsPrayerTime())
        {
            EnterState(NPCStates.Praying, "Sholat", "Masjid");
            return;
        }

        // ── Prioritas 3: Jadwal NPC ─────────────
        ScheduleEntry entry = data.GetEntryForHour(hour);
        if (entry != null)
        {
            EnterState(NPCStates.Working, entry.activityLabel, entry.locationTag);
            return;
        }

        // ── Default: kembali ke rumah ────────────
        GoHome();
    }

    // ─────────────────────────────────────────────
    // Transisi state
    // ─────────────────────────────────────────────

    void EnterState(NPCStates state, string activity, string locationTag)
    {
        _currentState = state;
        _currentActivity = activity;

        SetSpriteVisible(state != NPCStates.Sleeping);

        if (!string.IsNullOrEmpty(locationTag))
            TeleportTo(locationTag);

        if (state == NPCStates.Patrolling)
        {
            _patrolTarget = null;
            _isPatrolWaiting = false;
        }
    }

    void EnterSleep()
    {
        _currentState = NPCStates.Sleeping;
        _currentActivity = "Tidur";
        SetSpriteVisible(false);
        TeleportTo(data.homeTag);
    }

    void GoHome()
    {
        EnterState(NPCStates.AtHome, "Di rumah", data.homeTag);
    }

    // ─────────────────────────────────────────────
    // Patrol AI
    // ─────────────────────────────────────────────

    void HandlePatrolMovement()
    {
        if (patrolPath == null || _isPatrolWaiting) return;

        if (_patrolTarget == null)
            _patrolTarget = patrolPath.GetNextWaypoint();

        transform.position = Vector3.MoveTowards(
            transform.position,
            _patrolTarget.position,
            1.2f * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, _patrolTarget.position) < 0.05f)
        {
            _patrolTarget = null;
            StartCoroutine(PatrolWaitRoutine());
        }
    }

    IEnumerator PatrolWaitRoutine()
    {
        _isPatrolWaiting = true;
        yield return new WaitForSeconds(Random.Range(1.5f, 3.5f));
        _isPatrolWaiting = false;
    }

    // ─────────────────────────────────────────────
    // Helper
    // ─────────────────────────────────────────────

    void TeleportTo(string tag)
    {
        var npc = GetComponent<NPC>();
        if (npc != null && npc.IsDialogueActive()) return;

        if (_locationCache.TryGetValue(tag, out Vector3 pos))
        {
            transform.position = pos;
            return;
        }

        GameObject loc = GameObject.FindWithTag(tag);
        if (loc != null)
        {
            _locationCache[tag] = loc.transform.position;
            transform.position = loc.transform.position;
        }
        else
        {
            Debug.LogWarning($"[NPCBrain] Tag '{tag}' tidak ditemukan! NPC: {data.npcName}");
        }
    }

    void SetSpriteVisible(bool visible)
    {
        if (_sprite != null) _sprite.enabled = visible;
    }

    void BuildLocationCache()
    {
        if (data == null) return;
        CacheTag(data.homeTag);
        if (data.schedule != null)
            foreach (var entry in data.schedule)
                CacheTag(entry.locationTag);
        CacheTag("Masjid");
    }

    void CacheTag(string tag)
    {
        if (string.IsNullOrEmpty(tag) || _locationCache.ContainsKey(tag)) return;
        GameObject obj = GameObject.FindWithTag(tag);
        if (obj != null) _locationCache[tag] = obj.transform.position;
    }

    // ─────────────────────────────────────────────
    // API publik
    // ─────────────────────────────────────────────

    public string GetStatusText() => $"{data.npcName}\n{data.job}  ·  {_currentActivity}";
    public NPCStates GetState() => _currentState;
    public bool IsSleeping() => _currentState == NPCStates.Sleeping;
    public bool IsPatrolling() => _currentState == NPCStates.Patrolling;
}