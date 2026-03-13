using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public int damage = 25;
    public float range = 100f;

    public int maxAmmo = 30;
    private int currentAmmo;

    public float reloadTime = 1.5f;
    private bool isReloading = false;

    public HitMarkerUI hitMarker;

    public Camera fpsCam;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        currentAmmo--;

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
{
    EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

    if (enemy != null)
    {
        enemy.TakeDamage(damage);
        hitMarker.ShowHitMarker();
    }
}
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloading = false;
    }
}