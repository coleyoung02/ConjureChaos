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

    // Start is called before the first frame update
    void Start()
    {
        if(!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        progressManager = player.GetComponent<ProgressManager>();

        if (deathEvent == null)
            deathEvent = new UnityEvent();
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
            die();
        }
    }

}
