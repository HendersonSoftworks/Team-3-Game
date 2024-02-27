using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayOfFrost : Spell
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float range;
    [SerializeField]
    private float timer;

    private GameManager gameManager;
    private GameObject player;

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

        GetClosestTarget(gameManager);

        player = GameObject.FindGameObjectWithTag("Player");
        Beam(player, target, range);
    }

    void Update()
    {
        // Timer
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageEnemy(collision.gameObject, gameManager, damage);
    }
}
