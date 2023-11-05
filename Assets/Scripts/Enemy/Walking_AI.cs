using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking_AI : Parent_AI
{

    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist_move = speed * Time.deltaTime;
        Vector2 destination = player.transform.position;
        destination.y = transform.position.y;
        transform.position = Vector2.MoveTowards(transform.position, destination, dist_move);
    }
}
