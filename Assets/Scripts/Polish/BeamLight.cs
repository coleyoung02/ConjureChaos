using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BeamLight : MonoBehaviour
{
    [SerializeField] private float startAngle;
    [SerializeField] private float speed;
    [SerializeField] private float angleRange;
    [SerializeField] private float currentAngle;
    [SerializeField] private int direction;

    void Awake()
    {
        startAngle = gameObject.transform.rotation.eulerAngles.z;
        angleRange = UnityEngine.Random.Range(15f, 35.5f);
        speed = UnityEngine.Random.Range(angleRange / 3, angleRange / 8);
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
        currentAngle += Time.deltaTime * speed * direction;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, currentAngle + startAngle);
    }
}
