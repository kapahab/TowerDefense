using UnityEngine;

[CreateAssetMenu(fileName = "New Boon", menuName = "Scriptable Objects/BoonData")]
public class BoonData : ScriptableObject
{
    public string boonName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Weighted Randomizer")]
    [Range(1, 100)] public int weight = 50; // Higher weight = more likely to appear

    [Header("Stat Modifiers")]
    public float bonusDamage;
    public float bonusRange;
    public float bonusFireRate; // Note: Typically applied as an attack cooldown reduction
}
