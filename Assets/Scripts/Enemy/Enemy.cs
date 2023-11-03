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

    // Start is called before the first frame update
    void Start()
    {
        if(!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
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
            health -= dmg;
            return false;
        }
    }

    public void die()
    { 
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        playerHealth.PlayerTakeDamage(contactDamage);
    }

}
