using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    public int damage = 25;
    public float range = 100f;

    public int maxAmmo = 30;
    private int currentAmmo;

    public float reloadTime = 1.5f;
    private float reloadTimer = 0f;

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
    public int ammoLevel = 1;
    public int maxLevel = 10;

    [Header("Upgrade Cost")]
    public int damageCost = 100;
    public int fireRateCost = 150;
    public int ammoCost = 80;

    public Button damageButton;
    public Button fireRateButton;
    public Button ammoButton;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    public GunUI gunUI;

    void Start()
    {
        currentAmmo = maxAmmo;
        if (reloadText != null)
            reloadText.SetActive(false);

        UpdateUI();
        RefreshUpgradeButtons();
        if (gunUI != null)
            gunUI.RefreshUI();
    }

    void Update()
    {
        HandleReload();

        // ❌ กันยิงตอนเปิดร้าน
        if (ShopManager.instance != null && ShopManager.instance.IsShopOpen())
            return;

        if (!gameObject.activeInHierarchy) return;

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
                Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
            StartReload();

        // อัปเดตปุ่มทุก frame
        RefreshUpgradeButtons();
    }

    void Shoot()
{
    currentAmmo--;
    UpdateUI();

    // 🔊 เล่นเสียงยิง
    if (audioSource != null && shootSound != null)
        audioSource.PlayOneShot(shootSound);

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
    if (currentAmmo >= maxAmmo) return;

    reloadTimer = reloadTime;
    if (reloadText != null)
        reloadText.SetActive(true);
}

void HandleReload()
{
    if (reloadTimer <= 0f) return;

    reloadTimer -= Time.deltaTime;

    if (reloadTimer <= 0f)
    {
        currentAmmo = maxAmmo;
        if (reloadText != null)
            reloadText.SetActive(false);
        UpdateUI();
    }
}

    
    void UpdateUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
            ammoText.color = (currentAmmo <= 5) ? Color.red : Color.white;
        }
    }

    public void UpgradeMaxAmmo(int amount)
    {
        if (ammoLevel >= maxLevel) return; // Max แล้ว

        if (!GameManager.instance.SpendMoney(ammoCost)) return; // เงินไม่พอ

        ammoLevel++;
        maxAmmo += amount;
        currentAmmo = maxAmmo; // เติมเต็มทันที
        ammoCost += 0;

        UpdateUI();
        RefreshUpgradeButtons();

        if (gunUI != null)
            gunUI.RefreshUI();

        Debug.Log($"Ammo upgraded! MaxAmmo = {maxAmmo}");
    }

    public void BuyDamageUpgrade(int amount)
    {
        if (damageLevel >= maxLevel) return;
        if (!GameManager.instance.SpendMoney(damageCost)) return;

        damageLevel++;
        damage += amount;
        damageCost += 50;

        RefreshUpgradeButtons();
        if (gunUI != null)
            gunUI.RefreshUI();
    }

    public void BuyFireRateUpgrade(float amount)
    {
        if (fireRateLevel >= maxLevel) return;
        if (!GameManager.instance.SpendMoney(fireRateCost)) return;

        fireRateLevel++;
        fireRate += amount;
        if (fireRate > 50f) fireRate = 50f;
        fireRateCost += 75;

        RefreshUpgradeButtons();
        if (gunUI != null)
            gunUI.RefreshUI();
    }

    // ✅ ปุ่ม Upgrade จะ Disable ถ้าเงินไม่พอหรือ Max Level
    void RefreshUpgradeButtons()
    {
        if (GameManager.instance == null) return;

        if (damageButton != null)
            damageButton.interactable = damageLevel < maxLevel && GameManager.instance.money >= damageCost;

        if (fireRateButton != null)
            fireRateButton.interactable = fireRateLevel < maxLevel && GameManager.instance.money >= fireRateCost;

        if (ammoButton != null)
            ammoButton.interactable = ammoLevel < maxLevel && GameManager.instance.money >= ammoCost;
    }
}