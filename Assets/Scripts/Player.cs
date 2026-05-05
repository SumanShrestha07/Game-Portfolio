using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 14f;
    
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            SpawnBullet();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    private void SpawnBullet()
    {
        Instantiate(bulletPrefab, new Vector3(bulletSpawnPosition.position.x,bulletSpawnPosition.position.y,0), Quaternion.identity);
        
    }

    private void MoveLeft()
    {
        rb.AddForce(Vector2.left * (this.speed )); 
    }

    private void MoveRight()
    {
        rb.AddForce(Vector2.right * (this.speed )); 
    }

    public void AsteroidCrash()
    {
        // Remove health
        Debug.Log("Crashed");
    }
}