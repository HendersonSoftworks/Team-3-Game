using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class wizardBoss : MonoBehaviour
{
    private GameObject Player;

    public GameObject Summon;
    public GameObject enemyProjectile;
    public GameObject bossProjectile;

    public Sprite sprite1;
    public Sprite sprite2;
    private SpriteRenderer spriteRenderer;

    public float movementSpeed;
    public int health;

    private bool Shooting;
    private bool Fleeing;
    private bool Chasing;
    private bool bossRing;

    private float coolDownTimer;

    public float chargePause;
    public float chargeTime;
    public float chargeCooldown;
    private float ChaseX;
    private float ChaseY;

    private float testTimer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Player = GameObject.FindWithTag("Player");

        health = 1000;
        movementSpeed = 0.8f;

        bossRing = false;

        this.GetComponent<enemyHealth>().health = 1000;
    }

    // Update is called once per frame
    void Update()
    {
    testTimer += Time.deltaTime;


    float xDiff = (Player.transform.position.x - transform.position.x);
    float yDiff = (Player.transform.position.y - transform.position.y);

    float distance = (float)Math.Sqrt(
        (xDiff * xDiff)
        +
        (yDiff * yDiff)
        );
        //------------------------------testing to be removed
        if (testTimer>5)
        {
            this.GetComponent<enemyHealth>().health = 300;
        }
        //---------------------------------------------------
     
    if (this.GetComponent<enemyHealth>().health > 750) 
    {
        PhaseOne(xDiff, yDiff, distance);
    }
    else if(this.GetComponent<enemyHealth>().health > 400)
    {
        PhaseTwo(xDiff, yDiff, distance);
    }
    else if(this.GetComponent<enemyHealth>().health > 0)
    {
        PhaseThree(xDiff, yDiff, distance);
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

    void PhaseTwo(float xDiff, float yDiff, float distance) {

        if (!bossRing)
        {
            for (int i = 0; i < 360; i += 30)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, i);
                Instantiate(bossProjectile, transform.position, rotation);
            }
            bossRing = true;
        }

        float movement;
        movement = Math.Abs(xDiff) + Math.Abs(yDiff);
        movement = movementSpeed / movement;

        xDiff = xDiff* movement;
        yDiff = yDiff* movement;

        //ranged attack phase
        coolDownTimer += Time.deltaTime;

        if (Shooting)
        {

            if (coolDownTimer > 1f)
            {
                Instantiate(enemyProjectile, transform.position, transform.rotation);
                Shooting = false;
            }
            else if (coolDownTimer > 0.5f)
            {
                spriteRenderer.sprite = sprite2;
                //add animation
                //charging up spell , creating projectile animation.
            }

        }
        else
        {
            //set to fire periodically
            if (coolDownTimer > 2.5f)
            {
                Shooting = true;
                coolDownTimer = 0;
                spriteRenderer.sprite = sprite1;
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

    void PhaseThree(float xDiff, float yDiff, float distance) {
        float movement;
        movement = Math.Abs(xDiff) + Math.Abs(yDiff);
        movement = movementSpeed / movement;

        xDiff = xDiff * movement;
        yDiff = yDiff * movement;

        //ranged attack phase
        coolDownTimer += Time.deltaTime;

        if (Shooting)
        {

            if (coolDownTimer > 0.8f)
            {
                Instantiate(enemyProjectile, transform.position, transform.rotation);
                Shooting = false;
            }
            else if (coolDownTimer > 0.2f)
            {
                //add animation
                //charging up spell , creating projectile animation.
            }

        }
        else
        {
            //set to fire periodically
            if (coolDownTimer > 2f)
            {
                //summon a creep after firing 
                Instantiate(Summon, new Vector3(transform.position.x + UnityEngine.Random.Range(-1.0f, 1f), transform.position.y + UnityEngine.Random.Range(-1.0f, 1f),0), transform.rotation);
                Shooting = true;
                coolDownTimer = 0;
            }

            if (Fleeing)
            {
                transform.Translate(new Vector2(xDiff * -1, yDiff * -1) * Time.deltaTime);
                if (distance > 5)
                {
                    Fleeing = false;
                }
            }
            else
            {
                transform.Translate(new Vector2(xDiff, yDiff) * Time.deltaTime);
                if (distance < 4)
                {
                    Fleeing = true;
                }
            }
        }

    }
    
}