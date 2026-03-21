using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    // จุดที่ไอเทมจะเกิด / ติดมือ Player
    public Transform itemHolder;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Vector3 GetItemSpawnPosition()
    {
        if (itemHolder != null)
            return itemHolder.position;
        return transform.position + Vector3.forward; // fallback
    }
}