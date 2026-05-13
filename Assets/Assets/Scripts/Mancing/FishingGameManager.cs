using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach to: GameManager GameObject
/// 
/// UI Setup yang dibutuhkan di Canvas:
///   - scoreText      → TMP Text "Score: 0"
///   - timerText      → TMP Text "60"
///   - reelButton     → Button "TARIK!" — assign FishCatch.Reel() ke OnClick
///   - hookStatusText → TMP Text (opsional) — info "Ada ikan! Tarik!" 
///   - gameEndPanel   → Panel (awalnya inactive)
///     └─ finalScoreText → TMP Text di dalam panel
///     └─ restartButton  → Button "Main Lagi"
/// </summary>
public class FishingGameManager : MonoBehaviour
{
    public static FishingGameManager instance;

    // ── Gameplay ────────────────────────────────────────────────────────────
    [Header("Gameplay")]
    public float gameDuration = 60f;

    // ── UI References ───────────────────────────────────────────────────────
    [Header("HUD")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text hookStatusText;   // opsional – "Ada ikan! Tarik!" 
    public Button reelButton;       // aktif hanya saat ada ikan

    [Header("Game End")]
    public GameObject gameEndPanel;
    public TMP_Text finalScoreText;

    // ── Internal ─────────────────────────────────────────────────────────────
    private int score = 0;
    private float timer;
    private bool gameEnded = false;

    public bool IsGameEnded => gameEnded;

    // ── Unity Lifecycle ───────────────────────────────────────────────────────
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        timer = gameDuration;

        // Sembunyikan game end panel
        if (gameEndPanel != null)
            gameEndPanel.SetActive(false);

        // Reel button default disabled (aktif pas ada ikan)
        SetReelButtonActive(false);

        UpdateScoreUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        timer = Mathf.Max(0f, timer);
        UpdateTimerUI();

        if (timer <= 0f) EndGame();
    }

    // ── Public: dipanggil script lain ─────────────────────────────────────────
    public void AddScore()
    {
        score++;
        UpdateScoreUI();
        SetReelButtonActive(false);
        SetHookStatus("");
    }

    /// <summary>Dipanggil FishCatch saat ikan pertama kali kena kail.</summary>
    public void OnFishHooked()
    {
        SetReelButtonActive(true);
        SetHookStatus("Ada ikan! Tap TARIK! terus!");
    }

    // ── UI Helpers ─────────────────────────────────────────────────────────────
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Tangkapan: " + score;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int sisa = Mathf.CeilToInt(timer);
            timerText.text = sisa.ToString();

            // Warnain merah kalau sisa < 10 detik
            if (timerText.TryGetComponent(out TMP_Text tmp))
                tmp.color = sisa <= 10 ? Color.red : Color.white;
        }
    }

    void SetReelButtonActive(bool active)
    {
        if (reelButton != null)
            reelButton.interactable = active;
    }

    void SetHookStatus(string msg)
    {
        if (hookStatusText != null)
            hookStatusText.text = msg;
    }

    // ── Game End ───────────────────────────────────────────────────────────────
    void EndGame()
    {
        gameEnded = true;
        SetReelButtonActive(false);

        if (gameEndPanel != null)
            gameEndPanel.SetActive(true);

        if (finalScoreText != null)
        {
            string grade = GetGrade(score);
            finalScoreText.text =
                $"Waktu Habis!\n\nIkan Tertangkap: {score}\n\n{grade}";
        }
    }

    string GetGrade(int s)
    {
        if (s >= 15) return "🏆 Nelayan Legendaris!";
        if (s >= 10) return "⭐ Nelayan Handal!";
        if (s >= 5) return "👍 Lumayan Jago!";
        return "🎣 Pemula - Terus Berlatih!";
    }

    /// <summary>Assign ke Restart Button di game end panel.</summary>
    public void RestartGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}