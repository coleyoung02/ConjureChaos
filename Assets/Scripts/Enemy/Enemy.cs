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
    private ProjectileConjurer _conjurer;
    private Parent_AI _parentAI;
    private Dictionary<ProjectileConjurer.StatusEffects, int> _conjurerEffects = new();

    private bool _fireCoroutineRunning = false;
    private bool _slowCoroutineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        // Saves the conjurer so we only have to get it once
        _conjurer = FindObjectOfType<ProjectileConjurer>();
        
        // Saves the parent ai so we only have to get it once
        _parentAI = FindObjectOfType<Parent_AI>();
        
        // Gets the conjurers Status Effects
        _conjurerEffects = _conjurer.GetStatusEffects();

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
            StatusEffectManager();
            
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
            if (kvp.Key == ProjectileConjurer.StatusEffects.Fire && !_fireCoroutineRunning)
            {
                StartCoroutine(FireStatus(kvp.Value));
            }
            
            if (kvp.Key == ProjectileConjurer.StatusEffects.Slow && !_slowCoroutineRunning)
            {
                StartCoroutine(SlowStatus(kvp.Value));
            }
        }
    }
    
    private IEnumerator FireStatus(int duration)
    {
        _fireCoroutineRunning = true;
        for (int i = 0; i < duration; i++)
        {
            yield return new WaitForSeconds(1f);
            
            // Cant use damage enemy method because slow would consider this damage
            if (health < 5)
                health = 0;
            else
                health -= 5;
        }
        _fireCoroutineRunning = false;
    }
    
    private IEnumerator SlowStatus(int duration)
    {
        _slowCoroutineRunning = true;
        float originalSpeed = _parentAI.speed;
        _parentAI.speed *= 0.5f;
        
        yield return new WaitForSeconds(duration);
        
        _parentAI.speed = originalSpeed;
        _slowCoroutineRunning = false;
    }

}
