using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_AI : MonoBehaviour
{
    public GameObject player;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist_move = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,dist_move);
    }
}