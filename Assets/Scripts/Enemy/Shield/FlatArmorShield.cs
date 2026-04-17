using UnityEngine;

public class FlatArmorShield : Shield
{
    [Tooltip("An attack must deal strictly more than this damage in a single hit to strip a charge.")]
    public float damageThreshold = 50f;

    protected override bool CheckPierceCondition(float incomingDamage)
    {
        // Returns true (strips a layer) if the damage is higher than the threshold
        return incomingDamage > damageThreshold;
    }
}