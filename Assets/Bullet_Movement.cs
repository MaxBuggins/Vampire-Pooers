using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Movement : MonoBehaviour
{
    Vector2 vectorToPlayer;
    Vector2 playerPos;
    Vector2 enemyPos;
    public GameObject enemyShootAudio;

    public float moveSpeed = 5f;
    public float destroyDelay = 25; //in seconds

    public Rigidbody2D rb;

    void Start()
    {
        Instantiate(enemyShootAudio);
        enemyPos.x = rb.transform.position.x;
        enemyPos.y = rb.transform.position.y;
        playerPos.x = GameObject.Find("Player").transform.position.x;
        playerPos.y = GameObject.Find("Player").transform.position.y;
        vectorToPlayer = (playerPos - enemyPos).normalized;
        rb.velocity = new Vector2(vectorToPlayer.x * moveSpeed, vectorToPlayer.y * moveSpeed);
        
        Invoke(nameof(DestorySelf), destroyDelay); //will invoke (run the function) in so many seconds
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
    
    void DestorySelf() //deeath
    {
        Destroy(gameObject);
    }

}
