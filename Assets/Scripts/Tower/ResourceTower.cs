using System.Collections;
using UnityEngine;

public class ResourceTower : MonoBehaviour, ITowerDataContainer
{
    public TowerData data; // Reference to the tower data
    private TowerDataInstance dataIns;
    void Start()
    {
        dataIns = new TowerDataInstance();
        dataIns.InitializeFromBlueprint(data);
        StartCoroutine(GenerateResource());
    }


    IEnumerator GenerateResource()
    {
        while (true)
        {
            EconomyManager.AddGold(dataIns.goldGenerated);
            yield return new WaitForSeconds(dataIns.generationInterval); 
        }
    }

    public TowerDataInstance GetTowerDataInstance()
    {
        return dataIns;
    }
}
