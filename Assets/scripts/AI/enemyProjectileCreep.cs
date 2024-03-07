using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class enemyProjectileCreep : MonoBehaviour
{

    private GameObject Player;

    public GameObject enemyProjectile;

    public float movementSpeed;
    public int health;

    private bool Shooting;
    private bool Fleeing;

    private float coolDownTimer;    

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        Shooting = false;
        Fleeing = false;

        coolDownTimer = 0f;

        //movement speed is lower for ranged attack mobs
        movementSpeed = 0.6F;
        health = 60;
        this.GetComponent<enemyHealth>().health = 60;

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
        coolDownTimer += Time.deltaTime;


        if (Shooting)
        {

            if (coolDownTimer > 2.5f)
            {
                Instantiate(enemyProjectile, transform.position, transform.rotation);
                Shooting = false;
            }
            else if (coolDownTimer > 0.5f)
            {
                //add animation
                //charging up spell , creating projectile animation.
            }

        }
        else
        {
            //set to fire periodically
            if (coolDownTimer > 12f)
            {
                Shooting = true;
                coolDownTimer = 0;
            }

            if (Fleeing)
            {
                transform.Translate(new Vector2(xDiff * -1, yDiff * -1) * Time.deltaTime);
                if (distance > 4)
                {
                    Fleeing = false;
                }
            }
            else
            {
                transform.Translate(new Vector2(xDiff, yDiff) * Time.deltaTime);
                if (distance < 3)
                {
                    Fleeing = true;
                }
            }
        }

    }
}