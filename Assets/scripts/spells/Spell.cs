using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for spells.
/// </summary>
public class Spell : MonoBehaviour
{
    /// <summary>
    /// Frequency of casting spell.
    /// </summary>
    public float Recharge { get; set; }
    public float Damage { get; set; }
    public enum RangeTypes { single, area, chain, beam};
    
    /// <summary>
    /// Whether the spell targets a single enemy, an area, etc.
    /// </summary>
    public RangeTypes RangeType { get; set; }
    public enum EffectTypes { autotarget, destroy, slow, paralyse, weaken, blind, push, control, fear, heal, poison, create, teleport};
      
    /// <summary>
    /// Special effects that are activated upon casting / being hit by the spell. A spell can have multiple effects.
    /// </summary>
    public List<EffectTypes> Effects;
    
    /// <summary>
    /// Spell target is set during runtime
    /// </summary>
    public GameObject target;
    public GameObject impactAnimation;

    public virtual void GetClosestTarget(GameManager gameManager)
    {
        if (gameManager.enemies.Length == 0)
        {
            SelfDestruct();
            return;
        }

        if (target == null)
        {
            target = gameManager.enemies[0].gameObject;

            foreach (var enemy in gameManager.enemies)
            {
                if (enemy == null)
                {
                    return;
                }

                float new_dist = Vector2.Distance(transform.position, enemy.transform.position);
                float target_dist = Vector2.Distance(transform.position, target.transform.position);

                if (new_dist < target_dist)
                    target = enemy.gameObject;
            }
        }
    }

    public virtual void DamageEnemy(GameObject target, GameManager gameManager, float damage)
    {
        if (target.tag != "Enemy")
        {
            return;
        }

        Instantiate(impactAnimation, transform.position, Quaternion.identity);

        Destroy(gameObject);

        // Reduce health
        target.GetComponent<TomEnemy>().health -= damage;

        if (Effects.Contains(EffectTypes.destroy) || target.GetComponent<TomEnemy>().health < 0.1)
        {
            Destroy(target);
        }
    }

    public virtual void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public virtual void Move(GameObject target, float speed)
    {
        if (target == null)
        {
            return;
        }

        if (Effects.Contains(EffectTypes.autotarget))
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            return;
        }
    }

    public virtual void Beam(GameObject player, GameObject target, float range)
    {
        if (target == null)
        {
            return;
        }

        // Set size of beam
        Vector2 newScale = new Vector3(transform.localScale.x, range);
        transform.localScale = newScale ;

        // Set Y rotation so beam is aimed at target
        transform.up = (target.transform.position - transform.position);

        // Correct beam position to fit with scale
        float beamDist = Vector2.Distance(player.transform.position, target.transform.position);
        if (beamDist <= transform.localScale.y / 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, (transform.localScale.y / 2) + beamDist * 2);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, (transform.localScale.y / 2));
        }
        

        return;
    }

    public virtual void DestroySpell()
    {
        Destroy(gameObject);
    }
}