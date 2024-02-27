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

    public GameManager gameManager;

    float targetDist;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public virtual void GetClosestTarget(GameManager gameManager, GameObject player)
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

        targetDist = Vector2.Distance(player.transform.position, target.transform.position);
    }

    public virtual void DamageEnemy(GameObject target, GameManager gameManager, float damage)
    {
        if (target.tag != "Enemy")
            return;
        
        Instantiate(impactAnimation, target.transform.position, Quaternion.identity);

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
        if (target == null || targetDist > range )
        {
            DestroySpell();
        }

        print("beam!");

        
        if (targetDist <= range && target != null)
        {
            print("Beamed:" + target.gameObject.name);
            
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            Vector2 dir = (player.transform.position - target.transform.position);
            transform.up = dir;
            transform.position = LerpByDistance(player.transform.position, target.transform.position, targetDist / 2);
            ScaleSpellY(gameObject, targetDist);

            DamageEnemy(target, gameManager, Damage);
        }
    }

    public virtual void DestroySpell()
    {
        Destroy(gameObject);
    }

    public virtual void DisableBoxCollider()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }

    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }

    public void ScaleSpellY(GameObject spellObj, float scale)
    {
        // Get the current local scale
        Vector3 spriteScale = spellObj.transform.localScale;

        // Set the y value of the local scale
        spriteScale.y = scale;

        // Apply the new scale to the sprite
        spellObj.transform.localScale = spriteScale;
    }
}