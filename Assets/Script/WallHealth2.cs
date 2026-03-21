using UnityEngine;

public class WallHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // ฟังก์ชัน Reset Health สำหรับ Shop
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Wall health reset!");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth == 0)
        {
            Debug.Log("Wall destroyed!");
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}