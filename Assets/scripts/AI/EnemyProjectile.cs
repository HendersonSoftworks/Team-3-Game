using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class EnemyProjectile : MonoBehaviour
{
    private GameObject Player;
    public GameManager gameManager;
    public GameObject Projectile;

    private float xDiff;
    private float yDiff;

    float seconds;

    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Player = gameManager.player;

        seconds = 25f;
        movementSpeed = 0.8f;

        float movement;
        xDiff = (Player.transform.position.x - transform.position.x);
        yDiff = (Player.transform.position.y - transform.position.y);

        movement = Math.Abs(xDiff) + Math.Abs(yDiff);
        movement = movementSpeed / movement;

        xDiff = xDiff * movement;
        yDiff = yDiff * movement;

        Destroy(gameObject, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(xDiff, yDiff) * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.hitPoints -= 10;
            Destroy(gameObject);
        }
    }
}