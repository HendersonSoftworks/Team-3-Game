using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject Player;
    public GameObject Projectile;

    private float xDiff;
    private float yDiff;

    float seconds;

    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        seconds = 15f;
        movementSpeed = 0.8f;

        float movement;
        xDiff = (Player.transform.position.x - Projectile.transform.position.x);
        yDiff = (Player.transform.position.y - Projectile.transform.position.y);

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