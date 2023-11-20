using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent_AI : MonoBehaviour
{
    protected GameObject player;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float speed = 1.0f;

    // Start is called before the first frame update
    public void Start()
    {
        if (!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
