using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    public GameObject shopPanel;
    public TextMeshProUGUI promptText;
    public Button[] buyButtons;
    public TextMeshProUGUI[] itemNames;
    public TextMeshProUGUI[] itemPricesText;
    public TextMeshProUGUI[] itemDescriptionText;
    public AudioSource audioSource;
    public AudioClip buySound;
    public AudioClip failSound;

    [Header("Item Settings")]
    public string[] itemNamesData;
    public string[] itemDescriptions;
    public int[] itemPrices;
    public GameObject[] itemPrefabs;

    private bool playerInRange = false;

    public static ShopManager instance;

    void Awake()
    {
        Debug.Log("ShopManager Awake called");
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        shopPanel.SetActive(false);
        promptText.gameObject.SetActive(false);

        for (int i = 0; i < buyButtons.Length; i++)
        {
            if (i >= itemNamesData.Length) continue;

            int index = i;

            if (i < itemNames.Length) itemNames[i].text = itemNamesData[i];
            if (i < itemPricesText.Length) itemPricesText[i].text = "$" + itemPrices[i];
            if (i < itemDescriptionText.Length) itemDescriptionText[i].text = itemDescriptions[i];

            buyButtons[i].onClick.RemoveAllListeners();
            buyButtons[i].onClick.AddListener(() => BuyItem(index));
        }

        // อัปเดต UI ตอนเริ่มเกม
        UpdateItemUI();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
            ToggleShop();
    }

    void ToggleShop()
    {
        if (shopPanel.activeSelf) CloseShop();
        else OpenShop();
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (PlayerInventory.instance == null)
        {
            Debug.LogWarning("PlayerInventory.instance is null!");
            return;
        }

        FPSPlayerController controller = PlayerInventory.instance.GetComponent<FPSPlayerController>();
        if (controller == null)
        {
            Debug.LogWarning("FPSPlayerController not found on PlayerInventory.instance");
            return;
        }

        controller.enabled = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerInventory.instance != null)
        {
            FPSPlayerController controller = PlayerInventory.instance.GetComponent<FPSPlayerController>();
            if (controller != null) controller.enabled = true;
        }
    }

    void BuyItem(int index)
{
    Debug.Log("Trying to buy item " + index);

    int price = itemPrices[index];

    if (GameManager.instance.money >= price)
    {
        GunSystem gun = FindObjectOfType<GunSystem>();

        switch (index)
        {
            case 0:
                WallHealth wall = FindObjectOfType<WallHealth>();
                if (wall != null)
                    wall.ResetHealth();
                break;

            case 1:
                if (gun != null)
                    gun.IncreaseMaxAmmo(10);
                break;

            case 2: // Damage Upgrade
                if (gun != null)
                {
                    if (gun.damageLevel >= gun.maxLevel)
                    {
                        Debug.Log("Damage Max Level!");
                        return;
                    }

                    gun.UpgradeDamagePercent(20f); // เพิ่ม 20% ต่อ Level

                    itemPrices[index] += 50;
                    itemPricesText[index].text = "$" + itemPrices[index];
                }
                break;

            case 3: // FireRate Upgrade
                if (gun != null)
                {
                    if (gun.fireRateLevel >= gun.maxLevel)
                    {
                        Debug.Log("FireRate Max Level!");
                        return;
                    }

                    gun.UpgradeFireRatePercent(15f); // เพิ่ม 15% ต่อ Level

                    itemPrices[index] += 50;
                    itemPricesText[index].text = "$" + itemPrices[index];
                }
                break;
        }

        // หักเงิน
        GameManager.instance.AddMoney(-price);

        if (audioSource != null && buySound != null)
            audioSource.PlayOneShot(buySound);

        // อัปเดต UI หลังซื้อ
        UpdateItemUI();
    }
    else
    {
        if (audioSource != null && failSound != null)
            audioSource.PlayOneShot(failSound);
    }
}




   void UpdateItemUI()
{
    GunSystem gun = FindObjectOfType<GunSystem>();
    if (gun == null) return;

    // Damage
    if (itemNames.Length > 2)
    {
        if (gun.damageLevel >= gun.maxLevel)
        {
            itemNames[2].text = "Damage (MAX)";
            buyButtons[2].interactable = false;
        }
        else
        {
            itemNames[2].text = itemNamesData[2] + " (Lv." + gun.damageLevel + ")";
            buyButtons[2].interactable = true;
        }
    }

    // FireRate
    if (itemNames.Length > 3)
    {
        if (gun.fireRateLevel >= gun.maxLevel)
        {
            itemNames[3].text = "FireRate (MAX)";
            buyButtons[3].interactable = false;
        }
        else
        {
            itemNames[3].text = itemNamesData[3] + " (Lv." + gun.fireRateLevel + ")";
            buyButtons[3].interactable = true;
        }
    }
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptText.gameObject.SetActive(false);
        }
    }

    public bool IsShopOpen()
    {
        return shopPanel.activeSelf;
    }

    
}