using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 14f;
    [SerializeField] private float lifetime = 3f;
    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;

        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void Start()
    {
        // Move in the direction the bullet is facing (up = forward for top-down)
        _rb.linearVelocity = transform.up * speed;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            other.GetComponent<Obstacle>().TakeDamage();
            Destroy(gameObject);
        }
    }
}
