using UnityEngine;

public class UIOpener : MonoBehaviour
{
    public GameObject targetUI; // UI ที่จะเปิด

    public void ShowUI()
    {
        if (targetUI != null)
            targetUI.SetActive(true);
    }

    public void HideUI()
    {
        if (targetUI != null)
            targetUI.SetActive(false);
    }

    public void ToggleUI()
    {
        if (targetUI != null)
            targetUI.SetActive(!targetUI.activeSelf);
    }
}