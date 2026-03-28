using UnityEngine;

public class Simple3FrameAnimation : MonoBehaviour
{
    [Header("Sprites for Animation")]
    public Sprite frame1;
    public Sprite frame2;
    public Sprite frame3;

    [Header("Animation Settings")]
    public float frameRate = 5f; // เฟรมต่อวินาที

    private SpriteRenderer spriteRenderer;
    private Sprite[] frames;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject!");
            enabled = false;
            return;
        }

        frames = new Sprite[] { frame1, frame2, frame3 };
        spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame++;
            if (currentFrame >= frames.Length)
                currentFrame = 0;

            spriteRenderer.sprite = frames[currentFrame];
        }
    }
}