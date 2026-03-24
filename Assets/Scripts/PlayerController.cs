using UnityEngine;
using UnityEngine.InputSystem; // Bắt buộc phải có

public class PlayerController : MonoBehaviour
{
    // 1. Khai báo các biến Input Action
    public InputAction moveForward;
    public InputAction lookPosition;

    [Header("Movement Settings")]
    public float thrustForce = 10f;
    public float maxSpeed = 5f;
    public float scoreMultiplier = 1f;

    [Header("References")]
    public GameObject boosterFlame;
    public GameObject explosionEffect;
    public GameObject borderParent;

    private Rigidbody2D rb;
    private float elapsedTime = 0f;
    private int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 3. Kích hoạt các Action khi bắt đầu
        moveForward.Enable();
        lookPosition.Enable();

        if (boosterFlame != null) boosterFlame.SetActive(false);
    }

    void Update()
    {
        // Tính điểm và gửi sang UIManager
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        if (UIManager.instance != null) UIManager.instance.UpdateScoreUI(score);

        // 4. Cập nhật Logic Input mới
        // Thay thế Mouse.current.leftButton.isPressed
        if (moveForward.IsPressed())
        {
            // Thay thế Mouse.current.position.value
            Vector2 inputPos = lookPosition.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputPos);
            
            Vector2 direction = ((Vector2)worldPos - (Vector2)transform.position).normalized;
            
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
            
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        // Xử lý bật/tắt lửa Booster (Mobile/Mouse compatible)
        if (moveForward.WasPressedThisFrame())
        {
            if (boosterFlame != null) boosterFlame.SetActive(true);
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            if (boosterFlame != null) boosterFlame.SetActive(false);
        }

        // Hiệu ứng Flicker cho Booster
        if (boosterFlame != null && boosterFlame.activeSelf)
        {
            float flicker = 1f + Mathf.Sin(Time.time * 20f) * 0.2f;
            boosterFlame.transform.localScale = new Vector3(flicker, flicker, 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (borderParent != null) borderParent.SetActive(false);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        if (UIManager.instance != null) UIManager.instance.ShowGameOverUI();
        Destroy(gameObject);
    }
}