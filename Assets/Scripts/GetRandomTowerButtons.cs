using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetRandomTowerButtons : MonoBehaviour
{
    [SerializeField] private List<GameObject> towerButtons;
    public Cell cell;
    void Start()
    {
        PickXButton();
    }

    private void PickXButton()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject randomTower = towerButtons[Random.Range(0, towerButtons.Count)];
            GameObject instTower = Instantiate(randomTower,transform);
            instTower.GetComponent<TowerSelectionButton>().cell = cell;
            towerButtons.Remove(randomTower);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
