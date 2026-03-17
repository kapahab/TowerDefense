using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Mesh normalMesh, selectedMesh;

    private MeshFilter meshFilter;

    [SerializeField] private GameObject towerPrefab; //for now
    private GameObject towerInstance;

    public static Action<GameObject> OnTowerSpawned;

    private bool occupied = false;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (occupied && towerInstance == null)
        {
            occupied = false;
            meshFilter.mesh = normalMesh;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnMouseEnter()
    {
        if (occupied)
            return;

        meshFilter.mesh = selectedMesh;
    }

    void OnMouseExit()
    {
        if (occupied)
            return;

        meshFilter.mesh = normalMesh;
    }

    private void OnMouseDown()
    {
        if (occupied)
            return;
        towerInstance = Instantiate(towerPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        OnTowerSpawned?.Invoke(towerInstance);
        occupied = true;
        meshFilter.mesh = null;
    }


}
