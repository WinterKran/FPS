using UnityEngine;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
    public GameObject wheelUI;
    public Image[] weaponSlots;
    public Sprite[] highlightedSprites;
    public Image centerWeaponImage;
    public Sprite[] weaponSprites;
    public Transform weaponsParent;

    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;

    private int currentWeapon = 0;
    private int highlightedWeapon = 0;
    private bool wheelActive = false;

    void Start()
    {
        wheelUI.SetActive(false);
        if (centerWeaponImage != null)
            centerWeaponImage.enabled = false;

        SelectWeapon();
        UpdateWeaponUI();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // เปิดวงล้อ
        if (Input.GetKey(KeyCode.Q))
        {
            if (!wheelActive)
            {
                wheelUI.SetActive(true);
                wheelActive = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                if (centerWeaponImage != null)
                    centerWeaponImage.enabled = true;
            }

            HighlightWeaponWithMouse();

            if (centerWeaponImage != null && weaponSprites.Length > highlightedWeapon)
            {
                centerWeaponImage.sprite = weaponSprites[highlightedWeapon];
                centerWeaponImage.color = Color.white;
            }
        }

        // ปิดวงล้อและเลือกอาวุธ
        if (wheelActive && Input.GetKeyUp(KeyCode.Q))
        {
            currentWeapon = highlightedWeapon;
            SelectWeapon();
            UpdateWeaponUI();
            wheelUI.SetActive(false);
            wheelActive = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (centerWeaponImage != null)
                centerWeaponImage.enabled = false;
        }

        // เลือกด้วยปุ่ม 1/2/3
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentWeapon = 0; SelectWeapon(); UpdateWeaponUI(); }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponsParent.childCount >= 2) { currentWeapon = 1; SelectWeapon(); UpdateWeaponUI(); }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponsParent.childCount >= 3) { currentWeapon = 2; SelectWeapon(); UpdateWeaponUI(); }
    }

    void HighlightWeaponWithMouse()
    {
        Vector2 wheelCenter = RectTransformUtility.WorldToScreenPoint(null, wheelUI.transform.position);
        Vector2 dir = (Vector2)Input.mousePosition - wheelCenter;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        int slotCount = weaponSlots.Length;
        float sectorAngle = 360f / slotCount;
        highlightedWeapon = Mathf.FloorToInt((angle + 360f + sectorAngle / 2f) % 360f / sectorAngle);

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i == highlightedWeapon)
            {
                weaponSlots[i].sprite = highlightedSprites[i];
                weaponSlots[i].color = Color.white;
            }
            else if (i == currentWeapon)
            {
                weaponSlots[i].sprite = null;
                weaponSlots[i].color = selectedColor;
            }
            else
            {
                weaponSlots[i].sprite = null;
                weaponSlots[i].color = normalColor;
            }
        }
    }

    void SelectWeapon()
    {
        for (int i = 0; i < weaponsParent.childCount; i++)
            weaponsParent.GetChild(i).gameObject.SetActive(i == currentWeapon);
    }

    void UpdateWeaponUI()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i == currentWeapon)
            {
                weaponSlots[i].sprite = null;
                weaponSlots[i].color = selectedColor;
            }
            else
            {
                weaponSlots[i].sprite = null;
                weaponSlots[i].color = normalColor;
            }
        }

        if (centerWeaponImage != null && weaponSprites.Length > currentWeapon)
        {
            centerWeaponImage.sprite = weaponSprites[currentWeapon];
            centerWeaponImage.color = Color.white;
        }
    }
}