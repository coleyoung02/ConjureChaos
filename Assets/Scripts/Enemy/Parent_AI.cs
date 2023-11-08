using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent_AI : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public float speed = 1.0f;

    // Start is called before the first frame update
    public void Start()
    {
        if (!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
