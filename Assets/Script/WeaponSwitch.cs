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
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentWeapon++;

            if (currentWeapon >= transform.childCount)
            {
                currentWeapon = 0;
            }

            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == currentWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            i++;
        }
    }
}