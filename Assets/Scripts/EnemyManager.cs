using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //holds the list of towers? towers could send events when they are destroyed so that this script updates the enemys target list
    [SerializeField] List<GameObject> towersList;
    private Queue<GameObject> towerQueue;
    public static Action<Transform> OnNextTower;

    [SerializeField] private Transform spawnTransform;
    [SerializeField] private GameObject enemyPrefab;

    public int numberOfEnemies;
    void Start()
    {
        ListTowersAtStart();
        SpawnEnemies();
        OnNextTower?.Invoke(towerQueue.Dequeue().transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ListTowersAtStart()
    {
        towersList.Sort((towerA, towerB) =>
        {
            float distA = Vector3.Distance(spawnTransform.position, towerA.transform.position);
            float distB = Vector3.Distance(spawnTransform.position, towerB.transform.position);
            return distA.CompareTo(distB);
        });

        towerQueue = new Queue<GameObject>(towersList);
    }

    private void OnEnable()
    {
        TowerHealth.OnTowerDestroyed += NextTower;
    }

    void NextTower()
    {
        if (towerQueue.Count > 0)
        {
            GameObject nextTower = towerQueue.Dequeue();
            OnNextTower?.Invoke(nextTower.transform);
            // Do something with the nextTower, such as setting it as the target for an enemy
        }
        else
        {
            Debug.Log("No more towers in the queue.");
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemyPrefab, spawnTransform.position, Quaternion.identity);
        }
    }
}
