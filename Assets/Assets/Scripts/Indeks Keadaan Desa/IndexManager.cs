using UnityEngine;

public class IndexManager : MonoBehaviour
{
    public static IndexManager instance;

    public WaterSystem water;
    public EnvironmentSystem environment;
    public SocialSystem social;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public float GetAverageIndex()
    {
        return (water.Value + environment.Value + social.Value) / 3f;
    }
}