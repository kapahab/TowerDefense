using UnityEngine;
using System.Collections.Generic;

public class FlatArmorShield : Shield
{
    [Tooltip("The total amount of damage required within the time frame to strip a charge.")]
    public float requiredDamage = 100f;

    [Tooltip("The time frame (in seconds) in which the damage must be dealt.")]
    public float timeFrameInSeconds = 2f;

    // A small data structure to remember each hit's damage and exactly when it happened
    private struct DamageRecord
    {
        public float amount;
        public float time;
    }

    // This list acts as the shield's "memory" of recent attacks
    private List<DamageRecord> damageHistory = new List<DamageRecord>();

    protected override bool CheckPierceCondition(float incomingDamage)
    {
        float currentTime = Time.time;

        // 1. Record this new hit
        damageHistory.Add(new DamageRecord { amount = incomingDamage, time = currentTime });

        // 2. Remove any old hits from the list that fall outside our time window
        damageHistory.RemoveAll(record => currentTime - record.time > timeFrameInSeconds);

        // 3. Calculate the total damage recently taken
        float recentDamageSum = 0f;
        foreach (var record in damageHistory)
        {
            recentDamageSum += record.amount;
        }

        // 4. Returns true (strips a layer) if the recent burst damage is high enough
        return recentDamageSum >= requiredDamage;
    }

    // This is crucial for charge-based shields!
    protected override void OnChargeLost()
    {
        // Clear the history so the player has to build up a completely new 
        // burst of damage to break the next layer.
        damageHistory.Clear();
    }

    public float GetCurrentDamageProgress()
    {
        float currentTime = Time.time;

        // Clean up the history so the UI bar physically drains when time expires!
        damageHistory.RemoveAll(record => currentTime - record.time > timeFrameInSeconds);

        float recentDamageSum = 0f;
        foreach (var record in damageHistory)
        {
            recentDamageSum += record.amount;
        }

        return recentDamageSum;
    }
}