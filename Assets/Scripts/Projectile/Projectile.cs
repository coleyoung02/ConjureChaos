using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    
    [SerializeField]
    private Rigidbody2D rb;
    
    private void Start()
    {
        ProjectileMove();
    }

    private void OnTriggerEnter2D(Collider2D myCollider)
    {
        Debug.Log(myCollider.name);
        // This is where we will damage enemy if it is one
        // Prob keep track of enemies using tags
        // Prob will make a separate function to override in order to be able to choose damage based on what projectile is
        Destroy(gameObject);
    }

    private void ProjectileMove()
    {
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
    }
}
