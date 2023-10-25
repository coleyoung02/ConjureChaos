using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float damage = 10f;

    private void Start()
    {
        ProjectileMove();
    }
    
    private void ProjectileMove()
    {
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D myCollider)
    {
        Debug.Log(myCollider.name);
        // This is where we will damage enemy if it is one
        // Prob keep track of enemies using tags
        // Prob will make a separate function to override in order to be able to choose damage based on what projectile is
        DamageEnemy();
        Destroy(gameObject);
    }

    private void DamageEnemy()
    {
        // call Alex's damage enemy method using Damage as parameter
    }
    
    // Im thinking instead of more projectiles we create abilities instead
    // Most of our powerups planned are either easy stuff like changing speed or damage which seems better to change the damage/speed straight out
    // Harder stuff like homing could be an ability that when they unlock adds that component ability script to the projectile
    // Making separate projectiles like we planned seems not the best idea since we arent having different projectiles but we will keep having to stack effects on top of each other instead of them being separate
    
}
