using UnityEngine;

public abstract class Shield : MonoBehaviour
{
    [Tooltip("How many layers or charges this shield has before it is completely destroyed.")]
    public int shieldCharges = 1;

    // Child scripts define the rule for what strips a layer
    protected abstract bool CheckPierceCondition(float incomingDamage);

    // Optional: Allows child scripts to reset their state (e.g., clearing hit counters) when a layer breaks
    protected virtual void OnChargeLost() { }

    public float ProcessDamage(float incomingDamage)
    {
        // 1. If no charges are left, let all damage through to the next layer/health
        if (shieldCharges <= 0)
        {
            return incomingDamage;
        }

        // 2. Ask the specific shield type if this attack meets the rules to strip a charge
        if (CheckPierceCondition(incomingDamage))
        {
            // 3. Rule met! Strip one charge.
            shieldCharges--;
            OnChargeLost(); // Tell the child script to reset if needed

            if (shieldCharges <= 0)
            {
                Debug.Log($"{gameObject.name}'s {this.GetType().Name} was completely destroyed!");
            }
            else
            {
                Debug.Log($"{gameObject.name}'s {this.GetType().Name} lost a charge! Charges remaining: {shieldCharges}");
            }

            // The attack was spent breaking the charge. No damage reaches the enemy health.
            return 0f;
        }

        // 4. The attack failed to meet the condition. It is absorbed harmlessly.
        return 0f;
    }
}