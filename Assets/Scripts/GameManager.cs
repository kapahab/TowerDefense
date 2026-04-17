using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int currentGold;
    public int startGold;
    void Start()
    {
        GiveStartGold();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GiveStartGold()
    {
        currentGold += startGold;
    }
}
