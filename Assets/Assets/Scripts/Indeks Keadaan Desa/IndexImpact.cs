using UnityEngine;

[System.Serializable]
public class IndexImpact
{
    public float environment;
    public float social;

    [Range(0f, 1f)]
    public float quality = 1f;
}