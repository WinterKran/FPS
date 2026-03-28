using UnityEngine;
using System.Collections.Generic;

public class UIOpenerManager : MonoBehaviour
{
    public static UIOpenerManager instance;

    private GameObject currentUI; // UI ที่เปิดอยู่ตอนนี้

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // เปิด UI ใหม่ และปิดอันเก่า
    public void ShowUI(GameObject newUI)
    {
        if (currentUI != null && currentUI != newUI)
            currentUI.SetActive(false); // ปิด UI เก่า

        currentUI = newUI;

        if (currentUI != null)
            currentUI.SetActive(true);
    }

    // ปิด UI ปัจจุบัน
    public void HideCurrentUI()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
            currentUI = null;
        }
    }

    // Toggle UI ใหม่ (ถ้าเปิดอยู่แล้วจะปิด)
    public void ToggleUI(GameObject newUI)
    {
        if (currentUI == newUI)
        {
            HideCurrentUI();
        }
        else
        {
            ShowUI(newUI);
        }
    }
}