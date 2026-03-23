using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private int score = 0;
    public float scoreMultiplier = 1f;
    
    public float thrustForce = 10f;
    public float maxSpeed = 5f;

    public GameObject boosterFlame;
    public GameObject explosionEffect;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (boosterFlame != null) boosterFlame.SetActive(false);
    }

    void Update()
    {
        // 1. Tính điểm và cập nhật sang UIManager
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateScoreUI(score);
        }

        // 2. Di chuyển bằng chuột
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = ((Vector2)mousePos - (Vector2)transform.position).normalized;
            
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
            
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        // 3. Hiệu ứng lửa
        if (boosterFlame != null)
        {
            boosterFlame.SetActive(Mouse.current.leftButton.isPressed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowGameOverUI();
        }

        Destroy(gameObject);
    }
}