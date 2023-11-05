using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using Object = System.Object;

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
        if (myCollider.CompareTag("Enemy"))
        {
            // Is this the best way to do this?
            Enemy script = myCollider.gameObject.GetComponent<Enemy>();
            script.DamageEnemy(damage);
        }
        Destroy(gameObject);
    }

}
