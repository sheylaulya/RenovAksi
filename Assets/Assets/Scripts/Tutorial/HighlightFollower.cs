using UnityEngine;

public class HighlightFollower : MonoBehaviour
{
    public Transform target; // player
    public Vector3 offset;

    void Update()
    {
        if (target == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }
}