using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public  static UnityEvent deathEvent;
    public static bool deathListenerAdded = false;
    [SerializeField] public float maxHealth;
    protected float health;
    private float baseSpeed;
    [SerializeField] protected int contactDamage;
    private GameObject player;
    private PlayerHealth playerHealth;
    private ProgressManager progressManager;

    private SpriteRenderer sprite;
    private float hurtTime;
    private static float flashDuration = .3f;
    private static float redness = .5f;
    
    // Status effect variables
    private ProjectileConjurer _conjurer;
    private Parent_AI _parentAI;
    private Dictionary<ProjectileConjurer.StatusEffects, float> _conjurerEffects = new();
    private Dictionary<ProjectileConjurer.StatusEffects, float> durations;
    private Dictionary<ProjectileConjurer.StatusEffects, float> ticks;
    private static List<ProjectileConjurer.StatusEffects> tickable = new List<ProjectileConjurer.StatusEffects> { ProjectileConjurer.StatusEffects.Fire };
    private Dictionary<ProjectileConjurer.StatusEffects, Action<bool>> statusActions;

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        hurtTime = 0f;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        health = maxHealth;
        // Saves the conjurer so we only have to get it once
        _conjurer = FindObjectOfType<ProjectileConjurer>();
        
        // Saves the parent ai so we only have to get it once
        _parentAI = FindObjectOfType<Parent_AI>();
        baseSpeed = _parentAI.GetSpeed();

        // Gets the conjurers Status Effects
        _conjurerEffects = _conjurer.GetStatusEffects();
        durations = new Dictionary<ProjectileConjurer.StatusEffects, float>(_conjurerEffects);
        ticks = new Dictionary<ProjectileConjurer.StatusEffects, float>(_conjurerEffects);
        foreach (var e in _conjurerEffects)
        {
            durations[e.Key] = 0f;
            ticks[e.Key] = 0f;
        }
        statusActions = new Dictionary<ProjectileConjurer.StatusEffects, Action<bool>>()
        {
            { ProjectileConjurer.StatusEffects.Fire, this.FireStatus},
            { ProjectileConjurer.StatusEffects.Slow, this.SlowStatus}
        };


        if (!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        progressManager = player.GetComponent<ProgressManager>();

        if (deathEvent == null)
        {
            deathEvent = new UnityEvent();
            deathListenerAdded = false;
        }
            
        if(!deathListenerAdded)
        {
            deathEvent.AddListener(progressManager.incrementDeathCounter);
            deathEvent.AddListener(progressManager.checkCompletion);
            deathListenerAdded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {   
            die();
        }
        StatusUpdate();

        if (hurtTime > 0f)
        {
            float buildDuration = flashDuration * .05f;
            float steadyDuration = flashDuration * .3f;
            float dropDuration = flashDuration - buildDuration - steadyDuration;
            if (hurtTime > flashDuration - buildDuration)
            {
                Color c = sprite.color;
                c.g = Mathf.Lerp(1, 1-redness, (flashDuration - hurtTime) / buildDuration);
                c.b = Mathf.Lerp(1, 1 - redness, (flashDuration - hurtTime) / buildDuration);
                sprite.color = c;
            }
            else if (hurtTime <= dropDuration)
            {
                Color c = sprite.color;
                c.g = Mathf.Lerp(1, 1 - redness, hurtTime / dropDuration);
                c.b = Mathf.Lerp(1, 1 - redness, hurtTime / dropDuration);
                sprite.color = c;
            }
            hurtTime -= Time.deltaTime;
        }
    }
    
    void HealEnemy(float heal)
    {
        health += heal;
        if (health > maxHealth)
            health = maxHealth;
    }

    public bool DamageEnemy(float dmg)
    {
        hurtTime = flashDuration;
        //setup to allow a return of boolean before destruction, in case damager has to know if the attack killed the enemy. If multiplayer allows for kill counts per player
        if (health < dmg)
        {
            health = 0;
            return true;
        }
        else
        {
            health -= dmg;
            return false;
        }
    }

    public void die()
    {
        deathEvent.Invoke();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        //progressManager.checkCompletion();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerHealth.PlayerTakeDamage(contactDamage);
        }
    }
    
    public void StatusEffectManager()
    {
        foreach (KeyValuePair<ProjectileConjurer.StatusEffects, float> kvp in _conjurerEffects)
        {
            durations[kvp.Key] = Math.Max(kvp.Value, durations[kvp.Key]);
            if (!tickable.Contains(kvp.Key))
            {
                statusActions[kvp.Key].Invoke(true);
            }
        }
    }

    private void StatusUpdate()
    {
        foreach (KeyValuePair<ProjectileConjurer.StatusEffects, float> kvp in _conjurerEffects)
        {
            if (durations[kvp.Key] > 0)
                processStatus(kvp.Key);
        }
    }

    private void processStatus(ProjectileConjurer.StatusEffects status)
    {
        durations[status] -= Time.deltaTime;
        if (tickable.Contains(status))
        {
            ticks[status] += Time.deltaTime;
            if (ticks[status] >= 1f)
            {
                statusActions[status].Invoke(true);
                ticks[status] = 0;
            }
        }
        if (durations[status] <= 0) 
        {
            statusActions[status].Invoke(false);
        }
    }
    
    private void FireStatus(bool activation)
    {
        Debug.Log("fire " + activation);
        if (activation)
            DamageEnemy(5);
    }
    
    private void SlowStatus(bool activation)
    {
        Debug.Log("slow " + activation);
        if (activation) 
        {
            _parentAI.SetSpeed(baseSpeed * .5f);
        }
        else
        {
            _parentAI.SetSpeed(baseSpeed);
        }
    }

}
