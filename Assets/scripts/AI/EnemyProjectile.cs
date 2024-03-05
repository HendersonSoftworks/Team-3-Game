using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class EnemyProjectile : MonoBehaviour
{
    private GameObject Player;
    public GameObject Projectile;

    private float xDiff;
    private float yDiff;

    float seconds;

    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player"); 

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
}