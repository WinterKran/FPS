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

    public LayerMask playerLayer; // ให้ยิงโดนเฉพาะ Player

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag("Wall");

        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        if (!agent.isOnNavMesh || player == null || firePoint == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // เดินตาม
        if (distance <= lookRadius)
        {
            agent.SetDestination(player.position);
        }

        // อยู่ในระยะยิง
        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position); // หยุดเดิน
            LookAtPlayer();

            if (Time.time >= nextTimeToFire)
            {
                Shoot();
                nextTimeToFire = Time.time + 1f / fireRate;
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // ป้องกันให้ไม่เงย/ก้ม
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100f, playerLayer))
        {
            PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * 100f, Color.red, 0.5f); // ดู Raycast
    }

    // วาด Gizmos สำหรับ Look Radius และ Attack Range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}