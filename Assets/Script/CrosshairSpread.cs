using UnityEngine;

public class CrosshairSpread : MonoBehaviour
{
    public float normalSize = 20f;
    public float shootSize = 40f;
    public float speed = 10f;

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float target = Input.GetButton("Fire1") ? shootSize : normalSize;

        rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(target, target), Time.deltaTime * speed);
    }
}