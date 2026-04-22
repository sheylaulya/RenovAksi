using UnityEngine;

public class InvertedMaskController : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Material material;
    public Camera cam;

    [Header("Offset (World Space)")]
    public Vector3 worldOffset = new Vector3(0f, 1f, 0f); // shift hole up toward chest/center

    void Start()
    {
        // Fallback to main camera if not assigned
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        if (target == null || material == null || cam == null) return;

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // Target is behind the camera — hide the hole by pushing it offscreen
        if (screenPos.z < 0f)
        {
            material.SetVector("_HoleCenter", new Vector4(-1f, -1f, 0f, 0f));
            return;
        }

        Vector2 uv = new Vector2(
            screenPos.x / Screen.width,
            screenPos.y / Screen.height
        );

        // Keep aspect ratio in sync so ellipse shape stays correct at any resolution
        float aspectRatio = (float)Screen.width / Screen.height;

        material.SetVector("_HoleCenter", new Vector4(uv.x, uv.y, 0f, 0f));
        material.SetFloat("_AspectRatio", aspectRatio);
    }
}