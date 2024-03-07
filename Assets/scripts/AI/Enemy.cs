using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{

    private GameObject Player;

    public float movementSpeed;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        movementSpeed = 0.8F;
        this.GetComponent<enemyHealth>().health = 100;
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float movement;
        float xDiff = (Player.transform.position.x - transform.position.x);
        float yDiff = (Player.transform.position.y - transform.position.y);

        float distance = (float)Math.Sqrt(
            (xDiff * xDiff)
            +
            (yDiff * yDiff)
            );

        movement = Math.Abs(xDiff) + Math.Abs(yDiff);
        movement = movementSpeed / movement;

        xDiff = xDiff * movement;
        yDiff = yDiff * movement;

        transform.Translate(new Vector2(xDiff, yDiff) * Time.deltaTime);
    }
}
