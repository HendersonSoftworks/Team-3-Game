using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : Spell
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Effects = new List<EffectTypes>
        {
            EffectTypes.autotarget
        };

        Recharge = 3.0f;
        Damage = damage;
        RangeType = RangeTypes.single;
        target = null;

        GetClosestTarget(gameManager, gameManager.player);
    }

    void Update()
    {
        GetClosestTarget(gameManager, gameManager.player);

        Move(target, speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageEnemy(collision.gameObject, gameManager, damage);
    }
}
