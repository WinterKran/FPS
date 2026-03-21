using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private int currentEnemies = 0;
    private int enemiesToSpawn = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartWave(int waveNumber, int count)
    {
        enemiesToSpawn = count;
        currentEnemies = 0;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (currentEnemies < enemiesToSpawn)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f); // spawn ทุก 1 วินาที
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemies++;

        // Subscribe ให้ลดจำนวนเมื่อศัตรูตาย
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()
    {
        currentEnemies--;
        if (currentEnemies <= 0 && enemiesToSpawn > 0)
        {
            // จบ Wave
            WaveManager.instance.WaveCompleted();
        }
    }
}