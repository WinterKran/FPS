using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float lookRadius = 20f;
    public float attackRange = 10f;
    public int damage = 10;
    public float fireRate = 1f;

    private float nextTimeToFire = 0f;

    public Transform player;
    public Transform firePoint;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        if (!agent.isOnNavMesh) return;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // เดินตาม
        if (distance <= lookRadius)
        {
            agent.SetDestination(player.position);
        }

        // เมื่ออยู่ในระยะยิง
        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position); // หยุดเดิน
            transform.LookAt(player);

            if (Time.time >= nextTimeToFire)
            {
                Shoot();
                nextTimeToFire = Time.time + 1f / fireRate;
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100f))
        {
            PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}