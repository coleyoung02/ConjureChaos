using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] private TextMeshPro tm;
    [SerializeField] private Color yellow;
    [SerializeField] private int yellowCutoff;
    [SerializeField] private Color orange;
    [SerializeField] private int orangeCutoff;
    [SerializeField] private Color red;
    [SerializeField] private int redCutoff;
    private Vector2 velocity;
    private float alpha;
    private float decayRate;
    private float rotateRate;
    private Color colorToUse;

    public void SetNumber(float number)
    {
        tm.text = Mathf.RoundToInt(number).ToString();
        velocity = new Vector2(UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(.45f, .65f));
        decayRate = UnityEngine.Random.Range(.6f, .75f);
        rotateRate = -Mathf.Clamp(Mathf.Abs(velocity.x), .025f, .045f) * 1500f * Mathf.Sign(velocity.x);
        if (Mathf.RoundToInt(number) >= redCutoff)
        {
            colorToUse = red;
            gameObject.transform.localScale = Vector3.one * 1.3f;
        }
        else if (Mathf.RoundToInt(number) >= orangeCutoff)
        {
            colorToUse = orange;
            gameObject.transform.localScale = Vector3.one * 1.2f;
        }
        else if (Mathf.RoundToInt(number) >= yellowCutoff)
        {
            colorToUse = yellow;
            gameObject.transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            colorToUse = tm.color;
        }
        tm.color = colorToUse;
        alpha = tm.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotateRate * Time.deltaTime));
        Color c = colorToUse;
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
