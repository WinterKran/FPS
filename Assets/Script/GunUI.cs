using UnityEngine;
using TMPro;

public class GunUI : MonoBehaviour
{
    public GunSystem gunSystem; // Reference to your GunSystem
    public TextMeshProUGUI damageLevelText;
    public TextMeshProUGUI fireRateLevelText;

    void Update()
    {
        if (gunSystem == null) return;

        damageLevelText.text = "Damage Lv. " + gunSystem.damageLevel;
        fireRateLevelText.text = "FireRate Lv. " + gunSystem.fireRateLevel;
    }
}