using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class Enemy : MonoBehaviour
{

    public GameObject Player;
    public GameObject creep;

    public float chargePause;
    public float chargeTime;
    public float chargeCooldown;

    public float movementSpeed;
    public int health;

    private bool Chasing;
    private float ChaseX;
    private float ChaseY;

    private float coolDownTimer;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        Chasing = false;
        ChaseX = 0f;
        ChaseY = 0f;

        coolDownTimer = 0f;

        movementSpeed = 0.8F;
        health = 100;
    }

    // Update is called once per frame
    void Update()
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
}
