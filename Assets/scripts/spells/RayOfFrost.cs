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

    private GameObject player;
    private bool hitEnemy = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Effects = new List<EffectTypes>
        {
            EffectTypes.autotarget
        };

        RangeType = RangeTypes.beam;

        Recharge = 3.0f;
        Damage = damage;
        RangeType = RangeTypes.single;
        target = null;

        GetClosestTarget(gameManager, gameManager.player);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Timer
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        Beam(player, target, range);
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            //Gizmos.DrawLine(player.transform.position, target.transform.position);
        }
    }

    public override void DamageEnemy(GameObject target, GameManager gameManager, float damage)
    {
        if (hitEnemy == true)
        {
            return;
        }
        
        base.DamageEnemy(target, gameManager, damage);

        hitEnemy = true;
    }
}
