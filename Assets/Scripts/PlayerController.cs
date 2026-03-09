using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float frameRate = 8f; 
    public SpriteRenderer spriteRenderer;
    public Sprite[] allSprites; 

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float frameTimer;
    private int currentFrame; 
    private int directionOffset; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        UpdateDirection();
        PlayAnimation();
    }

    void UpdateDirection()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            if (moveInput.y < 0 && moveInput.x == 0) directionOffset = 0;   
            else if (moveInput.y < 0 && moveInput.x > 0) directionOffset = 9;  
            else if (moveInput.x > 0 && moveInput.y == 0) directionOffset = 18; 
            else if (moveInput.y > 0 && moveInput.x > 0) directionOffset = 27; 
            else if (moveInput.y > 0 && moveInput.x == 0) directionOffset = 36; 
            else if (moveInput.y > 0 && moveInput.x < 0) directionOffset = 45; 
            else if (moveInput.x < 0 && moveInput.y == 0) directionOffset = 54; 
            else if (moveInput.y < 0 && moveInput.x < 0) directionOffset = 63; 
        }
    }

    void PlayAnimation()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            frameTimer += Time.deltaTime;
            if (frameTimer >= (1f / frameRate))
            {
                frameTimer = 0;
                currentFrame++;
                if (currentFrame > 8) currentFrame = 1; 
            }
        }
        else
        {
            currentFrame = 0; 
            frameTimer = 0;
        }

        if (spriteRenderer != null && allSprites.Length > (directionOffset + currentFrame))
        {
            spriteRenderer.sprite = allSprites[directionOffset + currentFrame];
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}