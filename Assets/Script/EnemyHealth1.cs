using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int rewardMoney = 100;

    private int health;

    public event Action OnDeath; // ให้ Spawner รู้

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // แจ้ง Spawner
        OnDeath?.Invoke();

        // ✅ ให้ Player / GameManager ได้เงิน
        if (GameManager.instance != null)
        {
            GameManager.instance.AddMoney(rewardMoney);
        }

        Destroy(gameObject);
    }
}