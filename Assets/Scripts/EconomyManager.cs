using System;

public static class EconomyManager
{
    public static int currentGold { get; private set; }

    public static event Action<int> OnGoldChanged;

    public static void ResetGold(int startingAmount)
    {
        currentGold = startingAmount;
        OnGoldChanged?.Invoke(currentGold);
    }

    public static void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold);
    }

    public static bool TrySpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            OnGoldChanged?.Invoke(currentGold);
            return true;
        }
        return false;
    }
}