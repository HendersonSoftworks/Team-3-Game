using System;
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
    public float Speed { get; set; }
    public float Timer { get; set; }
    public float Damage { get; set; }
    public float Range { get; set; }
    public enum RangeTypes { single, area, chain, beam};
    
    /// <summary>
    /// Whether the spell targets a single enemy, an area, etc.
    /// </summary>
    public RangeTypes RangeType { get; set; }
    public enum EffectTypes { none, autotarget,burn, destroy, slow, paralyse, 
                              blind, push, control, fear, heal, poison, 
                              create, teleport, weaken,};
    /// <summary>
    /// Special effects that are activated upon casting / being hit by the spell. A spell can have multiple effects.
    /// </summary>
    public EffectTypes Effect0;
    public EffectTypes Effect1;
    public EffectTypes Effect2;
    public List<EffectTypes> Effects;

    /// <summary>
    /// Spell target is set during runtime
    /// </summary>
    public GameObject target;
    public GameObject impactAnimation;
    
    protected GameManager gameManager;
    protected SpriteRenderer spriteRenderer;

    public bool canRecast;
    public AudioClip castClip;
    public AudioClip impactClip;

    // Private properties
    private float targetDist;

    public virtual void SetupSpell(float damage, float speed, 
        EffectTypes effect0, EffectTypes effect1, EffectTypes effect2,
        RangeTypes rangeType, float range,
        float timer
        )
    {
        // Set GM & Renderer
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        // Set Damage
        Damage = damage;

        // Set Speed
        Speed = speed;

        // Set Effects
        Effects = new List<EffectTypes>
        {
            effect0, effect1, effect2
        };

        // Set Range Type
        RangeType = rangeType;

        // Set Range
        Range = range;

        // Set Timer
        Timer = timer;

        // Clear target
        target = null;
    }

    public virtual bool ReturnCastFlag()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            return true;
        }

        return false;
    }

    public virtual void GetClosestTarget(GameManager gameManager, GameObject player)
    {
        if (gameManager.enemies.Length == 0)
        {
            DestroySpell();
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

        //targetDist = Vector2.Distance(player.transform.position, target.transform.position);
        targetDist = Vector2.Distance(transform.position, target.transform.position);
    }

    public virtual void DamageEnemy(GameObject target, GameManager gameManager, float damage)
    {
        if (target.tag != "Enemy")
            return;
        
        Instantiate(impactAnimation, target.transform.position, Quaternion.identity);

        if (RangeType == RangeTypes.area)
        {
            // Loop through all other enemies within the explosion and damage them too
            foreach (var currentEnemy in gameManager.enemies)
            {
                var dist = Vector2.Distance(currentEnemy.transform.position, transform.position);
                if (dist < 4 && currentEnemy != target)
                {
                    currentEnemy.GetComponent<Enemy>().health -= (int)damage;
                }
            }
        }

        // Reduce enemy health
        Enemy enemy = target.GetComponent<Enemy>();
        enemy.health -= (int)damage;

        if (RangeType == RangeTypes.single || RangeType == RangeTypes.area)
        {
            DestroySpell();
        }

        // Audio
        if (impactClip != null)
        {
            gameManager.GetComponent<AudioSource>().PlayOneShot(impactClip);
        }
    }

    public virtual void Move(GameObject player, GameObject target, float speed)
    {
        if (target == null || targetDist > Range)
        {
            DestroySpell();
        }

        spriteRenderer.enabled = true;

        if (target != null)
        {
            LookAtTarget(target, player);
        }

        if (Effects.Contains(EffectTypes.autotarget) && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            return;
        }
    }

    public virtual void Beam(GameObject player, GameObject target, float range)
    {
        if (target == null || targetDist > Range)
        {
            DestroySpell();
        }
        
        if (targetDist <= range && target != null)
        {
            spriteRenderer.enabled = true;
            LookAtTarget(player, target);

            transform.position = LerpByDistance(player.transform.position, target.transform.position, targetDist / 2);
            ScaleSpellY(gameObject, targetDist);

            DamageEnemy(target, gameManager, Damage);
        }
    }

    public virtual void LookAtTarget(GameObject caster, GameObject target)
    {
        if (target != null && gameObject != null && caster != null)
        {
            Vector2 dir = (caster.transform.position - target.transform.position);
            transform.up = dir;
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