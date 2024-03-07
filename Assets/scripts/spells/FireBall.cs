using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Spell
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float range;
    [SerializeField]
    private float timer;

    void Start()
    {
        // Initialise spell stats
        SetupSpell(damage, speed, Effect0, Effect1, Effect2, RangeTypes.area, range, timer);

        // Get target
        GetClosestTarget(gameManager, gameManager.player);

        // Prevent duplicates, but causes a bug with new shop code
        //PreventFireBallCasting();
    }

    void Update()
    {
        GetClosestTarget(gameManager, gameManager.player);

        Move(gameManager.player, target, speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageEnemy(collision.gameObject, gameManager, damage);
    }

    private void PreventFireBallCasting()
    {
        // Prevent FireBall from being cast if one already exists in the scene
        FireBall[] otherFireBalls = FindObjectsOfType<FireBall>();
        if (otherFireBalls.Length > 1)
        {
            DestroySpell();
        }
    }
}
