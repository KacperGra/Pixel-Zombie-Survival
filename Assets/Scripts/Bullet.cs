using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb2d;
    public int bulletDamage = 1;
    public ParticleSystem bulletSplash;
    private Vector2 screenBounds;
    public Player player;

    private void Start()
    {
        if(player.weaponSelected == 1)
        {
            bulletDamage = 2;
        }
        else if (player.weaponSelected == 2)
        {
            bulletDamage = 3;
        }
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = transform.right * speed;
        if(transform.position.x > screenBounds.x + 1f || transform.position.x < -screenBounds.x - 1f) // Destroy bullet if it reach screen bounds
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Zombie zombie = collision.GetComponent<Zombie>();
        if(zombie != null)
        {
            zombie.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Instantiate(bulletSplash, transform.position, transform.rotation);
    }
}
