using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public static UnityEvent deathEvent;
    static bool deathListenerAdded = false;
    public float maxHealth;
    public float health;
    public int contactDamage;
    public GameObject player;
    PlayerHealth playerHealth;
    ProgressManager progressManager;
    
    // Status effect variables
    private float _statusEffectStart;
    private ProjectileConjurer _conjurer;
    
    // To avoid weird errors with modifying the dictionary by adding and removing. -1 stands for not active effect.
    private Dictionary<ProjectileConjurer.StatusEffects, float> _effectStartTimes = new ()
    {
        { ProjectileConjurer.StatusEffects.Fire, -1f },
        { ProjectileConjurer.StatusEffects.Slow, -1f }
    };
    
    private Dictionary<ProjectileConjurer.StatusEffects, int> _conjurerEffects = new();

    // Start is called before the first frame update
    void Start()
    {
        // Saves the conjurer so we only have to get it once
        _conjurer = FindObjectOfType<ProjectileConjurer>();
        
        // Gets the conjurers Status Effects
        _conjurerEffects = _conjurer.GetStatusEffects();

        PrintEffectStartTimes();
        
        if(!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        progressManager = player.GetComponent<ProgressManager>();

        if (deathEvent == null)
            deathEvent = new UnityEvent();
        if(!deathListenerAdded)
        {
            deathEvent.AddListener(progressManager.incrementDeathCounter);
            deathListenerAdded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks to see if the current status effects have expired

        if (health <= 0)
        {   
            die();
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
        //setup to allow a return of boolean before destruction, in case damager has to know if the attack killed the enemy. If multiplayer allows for kill counts per player
        if (health < dmg)
        {
            health = 0;
            return true;
        }
        else
        {
            // Applies status effects if it doesn't have any

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
        progressManager.checkCompletion();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerHealth.PlayerTakeDamage(contactDamage);
            die();
        }
    }
    
    private void StatusEffectManager()
    {
        foreach (KeyValuePair<ProjectileConjurer.StatusEffects, int> kvp in _conjurerEffects)
        {
            // If we currently dont have this effect
            if (_effectStartTimes.ContainsKey(kvp.Key) && _effectStartTimes[kvp.Key] == -1)
            {
                Debug.Log($"Adding active status effect: {kvp.Key}");
                // Keeps track of when the status effect was applied
                _effectStartTimes[kvp.Key] = Time.time;
                PrintEffectStartTimes();
            }
        }
    }
    private void CheckStatusEffectExpire()
    {
        Dictionary<ProjectileConjurer.StatusEffects, float> effectStartCopy = _effectStartTimes;
        foreach (KeyValuePair<ProjectileConjurer.StatusEffects, float> kvp in effectStartCopy)
        {
            if (kvp.Value == -1)
                continue;
            
            if (Time.time - kvp.Value >= _conjurerEffects[kvp.Key])
            {
                _effectStartTimes[kvp.Key] = -1f;
                Debug.Log($"Removing active status effect: {kvp.Key}");
                PrintEffectStartTimes();
            }
        }
    }

    private void PrintEffectStartTimes()
    {
        string message = "Current Effect Start Times Dict:\n";
        foreach (KeyValuePair<ProjectileConjurer.StatusEffects, float> kvp in _effectStartTimes)
        {
            message += $"Key: {kvp.Key} Value: {kvp.Value}\n";
        }
        
        Debug.Log(message);
    }

}
