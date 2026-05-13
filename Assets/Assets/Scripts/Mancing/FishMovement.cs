using UnityEngine;

/// <summary>
/// Attach to: Fish Prefab
/// Bounds (leftBound, rightBound, topBound, bottomBound) di-set otomatis oleh FishSpawner.
/// Bisa juga di-set manual di Inspector untuk testing.
/// </summary>
public class FishMovement : MonoBehaviour
{
    [Header("Speed")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    [Header("Swim Bounds — di-set otomatis oleh FishSpawner")]
    public float leftBound = -8f;
    public float rightBound = 8f;
    public float topBound = 2f;
    public float bottomBound = -4f;

    [Header("Sprite")]
    [Tooltip("TRUE kalau sprite default ngadep kanan")]
    public bool defaultFacingRight = true;
    public Sprite[] fishSprites;

    // ── Internal ───────────────────────────────────────────────────────────
    private float speed;
    private Vector2 dir;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        dir = Random.value > 0.5f ? Vector2.right : Vector2.left;

        PickRandomSprite();
        UpdateFacing();
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        // Clamp posisi agar tidak keluar bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
        pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
        transform.position = pos;

        // Balik arah X kalau nyentuh batas kiri/kanan
        if (transform.position.x >= rightBound)
        {
            dir = Vector2.left;
            UpdateFacing();
        }
        else if (transform.position.x <= leftBound)
        {
            dir = Vector2.right;
            UpdateFacing();
        }
    }

    void PickRandomSprite()
    {
        if (sr == null || fishSprites == null || fishSprites.Length == 0) return;
        sr.sprite = fishSprites[Random.Range(0, fishSprites.Length)];
    }

    void UpdateFacing()
    {
        if (sr == null) return;
        bool movingRight = dir.x > 0;
        sr.flipX = defaultFacingRight ? !movingRight : movingRight;
    }
}