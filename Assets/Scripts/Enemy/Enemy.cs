using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public int contactDamage;
    public GameObject player;
    PlayerHealth playerHealth;
    
    // Status effect variables
    private float _statusEffectStart;
    private ProjectileConjurer _conjurer;

    // To avoid weird errors with modifying the dictionary by adding and removing. -1 stands for not active effect.
    private Dictionary<ProjectileConjurer.StatusEffects, float> _effectStartTimes = new();

    private Dictionary<ProjectileConjurer.StatusEffects, int> _conjurerEffects = new();

    // Start is called before the first frame update
    void Start()
    {
        if(!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        
        // Saves the conjurer so we only have to get it once
        _conjurer = FindObjectOfType<ProjectileConjurer>();

        // Gets the conjurers Status Effects
        _conjurerEffects = _conjurer.GetStatusEffects();
        
        // Populates Effect Start Times Dict so that we don't have to do it manually when adding new status effects
        foreach (ProjectileConjurer.StatusEffects effects in Enum.GetValues(typeof(ProjectileConjurer.StatusEffects)))
        {
            _effectStartTimes[effects] = -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks to see if the current status effects have expired
        CheckStatusEffectExpire();
        
        if (health <= 0)
            die();
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
            StatusEffectManager();
            health -= dmg;
            return false;
        }
    }

    public void die()
    { 
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        FindObjectOfType<ProgressManager>().checkCompletion();
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
            }
        }
    }

    private void CheckStatusEffectExpire()
    {
        foreach (KeyValuePair<ProjectileConjurer.StatusEffects, float> kvp in _effectStartTimes)
        {
            if (Time.time - kvp.Value >= _conjurerEffects[kvp.Key])
            {
                _effectStartTimes[kvp.Key] = -1f;
                Debug.Log($"Removing active status effect: {kvp.Key}");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerHealth.PlayerTakeDamage(contactDamage);
            Destroy(gameObject);
        }
    }

}
