using UnityEngine;
using TMPro;

public class GunUI : MonoBehaviour
{
    public GunSystem gunSystem;

    public TextMeshProUGUI damageLevelText;
    public TextMeshProUGUI fireRateLevelText;
    public TextMeshProUGUI ammoLevelText;

    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI fireRateCostText;
    public TextMeshProUGUI ammoCostText;

    public void RefreshUI()
    {
        if (gunSystem == null) return;

        damageLevelText.text = $"Damage Lv. {gunSystem.damageLevel}";
        fireRateLevelText.text = $"FireRate Lv. {gunSystem.fireRateLevel}";
        ammoLevelText.text = $"Ammo Lv. {gunSystem.ammoLevel}";

        damageCostText.text = (gunSystem.damageLevel >= gunSystem.maxLevel) ? "MAX" : $"${gunSystem.damageCost}";
        fireRateCostText.text = (gunSystem.fireRateLevel >= gunSystem.maxLevel) ? "MAX" : $"${gunSystem.fireRateCost}";
        ammoCostText.text = (gunSystem.ammoLevel >= gunSystem.maxLevel) ? "MAX" : $"${gunSystem.ammoCost}";
    }
}