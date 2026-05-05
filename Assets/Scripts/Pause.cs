using UnityEngine;

public class Pause : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
    }
}
