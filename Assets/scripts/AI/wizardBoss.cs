using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class wizardBoss : MonoBehaviour
{
    public GameObject Player;
    public GameObject creep;

    public float movementSpeed;
    public int health;

    private bool Shooting;
    private bool Fleeing;

    private float coolDownTimer;

    public float chargePause;
    public float chargeTime;
    public float chargeCooldown;
    private bool Chasing;
    private float ChaseX;
    private float ChaseY;
    // Start is called before the first frame update
    void Start()
    {
        health = 1000;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (health > 50) 
        {
            float movement;
            float xDiff = (Player.transform.position.x - creep.transform.position.x);
            float yDiff = (Player.transform.position.y - creep.transform.position.y);

            float distance = (float)Math.Sqrt(
                (xDiff * xDiff)
                +
                (yDiff * yDiff)
                );


            if (Chasing)
            {
                coolDownTimer += Time.deltaTime;
                if (coolDownTimer > 3f)
                {
                    coolDownTimer = 0f;
                    Chasing = false;
                    //fire off another projectile at the end of the charge
                    //Instantiate(enemyProjectile, transform.position, transform.rotation);
                }
                else if (coolDownTimer > 1f)
                {
                    transform.Translate(new Vector2(ChaseX, ChaseY) * Time.deltaTime);
                }

            }
            else if (Math.Abs(distance) < 3 && !Chasing)
            {
                //Setting the creep to charge
                Chasing = true;
                movement = Math.Abs(xDiff) + Math.Abs(yDiff);
                movement = 2f / movement;
                //set the direction of the charge
                ChaseX = xDiff * movement;
                ChaseY = yDiff * movement;
                //fire off a projectile at the start of charge
                //Instantiate(enemyProjectile, transform.position, transform.rotation);
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
            //----------------------------------------------------------------------------------------------------PHASE 2 Ranged----------------------------------------------------------------------------------------------------//
        }
        else
        {
            if(health <= 0)
            {
                Destroy(gameObject);
            }
            //ranged attack phase
            if (Shooting)
            {

                if (coolDownTimer > 2.5f)
                {
                    //Instantiate(enemyProjectile, transform.position, transform.rotation);
                    Shooting = false;
                }
                else if (coolDownTimer > 0.5f)
                {
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
                        Fleeing = false;
                    }
                }
            }
        }
        */
        
    }
}
