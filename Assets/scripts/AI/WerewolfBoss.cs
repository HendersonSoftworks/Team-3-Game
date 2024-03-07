using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class WerewolfBoss : MonoBehaviour
{

    private GameObject Player;

    public GameObject Summon;

    public float chargePause;
    public float chargeTime;
    public float chargeCooldown;

    public float movementSpeed;
    public int health;

    private bool Chasing;
    private float ChaseX;
    private float ChaseY;

    private float coolDownTimer;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        Chasing = false;
        ChaseX = 0f;
        ChaseY = 0f;

        coolDownTimer = 0f;

        movementSpeed = 0.9F;
        health = 250;
        this.GetComponent<enemyHealth>().health = 250;
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

        coolDownTimer += Time.deltaTime;


        if (Chasing)
        {
            
            if (coolDownTimer > 3f)
            {
                coolDownTimer = 0f;
                Chasing = false;
                Instantiate(Summon, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.6f, 0.6f), transform.position.y + UnityEngine.Random.Range(-1.0f, 1f), 0), transform.rotation);
                Instantiate(Summon, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.6f, 0.6f), transform.position.y + UnityEngine.Random.Range(-1.0f, 1f), 0), transform.rotation);
            }
            else if (coolDownTimer > 1f)
            {
                transform.Translate(new Vector2(ChaseX, ChaseY) * Time.deltaTime);
            }

        }
        else if (Math.Abs(distance) < 3 && !Chasing && coolDownTimer > 3f)
        {
            Instantiate(Summon, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.6f, 0.6f), transform.position.y + UnityEngine.Random.Range(-1.0f, 1f), 0), transform.rotation);
            coolDownTimer = 0f;
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