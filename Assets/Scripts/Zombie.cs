using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;

public class Zombie : MonoBehaviour
{
    private int health;
    private float moveSpeed;
    private Transform player;
    private Vector2 movement;
    private Rigidbody2D rb2d;
    public ParticleSystem BloodSplashPrefab;
    public int minimumGold, maximumGold;
    Vector3 direction;
    public AudioClip hitSound;
    public AudioClip deadSound;

    private bool IsDead = false;
    
    [HideInInspector]
    public bool IsBoss;
    public bool IsTank;
    public bool IsSprinter;

    private void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        int roundNumber = FindObjectOfType<GameMaster>().roundNumber;

        moveSpeed = roundNumber * 0.1f + 1.8f;
        if(IsBoss == true)
        {
            health = roundNumber * 7;
        }
        else if(IsTank == true)
        {
            health = roundNumber + 2;
        }
        else if(IsSprinter == true)
        {
            moveSpeed += 0.3f;
            health = roundNumber / 2;
        }
        else
        {
            health = roundNumber;
        }
        minimumGold += roundNumber * 1;
        maximumGold += roundNumber * 2;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        if(health <= 0 && IsDead == false)
        {
            IsDead = true;
            Instantiate(BloodSplashPrefab, transform.position, transform.rotation);
            FindObjectOfType<Player>().AddGold(Random.Range(minimumGold, maximumGold));
            FindObjectOfType<GameMaster>().ZombieDeath();
            if(IsBoss == true)
            {
                FindObjectOfType<ZombieSpawner>().BossSpawned = false;               
            }
            AudioSource.PlayClipAtPoint(deadSound, transform.position);
            ++FindObjectOfType<GameMaster>().zombiesKilledOverall;
            Destroy(gameObject);           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(direction.x < 0)
        {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else
        {
            transform.rotation = new Quaternion(0f, 0, 0f, 0f);
        }
        if (FindObjectOfType<GameMaster>().IsGameOver == true)
        {
            Instantiate(BloodSplashPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
        }
        Physics2D.IgnoreLayerCollision(8, 9);
    }

    private void FixedUpdate()
    {
        Move(movement);
    }

    void Move(Vector2 direction)
    {
        rb2d.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.fixedDeltaTime));
    }
}
