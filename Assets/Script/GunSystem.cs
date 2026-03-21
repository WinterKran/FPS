using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public int damage = 25;
    public float range = 100f;

    public int maxAmmo = 30;
    private int currentAmmo;

    public float reloadTime = 1.5f;
    private bool isReloading = false;
    private float reloadTimer = 0f;

    // ✅ โหมดยิง
    public bool isAutomatic = false;
    public float fireRate = 10f; // ยิงต่อวินาที
    private float nextTimeToFire = 0f;

    public HitMarkerUI hitMarker;
    public Camera fpsCam;

    public TextMeshProUGUI ammoText;
    public GameObject reloadText;

    [Header("Upgrade Levels")]
    public int damageLevel = 1;
    public int fireRateLevel = 1;
    public int maxLevel = 5;

    void Start()
    {
        currentAmmo = maxAmmo;
        reloadText.SetActive(false);
        UpdateUI();
    }

    void Update()
{
    HandleReload();

    // ❌ เพิ่มบล็อกเมื่อ Shop Panel เปิด
    if (ShopManager.instance != null && ShopManager.instance.IsShopOpen())
        return;

    if (!gameObject.activeInHierarchy) return;
    if (isReloading) return;

    if (currentAmmo <= 0)
    {
        StartReload();
        return;
    }

    if (isAutomatic)
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
    else
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    if (Input.GetKeyDown(KeyCode.R))
    {
        StartReload();
    }
}

    void Shoot()
    {
        currentAmmo--;
        UpdateUI();

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

    void StartReload()
    {
        if (isReloading) return;

        isReloading = true;
        reloadTimer = reloadTime;
        reloadText.SetActive(true);
    }

    void HandleReload()
    {
        if (!isReloading) return;

        reloadTimer -= Time.deltaTime;

        if (reloadTimer <= 0f)
        {
            currentAmmo = maxAmmo;
            isReloading = false;

            reloadText.SetActive(false);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;

        if (currentAmmo <= 5)
            ammoText.color = Color.red;
        else
            ammoText.color = Color.white;
    }

    public void IncreaseMaxAmmo(int amount)
{
    maxAmmo += amount;
    currentAmmo = maxAmmo; // เติมกระสุนเต็มด้วย
    UpdateUI();
}



public void IncreaseDamage(int amount)
{
    damage += amount;
    Debug.Log("Damage Increased: " + damage);
}

public void IncreaseFireRate(float amount)
{
    fireRate += amount;

    // กันยิงเร็วเกินไป
    if (fireRate > 50f)
        fireRate = 50f;

    Debug.Log("Fire Rate Increased: " + fireRate);
}

public void UpgradeDamage(int amount)
{
    if (damageLevel >= maxLevel)
    {
        Debug.Log("Damage MAX LEVEL");
        return;
    }

    damageLevel++;
    damage += amount;

    Debug.Log("Damage Lv." + damageLevel + " | Damage: " + damage);
}

public void UpgradeFireRate(float amount)
{
    if (fireRateLevel >= maxLevel)
    {
        Debug.Log("FireRate MAX LEVEL");
        return;
    }

    fireRateLevel++;
    fireRate += amount;

    if (fireRate > 50f)
        fireRate = 50f;

    Debug.Log("FireRate Lv." + fireRateLevel + " | Rate: " + fireRate);
}

// GunSystem.cs
public void UpgradeDamagePercent(float percent)
{
    if (damageLevel >= maxLevel) return;

    damageLevel++;
    damage = Mathf.RoundToInt(damage * (1 + percent / 100f));

    Debug.Log("Damage Lv." + damageLevel + " | Damage: " + damage);
}

public void UpgradeFireRatePercent(float percent)
{
    if (fireRateLevel >= maxLevel) return;

    fireRateLevel++;
    fireRate *= (1 + percent / 100f);
    if (fireRate > 50f) fireRate = 50f;

    Debug.Log("FireRate Lv." + fireRateLevel + " | Rate: " + fireRate);
}
}