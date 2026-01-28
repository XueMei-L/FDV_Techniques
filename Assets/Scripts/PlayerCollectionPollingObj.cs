using UnityEngine;
using UnityEngine.UI;

public class PlayerCollect : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 300f;
    private Rigidbody2D rb2d;
    private bool isJumping = false;

    [Header("Score UI")]
    public Text scoreText;
    private int score = 0;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
            Debug.LogError("Rigidbody2D missing on player!");

        UpdateScoreUI();
    }

    void Update()
    {
        // Jump
        if (Input.GetButton("Jump") && !isJumping)
        {
            rb2d.AddForce(Vector2.up * jumpForce);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("NoCollis"))
        {
            isJumping = false;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);
        }

        // Optional: parent to moving platform
        if (collision.gameObject.CompareTag("Platform"))
            transform.SetParent(collision.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            Invoke(nameof(DetachFromPlatform), 0.01f);
    }

    private void DetachFromPlatform()
    {
        transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            score += 10;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
