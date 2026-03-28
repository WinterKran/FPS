using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 100;

    public GameObject gameOverUI;

    // ✅ HP Bar
    public Slider hpBar;
    public Image fillImage; // Fill ของ Slider

    private bool isGameOver = false;

    void Start()
    {
        health = maxHealth;
        hpBar.maxValue = maxHealth;
        hpBar.value = health;

        UpdateHPBar();
    }

    public void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        health = Mathf.Max(health, 0);

        Debug.Log("Player HP: " + health);

        UpdateHPBar(); // อัปเดตหลอด + สี

        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateHPBar()
    {
        hpBar.value = health;

        float percent = (float)health / maxHealth;

        if (percent > 0.5f)
            fillImage.color = Color.green;
        else if (percent > 0.2f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }

    void Die()
    {
        Debug.Log("Player Dead");

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;

        isGameOver = true;
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.J))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        void RestartGame()
    {
    Time.timeScale = 1f;
    SceneManager.LoadScene("Open"); // 👈 ใส่ชื่อ Scene เมนู
    }
    }


}