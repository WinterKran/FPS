using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float spawnTime = 5f;
    public int maxEnemies = 10;

    private int currentEnemies = 0;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnTime);
    }

    void SpawnEnemy()
    {
        if (currentEnemies >= maxEnemies)
            return;

        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Transform spawnPoint = spawnPoints[spawnIndex];

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemies++;
    }
}