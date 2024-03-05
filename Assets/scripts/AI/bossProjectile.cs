using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class bossProjectile : MonoBehaviour
{
    private GameObject Wizard;

    private float xDiff;
    private float yDiff;
    private float rotation;

    private float Speed;

    private bool direction;
    // Start is called before the first frame update
    void Start()
    {
        Wizard = GameObject.Find("Wizard");
        Speed = 1f;

        xDiff = Wizard.transform.position.x;
        yDiff = Wizard.transform.position.y;

        rotation = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        //following the movement of the wizard
        //set rotation to 0 before changing ti back to the orginal value 
        transform.rotation = Quaternion.Euler(0, 0, 0);
        xDiff =  Wizard.transform.position.x - xDiff;
        yDiff = Wizard.transform.position.y - yDiff;
        transform.Translate(new Vector2(xDiff, yDiff));
        transform.rotation = Quaternion.Euler(0, 0, rotation);

        xDiff = (Wizard.transform.position.x - transform.position.x);
        yDiff = (Wizard.transform.position.y - transform.position.y);

        float distance = (float)Math.Sqrt(
            (xDiff * xDiff)
            +
            (yDiff * yDiff)
            );
        if(direction)
        {
            if(distance < 1)
            {
                direction = false;
            }
            transform.Translate(Vector2.down * Time.deltaTime * Speed);
        }
        else
        {
            if (distance > 2.5)
            {
                direction = true;
            }
            transform.Translate(Vector2.up * Time.deltaTime * Speed);
        }

        xDiff = Wizard.transform.position.x;
        yDiff = Wizard.transform.position.y;
    }
}
