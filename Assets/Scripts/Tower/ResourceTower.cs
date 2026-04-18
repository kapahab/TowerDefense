using System.Collections;
using UnityEngine;

public class ResourceTower : MonoBehaviour
{
    public int goldPerInterval = 10; // Amount of gold generated per interval
    void Start()
    {
        StartCoroutine(GenerateResource());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GenerateResource()
    {
        while (true)
        {
            EconomyManager.AddGold(goldPerInterval);
            yield return new WaitForSeconds(5f); 
        }
    }
}
