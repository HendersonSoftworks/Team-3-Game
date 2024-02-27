using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Spell
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;

    private GameManager gameManager;

    [SerializeField]
    private List<GameObject> collisions;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        collisions = new List<GameObject>();

        Effects = new List<EffectTypes>
        {
            EffectTypes.autotarget
        };

        Recharge = 3.0f;
        Damage = Damage;
        RangeType = RangeTypes.area;
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

    public override void DamageEnemy(GameObject target, GameManager gameManager, float damage)
    {
        // Create big explosion sprite
        base.DamageEnemy(target, gameManager, damage);
    }
}
