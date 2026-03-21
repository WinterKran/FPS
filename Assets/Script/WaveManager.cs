using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    public int maxWaves = 8;
    public int enemyIncrement = 2;

    [Header("UI")]
    public GameObject waveUI;
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public TextMeshProUGUI waveText;
    public Button continueButton;

    [Header("Player")]
    public Transform player;
    public Transform spawnPoint;

    private bool waveFinished = false;
    private bool gameOver = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        waveUI.SetActive(false);
        gameOverUI.SetActive(false);
        victoryUI.SetActive(false);

        continueButton.onClick.AddListener(NextWave);

        EnemySpawner.instance.StartWave(currentWave, enemiesPerWave);
    }

    void Update()
{
    if (waveFinished && Input.GetKeyDown(KeyCode.T))
    {
        NextWave();
    }

    // 👉 กด J เพื่อรีเซ็ตเกม (ตอนจบเท่านั้น)
    if (gameOver && Input.GetKeyDown(KeyCode.J))
    {
        RestartGame();
    }
}

    public void WaveCompleted()
    {
        waveFinished = true;

        waveUI.SetActive(true);
        waveText.text = "Wave " + currentWave + " Completed!";
    }

    public void NextWave()
    {
        if (gameOver) return;

        waveFinished = false;
        waveUI.SetActive(false);

        currentWave++;

        if (currentWave > maxWaves)
        {
            // 🟩 Victory
            gameOver = true;
            victoryUI.SetActive(true);
            continueButton.gameObject.SetActive(false);
            Debug.Log("Victory!");
            return;
        }

        // 👉 รีเซ็ต Player
        ResetPlayerPosition();

        int enemiesThisWave = enemiesPerWave + (currentWave - 1) * enemyIncrement;
        EnemySpawner.instance.StartWave(currentWave, enemiesThisWave);
    }

    void ResetPlayerPosition()
    {
        if (player != null && spawnPoint != null)
        {
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
        }
    }

    public void GameOver()
    {
        if (gameOver) return;

        gameOver = true;

        waveUI.SetActive(false);
        gameOverUI.SetActive(true);
        continueButton.gameObject.SetActive(false);

        Debug.Log("Game Over!");
    }

    void RestartGame()
{
    // รีเซ็ตค่า Wave
    currentWave = 1;
    gameOver = false;
    waveFinished = false;

    // ซ่อน UI ทั้งหมด
    waveUI.SetActive(false);
    gameOverUI.SetActive(false);
    victoryUI.SetActive(false);

    // เปิดปุ่ม Continue กลับมา
    continueButton.gameObject.SetActive(true);

    // รีเซ็ต Player
    ResetPlayerPosition();

    // เริ่ม Wave ใหม่
    EnemySpawner.instance.StartWave(currentWave, enemiesPerWave);

    Debug.Log("Restart Game!");
}
}