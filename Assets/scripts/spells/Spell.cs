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
    public enum RangeTypes { single, area, chain};
    /// <summary>
    /// Whether the spell targets a single enemy, an area, etc.
    /// </summary>
    public RangeTypes RangeType { get; set; }
    public enum EffectTypes { autotarget, slow, paralyse, weaken, blind, push, control, fear, heal, poison, create}; 
    /// <summary>
    /// Special effects that are activated upon casting / being hit by the spell. A spell can have multiple effects.
    /// </summary>
    public List<EffectTypes> Effects { get; set; }
    public GameObject Target { get; set; }

    public virtual void GetClosestTarget(GameManager gameManager)
    {
        // Break this out into GetClosestTarget()
        if (Target == null)
        {
            Target = gameManager.enemies[0].gameObject;

            foreach (var enemy in gameManager.enemies)
            {
                float new_dist = Vector2.Distance(transform.position, enemy.transform.position);
                float target_dist = Vector2.Distance(transform.position, Target.transform.position);

                if (new_dist < target_dist)
                    Target = enemy.gameObject;
            }
        }
    }

    public virtual void DamageEnemy(TomEnemy tomEnemy)
    {
        print("Enemy damaged");
    }
}