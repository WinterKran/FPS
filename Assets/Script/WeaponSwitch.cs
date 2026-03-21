using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    int currentWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        // กด F → สลับอาวุธ
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentWeapon++;

            if (currentWeapon >= transform.childCount)
            {
                currentWeapon = 0;
            }

            SelectWeapon();
        }

        // (เพิ่ม) เลือกอาวุธด้วยปุ่ม 1 / 2 / 3
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
            SelectWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            currentWeapon = 1;
            SelectWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            currentWeapon = 2;
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == currentWeapon);
            i++;
        }
    }
}