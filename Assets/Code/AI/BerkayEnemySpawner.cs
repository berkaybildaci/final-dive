using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BerkayEnemySpawner : MonoBehaviour
{
    public static BerkayEnemySpawner instance;
    public GameObject enemyContainer;
    public GameObject enemyPrefab;
    private List<GameObject> enemySpawningPoints = new List<GameObject>();
    public float enemyCooldownTime;
    private float startTime;

    public float winTime;
    public float winMaxTime;
    public TMP_Text winTimerText;
    public Image timerFill;
    public float enemyCount;
    public float enemyKillCount;
    public TMP_Text killCountText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        startTime = Time.time;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            enemySpawningPoints.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        winTime -= Time.deltaTime;
        winTimerText.text = "" + winTime;
        timerFill.fillAmount = winTime / winMaxTime;
        
        if(winTime<0)
        {
            if(enemyCount > 10)
            {
                enemyCount = 0;
                winTime = winMaxTime;
            }
            int index = Random.Range(0, enemySpawningPoints.Count);
            GameObject newEnemy = Instantiate(enemyPrefab, enemySpawningPoints[index].transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(enemySpawningPoints[index].transform);
            enemyCount++;
            
        }


        if (Time.time - startTime >= enemyCooldownTime)
        {
            startTime = Time.time;
            enemyCooldownTime = Random.Range(3, 5);
            int index = Random.Range(0, enemySpawningPoints.Count);
            GameObject newEnemy = Instantiate(enemyPrefab, enemySpawningPoints[index].transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(enemySpawningPoints[index].transform);
        }
    }

    public void UpdateKillCount()
    {
        enemyKillCount++;
        killCountText.text = "Kills: " + enemyKillCount;
    }
}
