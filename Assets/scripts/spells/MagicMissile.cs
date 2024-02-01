using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : Spell
{
    private GameManager gameManager;

    [SerializeField]
    private float speed;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Recharge = 3.0f;
        Damage = 20.0f;
        RangeType = RangeTypes.single;
        //Effects.Add(EffectTypes.autotarget);
        Target = null;
        GetClosestTarget(gameManager);

    }

    void Update()
    {

        // Move towards enemy
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(Target.name + " hit!");
        Destroy(Target);
        GetClosestTarget(gameManager);
        gameManager.UpdateEnemyList();
        Destroy(gameObject);
    }
}
