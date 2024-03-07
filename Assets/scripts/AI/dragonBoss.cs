using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class dragonBoss : MonoBehaviour
{
    private GameObject Player;

    public GameObject enemyProjectile;

    public float movementSpeed;
    public int health;

    private bool Shooting;
    private bool Chasing;

    private float coolDownTimer;

    public float chargePause;
    public float chargeTime;
    public float chargeCooldown;
    private float ChaseX;
    private float ChaseY;

    private float randomX;
    private float randomY;
    private bool changePos;
    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.FindWithTag("Player");

        health = 700;
        this.GetComponent<enemyHealth>().health = 700;
        movementSpeed = 0.8f;

        changePos = true;
    }

    // Update is called once per frame
    void Update()
    {


        float xDiff = (Player.transform.position.x - transform.position.x);
        float yDiff = (Player.transform.position.y - transform.position.y);

        float distance = (float)Math.Sqrt(
            (xDiff * xDiff)
            +
            (yDiff * yDiff)
            );



        if (health > 400)
        {
            PhaseOne(xDiff, yDiff, distance);
        }
        else if (health > 0)
        {
            PhaseTwo();
        }

    }




    void PhaseOne(float xDiff, float yDiff, float distance)
    {
        float movement;

        if (Chasing)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer > 3f)
            {
                coolDownTimer = 0f;
                Chasing = false;
                //fire off another projectile at the end of the charge
                Instantiate(enemyProjectile, transform.position, transform.rotation);
            }
            else if (coolDownTimer > 1f)
            {
                transform.Translate(new Vector2(ChaseX, ChaseY) * Time.deltaTime);
            }

        }
        else if (Math.Abs(distance) < 3 && !Chasing)
        {
            //Setting the boss to charge
            Chasing = true;
            movement = Math.Abs(xDiff) + Math.Abs(yDiff);
            movement = 1.5f / movement;
            //set the direction of the charge
            ChaseX = xDiff * movement;
            ChaseY = yDiff * movement;
            //fire off a projectile at the start of charge
            Instantiate(enemyProjectile, transform.position, transform.rotation);
        }
        else
        {
            //normal chase behaviour 
            movement = Math.Abs(xDiff) + Math.Abs(yDiff);
            movement = movementSpeed / movement;

            xDiff = xDiff * movement;
            yDiff = yDiff * movement;

            transform.Translate(new Vector2(xDiff, yDiff) * Time.deltaTime);
        }

    }

    void PhaseTwo()
    {
        
        //ranged attack phase
        coolDownTimer += Time.deltaTime;

        if (Shooting)
        {

            if (coolDownTimer > 1f)
            {
                Instantiate(enemyProjectile, transform.position, transform.rotation);
                Shooting = false;
                changePos = true;
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
            if (coolDownTimer > 2.5f)
            {
                Instantiate(enemyProjectile, transform.position, transform.rotation);
                Shooting = true;
                coolDownTimer = 0;
            }
            else
            {
                if (changePos)
                {
                    //set random point to fly to
                    randomX = UnityEngine.Random.Range(-6f, 6f);
                    randomY = UnityEngine.Random.Range(-3f, 3f);
                    changePos = false;
                }

                //flying to random point 
                float xDiff = (randomX - transform.position.x);
                float yDiff = (randomY - transform.position.y);

                float distance = (float)Math.Sqrt(
                (xDiff * xDiff)
                +
                (yDiff * yDiff)
                );

                float movement;
                movement = Math.Abs(xDiff) + Math.Abs(yDiff);
                movement = 2f / movement;

                xDiff = xDiff * movement;
                yDiff = yDiff * movement;

                transform.Translate(new Vector2(xDiff, yDiff) * Time.deltaTime);
            }
            
            
        }
    }

}