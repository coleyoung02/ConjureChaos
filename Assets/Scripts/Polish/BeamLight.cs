using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BeamLight : MonoBehaviour
{
    private float startAngle;
    private float speed;
    private float angleRange;
    private float currentAngle;
    private int direction;


    void Awake()
    {
        startAngle = gameObject.transform.rotation.eulerAngles.z;
        angleRange = UnityEngine.Random.Range(20f, 45f);
        speed = UnityEngine.Random.Range(angleRange / 1.75f, angleRange / 3f);
        if (UnityEngine.Random.Range(0f, 1f) > .5f)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    private void Start()
    {
        currentAngle = UnityEngine.Random.Range(-angleRange / 2, angleRange / 2);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, currentAngle + startAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAngle > angleRange / 2)
        {
            direction = -1;
        }
        else if (currentAngle < - angleRange / 2)
        {
            direction = 1;
        }
        
        float diff = angleRange / 2f - Mathf.Abs(currentAngle);
        if (diff < 4f)
        {
            currentAngle += Time.deltaTime * Mathf.Lerp(speed / 10f, speed, diff/2f) * direction;
        }
        else
            currentAngle += Time.deltaTime * speed * direction;

        gameObject.transform.rotation = Quaternion.Euler(0, 0, currentAngle + startAngle);
    }
}
