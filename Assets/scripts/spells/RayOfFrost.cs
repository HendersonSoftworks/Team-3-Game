using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayOfFrost : Spell
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float timer;

    private GameObject player;
    private bool hitEnemy = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Damage = damage;

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

        Beam(player, target, Range);
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
