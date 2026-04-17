using UnityEngine;
using System.Collections.Generic;

public class HitCountShield : Shield
{
    public int requiredHits = 20;
    public float timeFrameInSeconds = 3f;

    private List<float> hitTimestamps = new List<float>();

    protected override bool CheckPierceCondition(float incomingDamage)
    {
        float currentTime = Time.time;
        hitTimestamps.Add(currentTime);

        // Remove old hits that fall outside our time window
        hitTimestamps.RemoveAll(timestamp => currentTime - timestamp > timeFrameInSeconds);

        // Returns true (strips a layer) if we hit the required density
        return hitTimestamps.Count >= requiredHits;
    }

    // This is called automatically by the base Shield class when a charge is stripped
    protected override void OnChargeLost()
    {
        // Clear the list so the player has to build up the combo again for the next layer
        hitTimestamps.Clear();
    }
}