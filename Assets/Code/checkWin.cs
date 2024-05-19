using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckWin : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI timer;
    public Image time;


    private float gameTime = 100f; // Total game time in seconds
    private float currentTime = 0f; // Current time elapsed
    private bool gameEnded = false; // Flag to track if the game has ended
    EntityData playerEntityData;


    void Start()
    {
        text.enabled = false;
        timer.enabled=true;
        playerEntityData = GetComponent<EntityData>();
    }

    void Update()
    {
        if (!gameEnded)
        {
            HandleGameTimer();
            CheckVictoryConditions();
        }
    }

    private void HandleGameTimer()
    {
        currentTime += Time.deltaTime; // Increment the current time by the time elapsed since the last frame
        float remainingTime = Mathf.Max(gameTime - currentTime, 0f);
        timer.text = ""+ remainingTime.ToString("F1");
        time.fillAmount=1-currentTime/gameTime;


        // Update UI elements or perform other actions based on the current time
        if (currentTime >= gameTime)
        {
            EndGame();
        }
        else if((int)playerEntityData.health<=0) {
            lose();
        }
    }

    private void CheckVictoryConditions()
    {
        // Check other victory conditions if needed
    }

    private void EndGame()
{
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyContainer");
    foreach (GameObject enemy in enemies)
    {
        Destroy(enemy);
    }
    GameObject[] enemie = GameObject.FindGameObjectsWithTag("TEAM");
    foreach (GameObject enemy in enemie)
    {
        Destroy(enemy);
    }
    GameObject[] enemi = GameObject.FindGameObjectsWithTag("spawner");
    foreach (GameObject enemy in enemi)
    {
        Destroy(enemy);
    }
    

    // Update UI to display win message
    timer.enabled=false;
    text.enabled = true;
    text.text = "Level 1 Complete";

  

}
private void lose()
{
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyContainer");
    foreach (GameObject enemy in enemies)
    {
        Destroy(enemy);
    }
    GameObject[] enemie = GameObject.FindGameObjectsWithTag("TEAM");
    foreach (GameObject enemy in enemie)
    {
        Destroy(enemy);
    }
    GameObject[] enemi = GameObject.FindGameObjectsWithTag("spawner");
    foreach (GameObject enemy in enemi)
    {
        Destroy(enemy);
    }
    
    time.fillAmount=0;

    // Update UI to display win message
    timer.enabled=false;
    text.enabled = true;
    text.text = "You Lose";
    Time.timeScale = 0f;
  

}
}
