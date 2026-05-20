using UnityEngine;

/// <summary>
/// Attach to: Hook GameObject
/// TIDAK pakai RequireComponent — gunakan LineRenderer object yang sudah ada di hierarchy.
///
/// SETUP:
///   1. lineRenderer → drag object "LineRenderer" (child of Fishing ROd) ke field ini
///   2. rodTip       → drag "Rod Tip" empty object ke field ini
///
/// Tidak perlu LineRenderer component di Hook — yang dipakai adalah object "LineRenderer"
/// yang sudah ada di hierarchy.
/// </summary>
public class FishingLine : MonoBehaviour
{
    [Header("References — drag dari Hierarchy")]
    [Tooltip("Drag object 'LineRenderer' (child of Fishing ROd) ke sini")]
    public LineRenderer lineRenderer;

    [Tooltip("Drag 'Rod Tip' empty object ke sini")]
    public Transform rodTip;

    [Header("Line Appearance")]
    public float lineWidth = 0.04f;
    public Color lineColor = Color.black;

    [Header("Sag")]
    [Range(0f, 1f)] public float sagAmount = 0.15f;
    [Range(3, 20)] public int segments = 10;

    // ── Lifecycle ──────────────────────────────────────────────────────────
    void Start()
    {
        if (lineRenderer == null)
        {
            // Fallback: cari LineRenderer di parent atau sibling
            lineRenderer = GetComponentInParent<LineRenderer>();
            if (lineRenderer == null)
                lineRenderer = FindObjectOfType<LineRenderer>();

            if (lineRenderer == null)
            {
                Debug.LogError("[FishingLine] LineRenderer tidak ditemukan! " +
                               "Drag LineRenderer object ke field 'lineRenderer' di Inspector.");
                return;
            }
            Debug.Log($"[FishingLine] LineRenderer auto-found: {lineRenderer.gameObject.name}");
        }

        SetupLineRenderer();

        if (rodTip == null)
            Debug.LogWarning("[FishingLine] rodTip belum di-assign! Drag Rod Tip object ke Inspector.");
    }

    void SetupLineRenderer()
    {
        lineRenderer.positionCount = segments;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth * 0.5f;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.useWorldSpace = true;
        lineRenderer.sortingOrder = 10;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"))
        {
            color = lineColor
        };
    }

    void LateUpdate()
    {
        if (lineRenderer == null) return;

        Vector3 start = rodTip != null
            ? rodTip.position
            : transform.position + Vector3.up * 3f;

        DrawLine(start, transform.position);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.positionCount = segments;
        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y -= sagAmount * Mathf.Sin(t * Mathf.PI);
            lineRenderer.SetPosition(i, pos);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (rodTip == null) return;
        Gizmos.color = Color.black;
        Gizmos.DrawLine(rodTip.position, transform.position);
        Gizmos.DrawSphere(rodTip.position, 0.08f);
    }
}