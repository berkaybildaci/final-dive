using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerkayEnemySpawner : MonoBehaviour
{
    public GameObject enemyContainer;
    public GameObject enemyPrefab;
    private List<GameObject> enemySpawningPoints = new List<GameObject>();
    public float enemyCooldownTime;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            enemySpawningPoints.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= enemyCooldownTime)
        {
            startTime = Time.time;
            enemyCooldownTime = Random.Range(5, 15);
            GameObject newEnemy = Instantiate(enemyPrefab, enemySpawningPoints[Random.Range(0, enemySpawningPoints.Count - 1)].transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(enemyContainer.transform);
            Debug.Log("enemy spawned in");
        }
    }
}
