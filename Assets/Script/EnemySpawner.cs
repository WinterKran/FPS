using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; // <-- ใส่ Enemy หลายแบบ
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnInterval = 1f;

    private int currentEnemies = 0;
    private int enemiesToSpawn = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // เริ่ม Wave
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
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0) return;

        // เลือก Spawn Point แบบสุ่ม
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // เลือก Enemy แบบสุ่ม
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[enemyIndex];

        // Spawn Enemy
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

        // ตรวจสอบ Wave จบ
        if (currentEnemies <= 0 && enemiesToSpawn > 0)
        {
            WaveManager.instance.WaveCompleted();
        }
    }
}