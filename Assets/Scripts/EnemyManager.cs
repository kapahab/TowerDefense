using System;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemyManager : MonoBehaviour // spawns enemies and sends tower lists to the enemies so that they can select their targets. also updates the tower list when towers are added or removed.
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //holds the list of towers? towers could send events when they are destroyed so that this script updates the enemys target list
    [SerializeField] List<GameObject> towersList;
    public static Action<List<Transform>> OnNextTower;

    [SerializeField] private Transform spawnTransform;
    [SerializeField] private GameObject enemyPrefab;

    public int numberOfEnemies;
    List<Transform> enemyTransform = new List<Transform>();

    private Vector3 invalidVector3 = new Vector3(-999, -999, -999);

    void Start()
    {
        SpawnEnemies();
        ListTowersAndQueue(spawnTransform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ListTowersAndQueue(Vector3 comparingPosition)
    {
        if (comparingPosition == invalidVector3)
        {
            Debug.Log("No more enemies left");
            return;
        }

        towersList.RemoveAll(t=>t == null);

        if (towersList.Count <= 0)
        {
            return;
        }

        towersList.Sort((towerA, towerB) =>
        {
            float distA = Vector3.Distance(comparingPosition, towerA.transform.position);
            float distB = Vector3.Distance(comparingPosition, towerB.transform.position);
            return distA.CompareTo(distB);
        });

        OnNextTower?.Invoke(towersList.ConvertAll(t => t.transform));
    }

    void NewTowerAdded(GameObject tower)
    {
        if (!towersList.Contains(tower))
        {
            towersList.Add(tower);
        }

        ListTowersAndQueue(GetEnemyAveragePosition());
    }

    // Call this when a tower is destroyed (instead of just Dequeueing)
    void TowerRemoved(GameObject tower)
    {
        if (towersList.Contains(tower))
        {
            towersList.Remove(tower);
        }

        ListTowersAndQueue(GetEnemyAveragePosition());
    }

    private void OnEnable()
    {
        TowerHealth.OnTowerDestroyed += TowerRemoved;
        Cell.OnTowerSpawned += NewTowerAdded;
    }

    Vector3 GetEnemyAveragePosition()
    {
        if (enemyTransform == null)
            return invalidVector3;

        enemyTransform.RemoveAll(e => e == null);

        if (enemyTransform.Count == 0)
            return invalidVector3;

        float aggregateX=0;
        float aggregateY=0;
        float aggregateZ=0;

        for (int i = 0; i < enemyTransform.Count; i++)
        {
            aggregateX += enemyTransform[i].position.x;
            aggregateY += enemyTransform[i].position.y;
            aggregateZ += enemyTransform[i].position.z;
        }

        return new Vector3(aggregateX / enemyTransform.Count, aggregateY / enemyTransform.Count, aggregateZ / enemyTransform.Count);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject enemyInst = Instantiate(enemyPrefab, spawnTransform.position, Quaternion.identity);
            enemyTransform.Add(enemyInst.transform);
        }
    }
}
