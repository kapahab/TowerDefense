using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerSelectionButton : MonoBehaviour, ISubmitHandler
{
    public GameObject towerPrefab;
    public Cell cell;
    Button button;
    public int towerCost = 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(InstantiateTower);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InstantiateTower()
    {
        if (cell != null)
        {
            if (EconomyManager.TrySpendGold(towerCost))
                cell.InstantiateTower(towerPrefab);
            else
            {
                Debug.Log("not enough money for tower");
            }
        }
        else
            Debug.Log("cell is null in TowerSelectionButton");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        InstantiateTower();
    }
}
