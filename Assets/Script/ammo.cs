using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public GunSystem gun;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI costText;

    public Button upgradeButton;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
{
    if (gun == null || GameManager.instance == null) return;

    if (levelText != null)
        levelText.text = "Ammo Lv. " + gun.ammoLevel;

    if (ammoText != null)
        ammoText.text = "Capacity: " + gun.maxAmmo; // แสดง Max Ammo ปัจจุบัน

    if (costText != null)
        costText.text = gun.ammoLevel >= gun.maxLevel ? "MAX" : "$" + gun.ammoCost;

    if (upgradeButton != null)
        upgradeButton.interactable =
            GameManager.instance.money >= gun.ammoCost &&
            gun.ammoLevel < gun.maxLevel;
}

    // ผูกกับปุ่ม UI
    public void OnClickUpgrade()
{
    if (gun == null) return;

    gun.UpgradeMaxAmmo(5); // เพิ่ม Capacity ทีละ 5 นัด
    RefreshUI();
}
}