using System.Collections;
using UnityEngine;

public class GlobalIncomeGenerator : MonoBehaviour
{
    [Header("Passive Income")]
    public int passiveGoldAmount = 5;
    public float generationInterval = 1f;

    private void Start()
    {
        StartCoroutine(GeneratePassiveIncome());
    }

    private IEnumerator GeneratePassiveIncome()
    {
        while (true)
        {
            yield return new WaitForSeconds(generationInterval);
            EconomyManager.AddGold(passiveGoldAmount);
        }
    }
}
