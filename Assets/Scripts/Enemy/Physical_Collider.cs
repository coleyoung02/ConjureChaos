using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical_Collider : MonoBehaviour
{
    public Walking_AI script;

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if (transform.position.x - collision.gameObject.transform.position.x > 0)
                script.TurnAround(false);
            else
                script.TurnAround(true);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (transform.position.x - collision.gameObject.transform.position.x > 0)
                script.TurnAround(false);
            else
                script.TurnAround(true);
        }
    }

}
