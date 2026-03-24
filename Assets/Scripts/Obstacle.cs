using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    public float maxSpinSpeed = 10f;
    public float maxAllowedSpeed = 10f; // Tốc độ tối đa cho phép trước khi tạo hiệu ứng va chạm

    [SerializeField] private GameObject bounceEffectPrefab;
    Rigidbody2D rb;

    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        rb = GetComponent<Rigidbody2D>();
        
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.AddForce(randomDirection * randomSpeed);

        rb.AddTorque(Random.Range(-maxSpinSpeed, maxSpinSpeed));
    }

    void FixedUpdate()
    {
        // Kiểm tra nếu tốc độ hiện tại vượt quá giới hạn
        if (rb.linearVelocity.magnitude > maxAllowedSpeed)
        {
            // Ép tốc độ quay về mức tối đa nhưng vẫn giữ nguyên hướng bay
            rb.linearVelocity = rb.linearVelocity.normalized * maxAllowedSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (bounceEffectPrefab != null)
        {
            Vector2 contactPoint = collision.GetContact(0).point; 
            Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);
        }
    }
}