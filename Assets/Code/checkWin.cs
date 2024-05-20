using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // Add this line to include the IEnumerator type

public class CheckWin : MonoBehaviour
{
    
    public TextMeshProUGUI text;
    public TextMeshProUGUI timer;
    public Image time;

    public Image loseImage; // Reference to the Canvas that contains the "You Lose" screen
    public TextMeshProUGUI loses; // Reference to the Canvas that contains the "You Lose" screen
    public Image WinImage; // Reference to the Canvas that contains the "You Lose" screen
    public TextMeshProUGUI Wins; // Reference to the Canvas that contains the "You Lose" screen

    private float gameTime = 100f; // Total game time in seconds
    private float currentTime = 0f; // Current time elapsed
    private bool gameEnded = false; // Flag to track if the game has ended
    EntityData playerEntityData;

    // Coordinate ranges for the player to win the game
    public float minX = 33f;
    public float maxX = 45f;
    public float minY = -4f;
    public float maxY = 8f;
    public float minZ = -121.3f;
    public float maxZ = -119.3f;

    void Start()
    {
        
        loses.enabled = false;
        WinImage.enabled=false;
        Wins.enabled=false;
        text.enabled = false;
        timer.enabled = true;
        playerEntityData = GetComponent<EntityData>();
        loseImage.enabled = false; // Disable the lose screen Canvas initially
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
        timer.text = remainingTime.ToString("F1");
        time.fillAmount = 1 - currentTime / gameTime;

        // Update UI elements or perform other actions based on the current time
        if (currentTime >= gameTime)
        {
            EndGame();
        }
        else if ((int)playerEntityData.health <= 0)
        {
            lose();
        }
    }

    private void CheckVictoryConditions()
    {
        Vector3 playerPosition = transform.position;

        // Check if the player is within the specified coordinate ranges
        if (playerPosition.x >= minX && playerPosition.x <= maxX &&
            playerPosition.y >= minY && playerPosition.y <= maxY &&
            playerPosition.z >= minZ && playerPosition.z <= maxZ)
        {
            Win();
        }
    }

    private void EndGame()
    {
        gameEnded = false; // Set the flag to prevent further updates

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
        timer.enabled = false;
        text.enabled = true;
        text.text = "Level 1 Complete";

    }
    

    private void lose()
    {
        gameEnded = true; // Set the flag to prevent further updates

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

        time.fillAmount = 0;

        // Update UI to display lose message
        timer.enabled = false;
        text.enabled = false;

        // Show the "You Lose" screen
        loseImage.enabled = true;
        loses.enabled = true;
        
        Time.timeScale = 0f; // Pause the game
    }
    private void Win()
    {
        gameEnded = true; // Set the flag to prevent further updates

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

        time.fillAmount = 0;

        // Update UI to display lose message
        timer.enabled = false;
        text.enabled = false;

        // Show the "You Lose" screen
        WinImage.enabled = true;
        Wins.enabled = true;
        
        Time.timeScale = 0f; // Pause the game
    }
}
