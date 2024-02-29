using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayOfFrost : Spell
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float timer;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float range;

    private bool hitEnemy = false;

    void Start()
    {
        // Initialise spell stats
        SetupSpell(damage, speed, Effect0, Effect1, Effect2, RangeTypes.beam, range, timer);

        // Get target
        GetClosestTarget(gameManager, gameManager.player);
    }

    void Update()
    {
        // Timer
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        Beam(gameManager.player, target, Range);
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
