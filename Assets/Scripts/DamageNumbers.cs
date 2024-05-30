using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] private TextMeshPro tm;
    private Vector2 velocity;
    private float alpha;
    private float decayRate;
    private float rotateRate;

    public void SetNumber(float number)
    {
        tm.text = Mathf.RoundToInt(number).ToString();
        velocity = new Vector2(UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(.45f, .65f));
        alpha = tm.color.a;
        decayRate = UnityEngine.Random.Range(.6f, .75f);
        rotateRate = -Mathf.Clamp(Mathf.Abs(velocity.x), .025f, .045f) * 1500f * Mathf.Sign(velocity.x);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotateRate * Time.deltaTime));
        Color c = Color.white;
        alpha -= decayRate * Time.deltaTime;
        c.a = alpha;
        if (alpha <= 0)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            tm.color = c;
        }
    }
}
