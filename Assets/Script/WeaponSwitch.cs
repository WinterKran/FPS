using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] weapons;       // ปืนจริง
    public Image[] weaponImages;       // UI Image ของปืน
    private int currentWeapon = 0;
    private bool isHoldingF = false;

    void Start()
    {
        SelectWeapon();
        UpdateWeaponUI();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isHoldingF = true;
            ShowWeaponUI(true);

            // Highlight UI ปืนถัดไป
            int nextWeapon = (currentWeapon + 1) % weapons.Length;
            HighlightWeapon(nextWeapon);
        }

        if (isHoldingF && Input.GetKeyUp(KeyCode.F))
        {
            // ปล่อย F → เปลี่ยนปืนจริง
            currentWeapon = (currentWeapon + 1) % weapons.Length;
            SelectWeapon();
            UpdateWeaponUI();
            ShowWeaponUI(false);
            isHoldingF = false;
        }

        // เลือกด้วยปุ่ม 1/2/3
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentWeapon = 0; SelectWeapon(); UpdateWeaponUI(); }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length >= 2) { currentWeapon = 1; SelectWeapon(); UpdateWeaponUI(); }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Length >= 3) { currentWeapon = 2; SelectWeapon(); UpdateWeaponUI(); }
    }

    void SelectWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(i == currentWeapon);
    }

    void UpdateWeaponUI()
    {
        for (int i = 0; i < weaponImages.Length; i++)
            weaponImages[i].color = (i == currentWeapon) ? Color.green : Color.white;
    }

    void ShowWeaponUI(bool show)
    {
        foreach (var img in weaponImages)
            img.gameObject.SetActive(show);
    }

    void HighlightWeapon(int index)
    {
        for (int i = 0; i < weaponImages.Length; i++)
            weaponImages[i].color = (i == index) ? Color.yellow : Color.white;
    }
}