using UnityEngine;

/// <summary>
/// Attach this script to your Main Camera to follow a 2D player smoothly.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The player or object the camera should follow.")]
    public Transform target;

    [Header("Camera Settings")]
    [Tooltip("How quickly the camera catches up to the target.")]
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f;

    [Tooltip("Offset from the target position.")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Boundary Settings (Optional)")]
    public bool useBoundaries = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow2D: No target assigned!");
            return;
        }

        // Desired position based on target + offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between current and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply boundaries if enabled
        if (useBoundaries)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = smoothedPosition;
    }
}
