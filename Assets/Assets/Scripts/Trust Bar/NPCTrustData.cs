using UnityEngine;

[CreateAssetMenu(fileName = "NPCTrustData", menuName = "NPC/Trust Data")]
public class NPCTrustData : ScriptableObject
{
    public Sprite spriteNPC;
    public string npcName;
    public float initialTrust = 0f;
    public float maxTrust = 100f;
}